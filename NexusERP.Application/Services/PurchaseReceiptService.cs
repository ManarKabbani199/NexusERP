using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Application.Services
{
    public class PurchaseReceiptService
        : IPurchaseReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IStockMovementService
            _stockMovementService;

        public PurchaseReceiptService(
            IUnitOfWork unitOfWork,
            IStockMovementService stockMovementService)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(
                    "unitOfWork");
            }

            if (stockMovementService == null)
            {
                throw new ArgumentNullException(
                    "stockMovementService");
            }

            _unitOfWork = unitOfWork;
            _stockMovementService =
                stockMovementService;
        }

        public IEnumerable<PurchaseReceipt> GetAll()
        {
            return _unitOfWork.PurchaseReceipts
                .GetAll()
                .Where(receipt =>
                    !receipt.IsDeleted)
                .OrderByDescending(receipt =>
                    receipt.ReceiptDate)
                .ToList();
        }

        public PurchaseReceipt GetById(int id)
        {
            return _unitOfWork.PurchaseReceipts
                .GetAll()
                .FirstOrDefault(receipt =>
                    receipt.Id == id &&
                    !receipt.IsDeleted);
        }

        public void CreateDraft(
            PurchaseReceipt purchaseReceipt)
        {
            ValidateDraft(purchaseReceipt);

            if (ReceiptNumberExists(
                purchaseReceipt.ReceiptNumber))
            {
                throw new InvalidOperationException(
                    "رقم مستند الاستلام مستخدم مسبقًا.");
            }

            purchaseReceipt.ReceiptNumber =
                purchaseReceipt.ReceiptNumber.Trim();

            if (purchaseReceipt.ReceiptDate ==
                default(DateTime))
            {
                purchaseReceipt.ReceiptDate =
                    DateTime.Today;
            }

            purchaseReceipt.IsPosted = false;
            purchaseReceipt.PostedAt = null;
            purchaseReceipt.IsCancelled = false;
            purchaseReceipt.CancelledAt = null;
            purchaseReceipt.IsActive = true;
            purchaseReceipt.IsDeleted = false;
            purchaseReceipt.CreatedAt = DateTime.Now;

            foreach (var receiptItem
                in purchaseReceipt.Items)
            {
                receiptItem.IsActive = true;
                receiptItem.IsDeleted = false;
                receiptItem.CreatedAt = DateTime.Now;
            }

            _unitOfWork.PurchaseReceipts.Add(
                purchaseReceipt);

            _unitOfWork.SaveChanges();
        }

        public void Post(int id, string postedBy)
        {
            if (string.IsNullOrWhiteSpace(postedBy))
            {
                throw new InvalidOperationException(
                    "يجب تحديد المستخدم الذي ينفذ الترحيل.");
            }

            postedBy = postedBy.Trim();

            if (postedBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }
            PurchaseReceipt receipt =
                GetById(id);

            if (receipt == null)
            {
                throw new InvalidOperationException(
                    "مستند الاستلام غير موجود.");
            }

            if (receipt.IsCancelled)
            {
                throw new InvalidOperationException(
                    "لا يمكن ترحيل مستند استلام ملغي.");
            }

            if (receipt.IsPosted)
            {
                throw new InvalidOperationException(
                    "تم ترحيل مستند الاستلام مسبقًا.");
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    receipt.PurchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                    PurchaseOrderStatus.Approved &&
                purchaseOrder.Status !=
                    PurchaseOrderStatus.PartiallyReceived)
            {
                throw new InvalidOperationException(
                    "يمكن الاستلام من أمر شراء معتمد " +
                    "أو مستلم جزئيًا فقط.");
            }

            var receiptItems = receipt.Items
                .Where(item => !item.IsDeleted)
                .ToList();

            if (!receiptItems.Any())
            {
                throw new InvalidOperationException(
                    "مستند الاستلام لا يحتوي على أصناف.");
            }

            foreach (PurchaseReceiptItem receiptItem
                in receiptItems)
            {
                PurchaseOrderItem orderItem =
                    _unitOfWork.PurchaseOrderItems
                        .GetById(
                            receiptItem
                                .PurchaseOrderItemId);

                if (orderItem == null ||
                    orderItem.IsDeleted ||
                    orderItem.PurchaseOrderId !=
                        receipt.PurchaseOrderId)
                {
                    throw new InvalidOperationException(
                        "أحد بنود الاستلام لا ينتمي " +
                        "إلى أمر الشراء.");
                }

                if (receiptItem.ReceivedQuantity <= 0)
                {
                    throw new InvalidOperationException(
                        "الكمية المستلمة يجب أن تكون " +
                        "أكبر من صفر.");
                }

                decimal remainingQuantity =
                    GetRemainingQuantity(orderItem.Id);

                if (receiptItem.ReceivedQuantity >
                    remainingQuantity)
                {
                    throw new InvalidOperationException(
                        "الكمية المستلمة للصنف " +
                        orderItem.Item.NameAr +
                        " أكبر من الكمية المتبقية.");
                }

                var movement = new StockMovement
                {

                    WarehouseId =
                        receipt.WarehouseId,

                    ItemId = orderItem.ItemId,

                    Quantity =
                        receiptItem.ReceivedQuantity,

                    MovementType =
                        StockMovementType.Receipt,

                    MovementDate =
                        receipt.ReceiptDate,

                    ReferenceNumber =
                        "PR-" +
                        receipt.ReceiptNumber,

                    PurchaseReceiptId = receipt.Id,

                    Notes =
                        "استلام مشتريات من أمر الشراء " +
                        purchaseOrder.OrderNumber,

                    CreatedAt = DateTime.Now,
                    CreatedBy = postedBy,

                    IsActive = true,
                    IsDeleted = false,
                    IsCancelled = false
                };

                _stockMovementService.CreateMovement(
                    movement,
                    false);
            }

            receipt.IsPosted = true;
            receipt.PostedAt = DateTime.Now;
            receipt.PostedBy = postedBy;

            receipt.UpdatedAt = DateTime.Now;
            receipt.UpdatedBy = postedBy;

            _unitOfWork.PurchaseReceipts.Update(
                receipt);

            UpdatePurchaseOrderStatus(
                purchaseOrder,
                receiptItems);

            // يحفظ مستند الاستلام والحركات
            // والأرصدة وحالة أمر الشراء معًا.
            _unitOfWork.SaveChanges();
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
                    !receiptItem.PurchaseReceipt
                        .IsCancelled &&
                    !receiptItem.PurchaseReceipt
                        .IsDeleted)
                .Sum(receiptItem =>
                    (decimal?)
                    receiptItem.ReceivedQuantity) ?? 0;
        }

        public decimal GetRemainingQuantity(
            int purchaseOrderItemId)
        {
            PurchaseOrderItem orderItem =
                _unitOfWork.PurchaseOrderItems
                    .GetById(purchaseOrderItemId);

            if (orderItem == null ||
                orderItem.IsDeleted)
            {
                throw new InvalidOperationException(
                    "بند أمر الشراء غير موجود.");
            }

            decimal receivedQuantity =
                GetReceivedQuantity(
                    purchaseOrderItemId);

            decimal remainingQuantity =
                orderItem.Quantity -
                receivedQuantity;

            return remainingQuantity < 0
                ? 0
                : remainingQuantity;
        }

        public bool ReceiptNumberExists(
            string receiptNumber,
            int? excludedId = null)
        {
            if (string.IsNullOrWhiteSpace(
                receiptNumber))
            {
                return false;
            }

            string normalizedNumber =
                receiptNumber.Trim().ToLower();

            return _unitOfWork.PurchaseReceipts
                .GetAll()
                .Any(receipt =>
                    !receipt.IsDeleted &&
                    receipt.ReceiptNumber.ToLower() ==
                        normalizedNumber &&
                    (!excludedId.HasValue ||
                     receipt.Id !=
                        excludedId.Value));
        }

        private void ValidateDraft(
            PurchaseReceipt purchaseReceipt)
        {
            if (purchaseReceipt == null)
            {
                throw new ArgumentNullException(
                    "purchaseReceipt");
            }

            if (string.IsNullOrWhiteSpace(
                purchaseReceipt.ReceiptNumber))
            {
                throw new InvalidOperationException(
                    "رقم مستند الاستلام مطلوب.");
            }

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    purchaseReceipt.PurchaseOrderId);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                    PurchaseOrderStatus.Approved &&
                purchaseOrder.Status !=
                    PurchaseOrderStatus.PartiallyReceived)
            {
                throw new InvalidOperationException(
                    "يمكن إنشاء استلام من أمر شراء " +
                    "معتمد أو مستلم جزئيًا فقط.");
            }

            Warehouse warehouse =
                _unitOfWork.Warehouses.GetById(
                    purchaseReceipt.WarehouseId);

            if (warehouse == null ||
                warehouse.IsDeleted ||
                !warehouse.IsActive)
            {
                throw new InvalidOperationException(
                    "المستودع المحدد غير موجود أو غير فعال.");
            }

            if (purchaseReceipt.Items == null ||
                !purchaseReceipt.Items.Any())
            {
                throw new InvalidOperationException(
                    "يجب إضافة صنف واحد على الأقل.");
            }

            bool hasDuplicateItems =
                purchaseReceipt.Items
                    .GroupBy(item =>
                        item.PurchaseOrderItemId)
                    .Any(group =>
                        group.Count() > 1);

            if (hasDuplicateItems)
            {
                throw new InvalidOperationException(
                    "لا يمكن تكرار بند أمر الشراء " +
                    "في مستند الاستلام.");
            }

            foreach (PurchaseReceiptItem receiptItem
                in purchaseReceipt.Items)
            {
                PurchaseOrderItem orderItem =
                    _unitOfWork.PurchaseOrderItems
                        .GetById(
                            receiptItem
                                .PurchaseOrderItemId);

                if (orderItem == null ||
                    orderItem.IsDeleted ||
                    orderItem.PurchaseOrderId !=
                        purchaseReceipt.PurchaseOrderId)
                {
                    throw new InvalidOperationException(
                        "أحد البنود لا ينتمي إلى " +
                        "أمر الشراء المحدد.");
                }

                if (receiptItem.ReceivedQuantity <= 0)
                {
                    throw new InvalidOperationException(
                        "الكمية المستلمة يجب أن تكون " +
                        "أكبر من صفر.");
                }

                decimal remainingQuantity =
                    GetRemainingQuantity(orderItem.Id);

                if (receiptItem.ReceivedQuantity >
                    remainingQuantity)
                {
                    throw new InvalidOperationException(
                        "الكمية المستلمة للصنف " +
                        orderItem.Item.NameAr +
                        " أكبر من الكمية المتبقية.");
                }
            }
        }

        private void UpdatePurchaseOrderStatus(
          PurchaseOrder purchaseOrder,
          IList<PurchaseReceiptItem> currentItems)
        {
            var orderItems =
                purchaseOrder.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .ToList();

            int currentReceiptId =
                currentItems
                    .Select(item =>
                        item.PurchaseReceiptId)
                    .FirstOrDefault();

            bool hasAnyReceivedQuantity = false;
            bool allItemsFullyReceived =
                orderItems.Any();

            foreach (PurchaseOrderItem orderItem
                in orderItems)
            {
                // الاستلامات السابقة فقط،
                // مع استبعاد مستند الاستلام الحالي.
                decimal previouslyReceived =
                    _unitOfWork.PurchaseReceiptItems
                        .GetAll()
                        .Where(receiptItem =>
                            receiptItem.PurchaseOrderItemId ==
                                orderItem.Id &&
                            receiptItem.PurchaseReceiptId !=
                                currentReceiptId &&
                            !receiptItem.IsDeleted &&
                            receiptItem.PurchaseReceipt != null &&
                            receiptItem.PurchaseReceipt.IsPosted &&
                            !receiptItem.PurchaseReceipt
                                .IsCancelled &&
                            !receiptItem.PurchaseReceipt
                                .IsDeleted)
                        .Sum(receiptItem =>
                            (decimal?)
                            receiptItem.ReceivedQuantity) ?? 0;

                decimal currentlyReceived =
                    currentItems
                        .Where(receiptItem =>
                            !receiptItem.IsDeleted &&
                            receiptItem.PurchaseOrderItemId ==
                                orderItem.Id)
                        .Sum(receiptItem =>
                            (decimal?)
                            receiptItem.ReceivedQuantity) ?? 0;

                decimal totalReceived =
                    previouslyReceived +
                    currentlyReceived;

                if (totalReceived > 0)
                {
                    hasAnyReceivedQuantity = true;
                }

                if (totalReceived <
                    orderItem.Quantity)
                {
                    allItemsFullyReceived = false;
                }
            }

            if (!hasAnyReceivedQuantity)
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.Approved;
            }
            else if (allItemsFullyReceived)
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.Received;
            }
            else
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.PartiallyReceived;
            }

            purchaseOrder.UpdatedAt =
                DateTime.Now;

            _unitOfWork.PurchaseOrders.Update(
                purchaseOrder);
        }

        public void Cancel(
         int id,
         string cancellationReason,
         string cancelledBy)
        {
            PurchaseReceipt receipt = GetById(id);

            if (receipt == null || receipt.IsDeleted)
            {
                throw new InvalidOperationException(
                    "مستند الاستلام غير موجود.");
            }

            if (receipt.IsCancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء مستند الاستلام مسبقًا.");
            }
            bool hasActivePurchaseReturns =
    _unitOfWork.PurchaseReturns
        .GetAll()
        .Any(purchaseReturn =>
            purchaseReturn.PurchaseReceiptId ==
                receipt.Id &&
            !purchaseReturn.IsDeleted &&
            purchaseReturn.Status !=
                PurchaseReturnStatus.Cancelled);

            if (hasActivePurchaseReturns)
            {
                throw new InvalidOperationException(
                    "لا يمكن إلغاء مستند الاستلام لأنه مرتبط " +
                    "بمرتجع مشتريات غير ملغي. " +
                    "يجب إلغاء المرتجع أولًا.");
            }

            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                throw new InvalidOperationException(
                    "يجب إدخال سبب الإلغاء.");
            }

            cancellationReason = cancellationReason.Trim();

            if (cancellationReason.Length > 500)
            {
                throw new InvalidOperationException(
                    "سبب الإلغاء لا يمكن أن يتجاوز 500 حرف.");
            }

            /*
             * قبل الإلغاء نتحقق من أن حذف كميات هذا
             * الاستلام لن يجعل الكمية المفوترة أكبر
             * من إجمالي الكمية المستلمة المتبقية.
             */
            if (receipt.IsPosted)
            {
                var receiptItems = receipt.Items
                    .Where(item => !item.IsDeleted)
                    .ToList();

                foreach (PurchaseReceiptItem receiptItem
                    in receiptItems)
                {
                    /*
                     * إجمالي الكمية المستلمة من المستندات
                     * الأخرى، مع استبعاد المستند الحالي.
                     */
                    decimal receivedFromOtherReceipts =
                        _unitOfWork.PurchaseReceipts
                            .GetAll()
                            .Where(otherReceipt =>
                                otherReceipt.Id != receipt.Id &&
                                otherReceipt.PurchaseOrderId ==
                                    receipt.PurchaseOrderId &&
                                otherReceipt.IsPosted &&
                                !otherReceipt.IsCancelled &&
                                !otherReceipt.IsDeleted)
                            .SelectMany(otherReceipt =>
                                otherReceipt.Items)
                            .Where(otherItem =>
                                !otherItem.IsDeleted &&
                                otherItem.PurchaseOrderItemId ==
                                    receiptItem.PurchaseOrderItemId)
                            .Select(otherItem =>
                                (decimal?)
                                    otherItem.ReceivedQuantity)
                            .Sum() ?? 0;

                    /*
                     * إجمالي الكمية الموجودة في فواتير
                     * مُرحّلة أو مدفوعة.
                     *
                     * الفاتورة المسودة والملغاة لا تمنع
                     * إلغاء الاستلام.
                     */
                    decimal invoicedQuantity =
                        _unitOfWork.PurchaseInvoices
                            .GetAll()
                            .Where(invoice =>
                                invoice.PurchaseOrderId ==
                                    receipt.PurchaseOrderId &&
                                !invoice.IsDeleted &&
                                invoice.Status !=
                                    PurchaseInvoiceStatus.Draft &&
                                invoice.Status !=
                                    PurchaseInvoiceStatus.Cancelled)
                            .SelectMany(invoice =>
                                invoice.Items)
                            .Where(invoiceItem =>
                                !invoiceItem.IsDeleted &&
                                invoiceItem.PurchaseOrderItemId ==
                                    receiptItem.PurchaseOrderItemId)
                            .Select(invoiceItem =>
                                (decimal?)invoiceItem.Quantity)
                            .Sum() ?? 0;

                    if (receivedFromOtherReceipts <
                        invoicedQuantity)
                    {
                        PurchaseOrderItem orderItem =
                            _unitOfWork.PurchaseOrderItems
                                .GetById(
                                    receiptItem
                                        .PurchaseOrderItemId);

                        string itemName =
                            orderItem != null &&
                            orderItem.Item != null
                                ? orderItem.Item.NameAr
                                : "الصنف";

                        throw new InvalidOperationException(
                            "لا يمكن إلغاء مستند الاستلام؛ " +
                            "لأن كمية الصنف \"" +
                            itemName +
                            "\" مرتبطة بفاتورة مشتريات " +
                            "مُرحّلة أو مدفوعة.");
                    }
                }
            }

            /*
             * بعد اجتياز فحص الفواتير:
             * إذا كان الاستلام مُرحّلًا نعكس حركات المخزون.
             */
            if (receipt.IsPosted)
            {
                var stockMovements =
                    _unitOfWork.StockMovements
                        .GetAll()
                        .Where(movement =>
                            movement.PurchaseReceiptId ==
                                receipt.Id &&
                            !movement.IsDeleted &&
                            !movement.IsCancelled &&
                            !movement.OriginalMovementId
                                .HasValue)
                        .ToList();

                if (!stockMovements.Any())
                {
                    throw new InvalidOperationException(
                        "لم يتم العثور على حركات المخزون " +
                        "الخاصة بمستند الاستلام.");
                }

                foreach (StockMovement movement
                    in stockMovements)
                {
                    _stockMovementService.CancelMovement(
                        movement.Id,
                        cancellationReason,
                        cancelledBy,
                        false);
                }
            }

            receipt.IsCancelled = true;
            receipt.CancelledAt = DateTime.Now;

            receipt.CancelledBy =
                string.IsNullOrWhiteSpace(cancelledBy)
                    ? null
                    : cancelledBy.Trim();

            receipt.CancellationReason =
                cancellationReason;

            receipt.IsActive = false;
            receipt.UpdatedAt = DateTime.Now;
            receipt.UpdatedBy = receipt.CancelledBy;

            _unitOfWork.PurchaseReceipts.Update(receipt);

            PurchaseOrder purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(
                    receipt.PurchaseOrderId);

            if (purchaseOrder != null &&
                !purchaseOrder.IsDeleted)
            {
                UpdateOrderStatusAfterCancellation(
                    purchaseOrder,
                    receipt.Id);
            }

            /*
             * حفظ إلغاء الاستلام والحركات العكسية
             * والأرصدة وحالة أمر الشراء معًا.
             */
            _unitOfWork.SaveChanges();
        }
        private void UpdateOrderStatusAfterCancellation(
    PurchaseOrder purchaseOrder,
    int cancelledReceiptId)
        {
            var orderItems = purchaseOrder.Items
                .Where(item => !item.IsDeleted)
                .ToList();

            bool hasAnyReceivedQuantity = false;
            bool allItemsFullyReceived = true;

            foreach (PurchaseOrderItem orderItem
                in orderItems)
            {
                decimal receivedQuantity =
                    _unitOfWork.PurchaseReceiptItems
                        .GetAll()
                        .Where(receiptItem =>
                            receiptItem.PurchaseOrderItemId ==
                                orderItem.Id &&
                            !receiptItem.IsDeleted &&
                            receiptItem.PurchaseReceipt != null &&
                            receiptItem.PurchaseReceipt.Id !=
                                cancelledReceiptId &&
                            receiptItem.PurchaseReceipt.IsPosted &&
                            !receiptItem.PurchaseReceipt
                                .IsCancelled &&
                            !receiptItem.PurchaseReceipt
                                .IsDeleted)
                        .Sum(receiptItem =>
                            (decimal?)
                            receiptItem.ReceivedQuantity) ?? 0;

                if (receivedQuantity > 0)
                {
                    hasAnyReceivedQuantity = true;
                }

                if (receivedQuantity <
                    orderItem.Quantity)
                {
                    allItemsFullyReceived = false;
                }
            }

            if (!hasAnyReceivedQuantity)
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.Approved;
            }
            else if (allItemsFullyReceived)
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.Received;
            }
            else
            {
                purchaseOrder.Status =
                    PurchaseOrderStatus.PartiallyReceived;
            }

            purchaseOrder.UpdatedAt = DateTime.Now;

            _unitOfWork.PurchaseOrders.Update(
                purchaseOrder);
        }
    }
}