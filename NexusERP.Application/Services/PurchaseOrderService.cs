using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PurchaseOrder> GetAll()
        {
            return _unitOfWork.PurchaseOrders
                .GetAll()
                .Where(order => !order.IsDeleted)
                .OrderByDescending(order => order.OrderDate)
                .ToList();
        }

        public PurchaseOrder GetById(int id)
        {
            return _unitOfWork.PurchaseOrders
                .GetAll()
                .FirstOrDefault(order =>
                    order.Id == id &&
                    !order.IsDeleted);
        }

        public void Create(PurchaseOrder purchaseOrder)
        {
            ValidatePurchaseOrder(purchaseOrder);

            if (OrderNumberExists(purchaseOrder.OrderNumber))
            {
                throw new InvalidOperationException(
                    "رقم أمر الشراء مستخدم مسبقًا.");
            }

            purchaseOrder.OrderNumber =
                purchaseOrder.OrderNumber.Trim();

            purchaseOrder.CreatedAt = DateTime.Now;
            purchaseOrder.IsActive = true;
            purchaseOrder.IsDeleted = false;

            foreach (var orderItem in purchaseOrder.Items)
            {
                orderItem.CreatedAt = DateTime.Now;
                orderItem.IsActive = true;
                orderItem.IsDeleted = false;
            }

            _unitOfWork.PurchaseOrders.Add(purchaseOrder);
            _unitOfWork.SaveChanges();
        }

        public void Update(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException("purchaseOrder");
            }

            ValidatePurchaseOrder(purchaseOrder);

            if (OrderNumberExists(
                purchaseOrder.OrderNumber,
                purchaseOrder.Id))
            {
                throw new InvalidOperationException(
                    "رقم أمر الشراء مستخدم مسبقًا.");
            }

            var existingOrder =
                _unitOfWork.PurchaseOrders.GetById(purchaseOrder.Id);

            if (existingOrder == null || existingOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            existingOrder.OrderNumber =
                purchaseOrder.OrderNumber.Trim();

            existingOrder.OrderDate = purchaseOrder.OrderDate;
            existingOrder.SupplierId = purchaseOrder.SupplierId;
            existingOrder.Notes = purchaseOrder.Notes;
            existingOrder.UpdatedAt = DateTime.Now;

            _unitOfWork.PurchaseOrders.Update(existingOrder);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var purchaseOrder =
                _unitOfWork.PurchaseOrders
                    .GetById(id);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                PurchaseOrderStatus.Draft)
            {
                throw new InvalidOperationException(
                    "لا يمكن حذف أمر الشراء بعد اعتماده. " +
                    "يمكن حذف أوامر الشراء المسودة فقط.");
            }

            purchaseOrder.IsDeleted = true;
            purchaseOrder.IsActive = false;
            purchaseOrder.UpdatedAt = DateTime.Now;

            foreach (var orderItem
                in purchaseOrder.Items
                    .Where(item => !item.IsDeleted))
            {
                orderItem.IsDeleted = true;
                orderItem.IsActive = false;
                orderItem.UpdatedAt = DateTime.Now;

                _unitOfWork.PurchaseOrderItems.Update(
                    orderItem);
            }

            _unitOfWork.PurchaseOrders.Update(
                purchaseOrder);

            _unitOfWork.SaveChanges();
        }

        public bool OrderNumberExists(
            string orderNumber,
            int? excludedId = null)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return false;
            }

            string normalizedOrderNumber =
                orderNumber.Trim().ToLower();

            return _unitOfWork.PurchaseOrders
                .GetAll()
                .Any(order =>
                    !order.IsDeleted &&
                    order.OrderNumber.ToLower() ==
                        normalizedOrderNumber &&
                    (!excludedId.HasValue ||
                     order.Id != excludedId.Value));
        }

        private void ValidatePurchaseOrder(
            PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException("purchaseOrder");
            }

            if (string.IsNullOrWhiteSpace(
                purchaseOrder.OrderNumber))
            {
                throw new InvalidOperationException(
                    "رقم أمر الشراء مطلوب.");
            }

            if (purchaseOrder.OrderDate == DateTime.MinValue)
            {
                throw new InvalidOperationException(
                    "تاريخ أمر الشراء مطلوب.");
            }

            var supplier =
                _unitOfWork.Suppliers.GetById(
                    purchaseOrder.SupplierId);

            if (supplier == null || supplier.IsDeleted)
            {
                throw new InvalidOperationException(
                    "المورد المحدد غير موجود.");
            }

            if (purchaseOrder.Items == null ||
                !purchaseOrder.Items.Any())
            {
                throw new InvalidOperationException(
                    "يجب إضافة صنف واحد على الأقل.");
            }

            if (purchaseOrder.Items
                .GroupBy(item => item.ItemId)
                .Any(group => group.Count() > 1))
            {
                throw new InvalidOperationException(
                    "لا يمكن تكرار الصنف في أمر الشراء.");
            }

            foreach (var orderItem in purchaseOrder.Items)
            {
                var item =
                    _unitOfWork.Items.GetById(orderItem.ItemId);

                if (item == null || item.IsDeleted)
                {
                    throw new InvalidOperationException(
                        "أحد الأصناف المحددة غير موجود.");
                }

                if (orderItem.Quantity <= 0)
                {
                    throw new InvalidOperationException(
                        "كمية الصنف يجب أن تكون أكبر من صفر.");
                }

                if (orderItem.UnitPrice < 0)
                {
                    throw new InvalidOperationException(
                        "سعر الصنف لا يمكن أن يكون سالبًا.");
                }
            }
        }

        public void Approve(int id, string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(approvedBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي يعتمد أمر الشراء.");
            }

            approvedBy = approvedBy.Trim();

            if (approvedBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }

            var purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(id);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                PurchaseOrderStatus.Draft)
            {
                throw new InvalidOperationException(
                    "يمكن اعتماد أوامر الشراء المسودة فقط.");
            }

            if (purchaseOrder.Items == null ||
                !purchaseOrder.Items.Any(item => !item.IsDeleted))
            {
                throw new InvalidOperationException(
                    "لا يمكن اعتماد أمر شراء لا يحتوي على أصناف.");
            }

            purchaseOrder.Status = PurchaseOrderStatus.Approved;
            purchaseOrder.UpdatedAt = DateTime.Now;
            purchaseOrder.UpdatedBy = approvedBy;
            purchaseOrder.ApprovedBy = approvedBy;
            purchaseOrder.ApprovedAt = DateTime.Now;

            _unitOfWork.PurchaseOrders.Update(purchaseOrder);
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

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(id);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status ==
                PurchaseOrderStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء أمر الشراء مسبقًا.");
            }

            if (purchaseOrder.Status ==
                    PurchaseOrderStatus.PartiallyReceived ||
                purchaseOrder.Status ==
                    PurchaseOrderStatus.Received)
            {
                throw new InvalidOperationException(
                    "لا يمكن إلغاء أمر شراء يحتوي على " +
                    "استلامات مُرحّلة. يجب إلغاء مستندات " +
                    "الاستلام أولًا.");
            }

            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                throw new InvalidOperationException(
                    "يجب إدخال سبب إلغاء أمر الشراء.");
            }

            cancellationReason = cancellationReason.Trim();

            if (cancellationReason.Length > 500)
            {
                throw new InvalidOperationException(
                    "سبب الإلغاء لا يمكن أن يتجاوز 500 حرف.");
            }

            DateTime cancelledAt = DateTime.Now;

            purchaseOrder.Status = PurchaseOrderStatus.Cancelled;
            purchaseOrder.CancelledAt = cancelledAt;
            purchaseOrder.CancelledBy = cancelledBy;
            purchaseOrder.CancellationReason = cancellationReason;
            purchaseOrder.IsActive = false;

            purchaseOrder.UpdatedAt = cancelledAt;
            purchaseOrder.UpdatedBy = cancelledBy;

            _unitOfWork.PurchaseOrders.Update(purchaseOrder);
            _unitOfWork.SaveChanges();
        }
    }
}