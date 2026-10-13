using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NexusERP.Application.Services
{
    public class PurchaseInvoiceService
        : IPurchaseInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseInvoiceService(
            IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(
                    "unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        public DateTime? CancelledAt { get; set; }

  
        public string CancelledBy { get; set; }


        public string CancellationReason { get; set; }


        public IEnumerable<PurchaseInvoice> GetAll()
        {
            return _unitOfWork.PurchaseInvoices
                .GetAll()
                .Where(invoice =>
                    !invoice.IsDeleted)
                .OrderByDescending(invoice =>
                    invoice.InvoiceDate)
                .ToList();
        }

        public PurchaseInvoice GetById(int id)
        {
            return _unitOfWork.PurchaseInvoices
                .GetAll()
                .FirstOrDefault(invoice =>
                    invoice.Id == id &&
                    !invoice.IsDeleted);
        }

        public void CreateDraft(
            PurchaseInvoice purchaseInvoice)
        {
            ValidateDraft(purchaseInvoice);

            if (string.IsNullOrWhiteSpace(purchaseInvoice.CreatedBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينشئ الفاتورة.");
            }

            purchaseInvoice.CreatedBy =
                purchaseInvoice.CreatedBy.Trim();

            if (purchaseInvoice.CreatedBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }


            if (InvoiceNumberExists(
                purchaseInvoice.InvoiceNumber))
            {
                throw new InvalidOperationException(
                    "رقم فاتورة المشتريات مستخدم مسبقًا.");
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    purchaseInvoice.PurchaseOrderId);

            purchaseInvoice.InvoiceNumber =
                purchaseInvoice.InvoiceNumber.Trim();

            purchaseInvoice.SupplierId =
                purchaseOrder.SupplierId;

            if (purchaseInvoice.InvoiceDate ==
                default(DateTime))
            {
                purchaseInvoice.InvoiceDate =
                    DateTime.Today;
            }

            purchaseInvoice.Status =
                PurchaseInvoiceStatus.Draft;

            purchaseInvoice.PostedAt = null;
            purchaseInvoice.PostedBy = null;
            purchaseInvoice.CancelledAt = null;
            purchaseInvoice.CancelledBy = null;
            purchaseInvoice.CancellationReason = null;

            purchaseInvoice.IsActive = true;
            purchaseInvoice.IsDeleted = false;
            purchaseInvoice.CreatedAt = DateTime.Now;
          



            foreach (PurchaseInvoiceItem invoiceItem
                in purchaseInvoice.Items)
            {
                invoiceItem.IsActive = true;
                invoiceItem.IsDeleted = false;
                invoiceItem.CreatedAt = DateTime.Now;
                invoiceItem.CreatedBy = purchaseInvoice.CreatedBy;
            }

            _unitOfWork.PurchaseInvoices.Add(
                purchaseInvoice);

            _unitOfWork.SaveChanges();
        }

        public void Post(
            int id,
            string postedBy)
        {
            if (string.IsNullOrWhiteSpace(postedBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينفذ الترحيل.");
            }

            postedBy = postedBy.Trim();

            if (postedBy.Length > 100)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 100 حرف.");
            }


            PurchaseInvoice invoice =
                GetById(id);

            if (invoice == null)
            {
                throw new InvalidOperationException(
                    "فاتورة المشتريات غير موجودة.");
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "لا يمكن ترحيل فاتورة ملغاة.");
            }

            if (invoice.Status !=
                PurchaseInvoiceStatus.Draft)
            {
                throw new InvalidOperationException(
                    "يمكن ترحيل الفاتورة المسودة فقط.");
            }

            if (invoice.Items == null ||
                !invoice.Items.Any(item =>
                    !item.IsDeleted))
            {
                throw new InvalidOperationException(
                    "الفاتورة لا تحتوي على أصناف.");
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    invoice.PurchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted ||
                purchaseOrder.Status ==
                    PurchaseOrderStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود أو ملغي.");
            }

            var activeItems = invoice.Items
    .Where(item => !item.IsDeleted)
    .ToList();

            bool hasDuplicateItems = activeItems
                .GroupBy(item => item.PurchaseOrderItemId)
                .Any(group => group.Count() > 1);

            if (hasDuplicateItems)
            {
                throw new InvalidOperationException(
                    "لا يمكن تكرار بند أمر الشراء في الفاتورة.");
            }

            foreach (PurchaseInvoiceItem item in activeItems)
            {
                ValidateInvoiceItem(
                    purchaseOrder,
                    item,
                    invoice.Id);
            }

            invoice.Status =
                PurchaseInvoiceStatus.Posted;

            invoice.PostedAt =
                DateTime.Now;

            invoice.PostedBy =
                string.IsNullOrWhiteSpace(postedBy)
                    ? null
                    : postedBy.Trim();

            invoice.UpdatedAt =
                DateTime.Now;

            invoice.UpdatedBy = postedBy;

            _unitOfWork.PurchaseInvoices.Update(
                invoice);

            // ترحيل الفاتورة لا ينشئ حركة مخزون.
            _unitOfWork.SaveChanges();
        }

        public void MarkAsPaid(int id)
        {
            PurchaseInvoice invoice =
                GetById(id);

            if (invoice == null)
            {
                throw new InvalidOperationException(
                    "فاتورة المشتريات غير موجودة.");
            }

            if (invoice.Status !=
                PurchaseInvoiceStatus.Posted)
            {
                throw new InvalidOperationException(
                    "يمكن دفع الفاتورة المُرحّلة فقط.");
            }

            invoice.Status =
                PurchaseInvoiceStatus.Paid;

            invoice.UpdatedAt =
                DateTime.Now;

            _unitOfWork.PurchaseInvoices.Update(
                invoice);

            _unitOfWork.SaveChanges();
        }

        public void Cancel(
       int id,
       string cancellationReason,
       string cancelledBy)
        {
            if (string.IsNullOrWhiteSpace(cancelledBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينفذ الإلغاء.");
            }

            cancelledBy = cancelledBy.Trim();

            if (cancelledBy.Length > 100)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 100 حرف.");
            }


            PurchaseInvoice invoice =
                GetById(id);

            if (invoice == null ||
                invoice.IsDeleted)
            {
                throw new InvalidOperationException(
                    "فاتورة المشتريات غير موجودة.");
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء الفاتورة مسبقًا.");
            }

            if (invoice.Status ==
                PurchaseInvoiceStatus.Paid)
            {
                throw new InvalidOperationException(
                    "لا يمكن إلغاء فاتورة مدفوعة.");
            }

            /*
             * منع إلغاء الفاتورة إذا كانت تحتوي
             * على دفعة جزئية أو كاملة غير ملغاة.
             */
            bool hasActivePayments =
                _unitOfWork.PurchasePayments
                    .GetAll()
                    .Any(payment =>
                        payment.PurchaseInvoiceId ==
                            invoice.Id &&
                        !payment.IsDeleted &&
                        !payment.IsCancelled);

            if (hasActivePayments)
            {
                throw new InvalidOperationException(
                    "لا يمكن إلغاء فاتورة تحتوي على دفعات مسجلة. " +
                    "يجب إلغاء الدفعات أولًا.");
            }

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                throw new InvalidOperationException(
                    "يجب إدخال سبب الإلغاء.");
            }

            cancellationReason =
                cancellationReason.Trim();

            if (cancellationReason.Length > 500)
            {
                throw new InvalidOperationException(
                    "سبب الإلغاء لا يمكن أن يتجاوز 500 حرف.");
            }

            invoice.Status =
                PurchaseInvoiceStatus.Cancelled;

            invoice.CancelledAt =
                DateTime.Now;

            invoice.CancelledBy =
                string.IsNullOrWhiteSpace(cancelledBy)
                    ? null
                    : cancelledBy.Trim();

            invoice.CancellationReason =
                cancellationReason;

            invoice.IsActive = false;
            invoice.UpdatedAt = DateTime.Now;
            invoice.UpdatedBy = invoice.CancelledBy;

            invoice.CancelledBy = cancelledBy;
            invoice.UpdatedBy = cancelledBy;

            _unitOfWork.PurchaseInvoices.Update(
                invoice);

            /*
             * إلغاء فاتورة المشتريات لا يعدّل المخزون؛
             * لأن المخزون مرتبط بمستند الاستلام.
             */
            _unitOfWork.SaveChanges();
        }

        public bool InvoiceNumberExists(
            string invoiceNumber,
            int? excludedId = null)
        {
            if (string.IsNullOrWhiteSpace(
                invoiceNumber))
            {
                return false;
            }

            string normalizedNumber =
                invoiceNumber.Trim().ToLower();

            return _unitOfWork.PurchaseInvoices
                .GetAll()
                .Any(invoice =>
                    !invoice.IsDeleted &&
                    invoice.InvoiceNumber.ToLower() ==
                        normalizedNumber &&
                    (!excludedId.HasValue ||
                     invoice.Id != excludedId.Value));
        }

        private void ValidateDraft(
            PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice == null)
            {
                throw new ArgumentNullException(
                    "purchaseInvoice");
            }

            if (string.IsNullOrWhiteSpace(
                purchaseInvoice.InvoiceNumber))
            {
                throw new InvalidOperationException(
                    "رقم فاتورة المشتريات مطلوب.");
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    purchaseInvoice.PurchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                    PurchaseOrderStatus.PartiallyReceived &&
                purchaseOrder.Status !=
                    PurchaseOrderStatus.Received)
            {
                throw new InvalidOperationException(
                    "يمكن إنشاء فاتورة فقط لأمر شراء " +
                    "تم استلامه جزئيًا أو بالكامل.");
            }

            if (purchaseInvoice.Items == null ||
                !purchaseInvoice.Items.Any())
            {
                throw new InvalidOperationException(
                    "يجب إضافة صنف واحد على الأقل.");
            }

            bool hasDuplicateItems =
                purchaseInvoice.Items
                    .GroupBy(item =>
                        item.PurchaseOrderItemId)
                    .Any(group =>
                        group.Count() > 1);

            if (hasDuplicateItems)
            {
                throw new InvalidOperationException(
                    "لا يمكن تكرار بند أمر الشراء في الفاتورة.");
            }

            foreach (PurchaseInvoiceItem invoiceItem
                in purchaseInvoice.Items)
            {
                ValidateInvoiceItem(
                    purchaseOrder,
                    invoiceItem);
            }
        }

        private void ValidateInvoiceItem(
   PurchaseOrder purchaseOrder,
   PurchaseInvoiceItem invoiceItem,
   int? excludedInvoiceId = null)
        {
            PurchaseOrderItem orderItem =
                _unitOfWork.PurchaseOrderItems
                    .GetById(
                        invoiceItem.PurchaseOrderItemId);

            if (orderItem == null ||
                orderItem.IsDeleted ||
                orderItem.PurchaseOrderId !=
                    purchaseOrder.Id)
            {
                throw new InvalidOperationException(
                    "أحد بنود الفاتورة لا ينتمي " +
                    "إلى أمر الشراء.");
            }

            if (invoiceItem.Quantity <= 0)
            {
                throw new InvalidOperationException(
                    "كمية الفاتورة يجب أن تكون أكبر من صفر.");
            }

            if (invoiceItem.UnitPrice < 0)
            {
                throw new InvalidOperationException(
                    "سعر الوحدة لا يمكن أن يكون سالبًا.");
            }

            if (invoiceItem.TaxRate < 0 ||
                invoiceItem.TaxRate > 100)
            {
                throw new InvalidOperationException(
                    "نسبة الضريبة يجب أن تكون بين 0 و100.");
            }

            decimal receivedQuantity =
                GetReceivedQuantity(orderItem.Id);

            decimal invoicedQuantity =
                GetInvoicedQuantity(orderItem.Id);

            decimal availableQuantity =
    CalculateAvailableQuantity(
        orderItem.Id,
        excludedInvoiceId);

            if (invoiceItem.Quantity > availableQuantity)
            {
                string itemName = orderItem.Item != null
                    ? orderItem.Item.NameAr
                    : orderItem.Id.ToString();

                throw new InvalidOperationException(
                    "كمية الفاتورة للصنف \"" + itemName +
                    "\" أكبر من المتاح للفوترة بعد خصم " +
                    "المرتجعات والفواتير الأخرى. المتاح: " +
                    availableQuantity.ToString("0.##"));
            }
        }

        public decimal GetReceivedQuantity(
            int purchaseOrderItemId)
        {
            return _unitOfWork.PurchaseReceiptItems
                .GetAll()
                .Where(receiptItem =>
                    receiptItem.PurchaseOrderItemId ==
                        purchaseOrderItemId &&
                    !receiptItem.IsDeleted &&
                    receiptItem.PurchaseReceipt != null &&
                    receiptItem.PurchaseReceipt.IsPosted &&
                    !receiptItem.PurchaseReceipt.IsCancelled &&
                    !receiptItem.PurchaseReceipt.IsDeleted)
                .Sum(receiptItem =>
                    (decimal?)
                    receiptItem.ReceivedQuantity) ?? 0;
        }

        public decimal GetInvoicedQuantity(
            int purchaseOrderItemId)
        {
            return _unitOfWork.PurchaseInvoiceItems
                .GetAll()
                .Where(invoiceItem =>
                    invoiceItem.PurchaseOrderItemId ==
                        purchaseOrderItemId &&
                    !invoiceItem.IsDeleted &&
                    invoiceItem.PurchaseInvoice != null &&
                    !invoiceItem.PurchaseInvoice.IsDeleted &&
                    invoiceItem.PurchaseInvoice.Status !=
                        PurchaseInvoiceStatus.Cancelled)
                .Sum(invoiceItem =>
                    (decimal?)
                    invoiceItem.Quantity) ?? 0;
        }

        //داله الكميه المتاحه
        public decimal GetAvailableQuantity(
      int purchaseOrderItemId)
        {
            return CalculateAvailableQuantity(
                purchaseOrderItemId,
                null);
        }

        private decimal CalculateAvailableQuantity(
            int purchaseOrderItemId,
            int? excludedInvoiceId)
        {
            PurchaseOrderItem orderItem =
                _unitOfWork.PurchaseOrderItems.GetById(
                    purchaseOrderItemId);

            if (orderItem == null || orderItem.IsDeleted)
            {
                return 0;
            }

            decimal receivedQuantity =
                GetReceivedQuantity(purchaseOrderItemId);

            decimal returnedQuantity =
                _unitOfWork.PurchaseReturnItems
                    .GetAll()
                    .Where(item =>
                        !item.IsDeleted &&
                        item.PurchaseReceiptItem != null &&
                        item.PurchaseReceiptItem.PurchaseOrderItemId ==
                            purchaseOrderItemId &&
                        item.PurchaseReturn != null &&
                        !item.PurchaseReturn.IsDeleted &&
                        item.PurchaseReturn.Status ==
                            PurchaseReturnStatus.Posted)
                    .Sum(item =>
                        (decimal?)item.ReturnedQuantity) ?? 0;

            decimal invoicedQuantity =
                _unitOfWork.PurchaseInvoiceItems
                    .GetAll()
                    .Where(item =>
                        !item.IsDeleted &&
                        item.PurchaseOrderItemId ==
                            purchaseOrderItemId &&
                        item.PurchaseInvoice != null &&
                        !item.PurchaseInvoice.IsDeleted &&
                        item.PurchaseInvoice.Status !=
                            PurchaseInvoiceStatus.Cancelled &&
                        (!excludedInvoiceId.HasValue ||
                         item.PurchaseInvoiceId !=
                            excludedInvoiceId.Value))
                    .Sum(item =>
                        (decimal?)item.Quantity) ?? 0;

            return Math.Max(
                0m,
                receivedQuantity -
                returnedQuantity -
                invoicedQuantity);
        }


    }
}