using System;
using System.Collections.Generic;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;

namespace NexusERP.Application.Services
{
    public class PurchasePaymentService :
        IPurchasePaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchasePaymentService(
            IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(
                    "unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PurchasePayment> GetAll()
        {
            return _unitOfWork.PurchasePayments
                .GetAll()
                .Where(payment =>
                    !payment.IsDeleted)
                .OrderByDescending(payment =>
                    payment.PaymentDate)
                .ThenByDescending(payment =>
                    payment.Id)
                .ToList();
        }

        public PurchasePayment GetById(int id)
        {
            return _unitOfWork.PurchasePayments
                .GetAll()
                .FirstOrDefault(payment =>
                    payment.Id == id &&
                    !payment.IsDeleted);
        }

        public IEnumerable<PurchasePayment>
            GetByInvoiceId(int purchaseInvoiceId)
        {
            return _unitOfWork.PurchasePayments
                .GetAll()
                .Where(payment =>
                    !payment.IsDeleted &&
                    payment.PurchaseInvoiceId ==
                        purchaseInvoiceId)
                .OrderByDescending(payment =>
                    payment.PaymentDate)
                .ThenByDescending(payment =>
                    payment.Id)
                .ToList();
        }

        public decimal GetInvoiceTotal(
    int purchaseInvoiceId)
        {
            PurchaseInvoice invoice =
                _unitOfWork.PurchaseInvoices.GetById(
                    purchaseInvoiceId);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                return 0;
            }

            var activeItems =
                invoice.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .ToList();

            decimal subtotal =
                activeItems.Sum(item =>
                    item.Quantity *
                    item.UnitPrice);

            decimal discountAmount =
                invoice.DiscountAmount;

            if (discountAmount < 0)
            {
                discountAmount = 0;
            }

            if (discountAmount > subtotal)
            {
                discountAmount = subtotal;
            }

            decimal taxAmount =
                activeItems.Sum(item =>
                    item.Quantity *
                    item.UnitPrice *
                    item.TaxRate / 100m);

            decimal invoiceTotal =
                subtotal -
                discountAmount +
                taxAmount;

            return invoiceTotal;
        }

        public decimal GetPaidAmount(
            int purchaseInvoiceId)
        {
            return _unitOfWork.PurchasePayments
                .GetAll()
                .Where(payment =>
                    !payment.IsDeleted &&
                    !payment.IsCancelled &&
                    payment.PurchaseInvoiceId ==
                        purchaseInvoiceId)
                .Select(payment =>
                    payment.Amount)
                .DefaultIfEmpty(0)
                .Sum();
        }

        public decimal GetRemainingAmount(
            int purchaseInvoiceId)
        {
            decimal invoiceTotal =
                GetInvoiceTotal(purchaseInvoiceId);

            decimal paidAmount =
                GetPaidAmount(purchaseInvoiceId);

            decimal remainingAmount =
                invoiceTotal - paidAmount;

            return remainingAmount > 0
                ? remainingAmount
                : 0;
        }

        public int Create(
            PurchasePayment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(
                    "payment");
            }
            if (string.IsNullOrWhiteSpace(payment.CreatedBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينشئ الدفعة.");
            }

            payment.CreatedBy = payment.CreatedBy.Trim();

            if (payment.CreatedBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }

            if (string.IsNullOrWhiteSpace(
                payment.PaymentNumber))
            {
                throw new InvalidOperationException(
                    "رقم الدفعة مطلوب.");
            }

            payment.PaymentNumber =
                payment.PaymentNumber.Trim();

            bool duplicateNumber =
                _unitOfWork.PurchasePayments
                    .GetAll()
                    .Any(existingPayment =>
                        !existingPayment.IsDeleted &&
                        existingPayment.PaymentNumber ==
                            payment.PaymentNumber);

            if (duplicateNumber)
            {
                throw new InvalidOperationException(
                    "رقم الدفعة مستخدم مسبقًا.");
            }

            PurchaseInvoice invoice =
                _unitOfWork.PurchaseInvoices.GetById(
                    payment.PurchaseInvoiceId);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                throw new InvalidOperationException(
                    "فاتورة المشتريات غير موجودة.");
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Draft)
            {
                throw new InvalidOperationException(
                    "يجب ترحيل الفاتورة قبل تسجيل دفعة.");
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "لا يمكن دفع فاتورة ملغاة.");
            }

            if (payment.Amount <= 0)
            {
                throw new InvalidOperationException(
                    "مبلغ الدفعة يجب أن يكون أكبر من صفر.");
            }

            decimal remainingAmount =
                GetRemainingAmount(
                    payment.PurchaseInvoiceId);

            if (remainingAmount <= 0)
            {
                throw new InvalidOperationException(
                    "تم دفع الفاتورة بالكامل.");
            }

            if (payment.Amount >
                remainingAmount)
            {
                throw new InvalidOperationException(
                    "مبلغ الدفعة أكبر من المبلغ المتبقي.");
            }

            if (payment.PaymentDate ==
                DateTime.MinValue)
            {
                payment.PaymentDate =
                    DateTime.Now;
            }

            payment.ReferenceNumber =
                string.IsNullOrWhiteSpace(
                    payment.ReferenceNumber)
                    ? null
                    : payment.ReferenceNumber.Trim();

            payment.Notes =
                string.IsNullOrWhiteSpace(
                    payment.Notes)
                    ? null
                    : payment.Notes.Trim();

            payment.IsCancelled = false;
            payment.CancelledAt = null;
            payment.CancelledBy = null;
            payment.CancellationReason = null;

            payment.IsActive = true;
            payment.IsDeleted = false;
            payment.CreatedAt = DateTime.Now;

            _unitOfWork.PurchasePayments.Add(
                payment);

            decimal paidAfterPayment =
                GetPaidAmount(invoice.Id) +
                payment.Amount;

            decimal invoiceTotal =
                GetInvoiceTotal(invoice.Id);

            invoice.Status =
                paidAfterPayment >= invoiceTotal
                    ? PurchaseInvoiceStatus.Paid
                    : PurchaseInvoiceStatus.Posted;

            invoice.UpdatedAt = DateTime.Now;
            invoice.UpdatedBy = payment.CreatedBy;


            _unitOfWork.PurchaseInvoices.Update(
                invoice);

            _unitOfWork.SaveChanges();

            return payment.Id;
        }

        public void Cancel(
            int paymentId,
            string cancellationReason,
            string cancelledBy)
        {

            if (string.IsNullOrWhiteSpace(cancelledBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينفذ الإلغاء.");
            }

            cancelledBy = cancelledBy.Trim();

            if (cancelledBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }

            PurchasePayment payment =
                GetById(paymentId);

            if (payment == null)
            {
                throw new InvalidOperationException(
                    "الدفعة غير موجودة.");
            }

            if (payment.IsCancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء الدفعة مسبقًا.");
            }

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                throw new InvalidOperationException(
                    "سبب إلغاء الدفعة مطلوب.");
            }

            cancellationReason =
                cancellationReason.Trim();

            if (cancellationReason.Length > 500)
            {
                throw new InvalidOperationException(
                    "سبب الإلغاء لا يمكن أن يتجاوز 500 حرف.");
            }

            PurchaseInvoice invoice =
                _unitOfWork.PurchaseInvoices.GetById(
                    payment.PurchaseInvoiceId);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                throw new InvalidOperationException(
                    "فاتورة المشتريات غير موجودة.");
            }

            payment.IsCancelled = true;
            payment.CancelledAt = DateTime.Now;

            payment.CancelledBy = cancelledBy;

            payment.CancellationReason =
                cancellationReason;

            payment.IsActive = false;
            payment.UpdatedAt = DateTime.Now;
            payment.UpdatedBy =
                payment.CancelledBy;

            _unitOfWork.PurchasePayments.Update(
                payment);

            decimal paidAfterCancellation =
                _unitOfWork.PurchasePayments
                    .GetAll()
                    .Where(existingPayment =>
                        !existingPayment.IsDeleted &&
                        !existingPayment.IsCancelled &&
                        existingPayment.Id !=
                            payment.Id &&
                        existingPayment
                            .PurchaseInvoiceId ==
                                invoice.Id)
                    .Select(existingPayment =>
                        existingPayment.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

            decimal invoiceTotal =
                GetInvoiceTotal(invoice.Id);

            invoice.Status =
                paidAfterCancellation >= invoiceTotal &&
                invoiceTotal > 0
                    ? PurchaseInvoiceStatus.Paid
                    : PurchaseInvoiceStatus.Posted;

            invoice.UpdatedAt = DateTime.Now;
            invoice.UpdatedBy =
                payment.CancelledBy;

            _unitOfWork.PurchaseInvoices.Update(
                invoice);

            _unitOfWork.SaveChanges();
        }
    }
}