using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Application.Services
{
    public class PurchaseReturnService :
        IPurchaseReturnService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IStockMovementService
            _stockMovementService;

        public PurchaseReturnService(
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

        public IEnumerable<PurchaseReturn> GetAll()
        {
            return _unitOfWork.PurchaseReturns
                .GetAll()
                .Where(purchaseReturn =>
                    !purchaseReturn.IsDeleted)
                .OrderByDescending(purchaseReturn =>
                    purchaseReturn.ReturnDate)
                .ToList();
        }

        public PurchaseReturn GetById(int id)
        {
            return _unitOfWork.PurchaseReturns
                .GetAll()
                .FirstOrDefault(purchaseReturn =>
                    purchaseReturn.Id == id &&
                    !purchaseReturn.IsDeleted);
        }

        public decimal GetRemainingReturnableQuantity(
            int purchaseReceiptItemId)
        {
            PurchaseReceiptItem receiptItem =
                _unitOfWork.PurchaseReceiptItems
                    .GetById(purchaseReceiptItemId);

            if (receiptItem == null ||
                receiptItem.IsDeleted)
            {
                return 0;
            }

            decimal previouslyReturnedQuantity =
                _unitOfWork.PurchaseReturnItems
                    .GetAll()
                    .Where(returnItem =>
                        returnItem.PurchaseReceiptItemId ==
                            purchaseReceiptItemId &&
                        !returnItem.IsDeleted &&
                        returnItem.PurchaseReturn != null &&
                        !returnItem.PurchaseReturn.IsDeleted &&
                        returnItem.PurchaseReturn.Status ==
                            PurchaseReturnStatus.Posted)
                    .Select(returnItem =>
                        (decimal?)
                            returnItem.ReturnedQuantity)
                    .Sum() ?? 0;

            decimal remainingQuantity =
                receiptItem.ReceivedQuantity -
                previouslyReturnedQuantity;

            return remainingQuantity > 0
                ? remainingQuantity
                : 0;
        }

        public void Create(
            PurchaseReturn purchaseReturn)
        {
            if (purchaseReturn == null)
            {
                throw new ArgumentNullException(
                    "purchaseReturn");
            }

            if (string.IsNullOrWhiteSpace(
                purchaseReturn.ReturnNumber))
            {
                throw new InvalidOperationException(
                    "رقم مرتجع المشتريات مطلوب.");
            }

            purchaseReturn.ReturnNumber =
                purchaseReturn.ReturnNumber.Trim();

            if (purchaseReturn.ReturnNumber.Length > 30)
            {
                throw new InvalidOperationException(
                    "رقم المرتجع لا يمكن أن يتجاوز 30 حرفًا.");
            }

            bool duplicateReturnNumber =
                _unitOfWork.PurchaseReturns
                    .GetAll()
                    .Any(existingReturn =>
                        existingReturn.ReturnNumber ==
                            purchaseReturn.ReturnNumber &&
                        !existingReturn.IsDeleted);

            if (duplicateReturnNumber)
            {
                throw new InvalidOperationException(
                    "رقم مرتجع المشتريات مستخدم مسبقًا.");
            }

            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts.GetById(
                    purchaseReturn.PurchaseReceiptId);

            if (receipt == null ||
                receipt.IsDeleted)
            {
                throw new InvalidOperationException(
                    "مستند استلام المشتريات غير موجود.");
            }

            if (!receipt.IsPosted)
            {
                throw new InvalidOperationException(
                    "لا يمكن إنشاء مرتجع من مستند " +
                    "استلام غير مُرحّل.");
            }

            if (receipt.IsCancelled)
            {
                throw new InvalidOperationException(
                    "لا يمكن إنشاء مرتجع من مستند " +
                    "استلام ملغي.");
            }

            if (purchaseReturn.WarehouseId !=
                receipt.WarehouseId)
            {
                throw new InvalidOperationException(
                    "مستودع المرتجع يجب أن يكون نفس " +
                    "مستودع الاستلام.");
            }

            var returnItems =
                purchaseReturn.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .ToList();

            if (!returnItems.Any())
            {
                throw new InvalidOperationException(
                    "يجب إضافة صنف واحد على الأقل " +
                    "إلى مرتجع المشتريات.");
            }

            foreach (PurchaseReturnItem returnItem
                in returnItems)
            {
                PurchaseReceiptItem receiptItem =
                    _unitOfWork.PurchaseReceiptItems
                        .GetById(
                            returnItem
                                .PurchaseReceiptItemId);

                if (receiptItem == null ||
                    receiptItem.IsDeleted ||
                    receiptItem.PurchaseReceiptId !=
                        receipt.Id)
                {
                    throw new InvalidOperationException(
                        "أحد بنود المرتجع لا ينتمي " +
                        "إلى مستند الاستلام المحدد.");
                }

                if (returnItem.ReturnedQuantity <= 0)
                {
                    throw new InvalidOperationException(
                        "الكمية المرتجعة يجب أن تكون " +
                        "أكبر من صفر.");
                }

                decimal remainingQuantity =
                    GetRemainingReturnableQuantity(
                        receiptItem.Id);

                if (returnItem.ReturnedQuantity >
                    remainingQuantity)
                {
                    throw new InvalidOperationException(
                        "الكمية المرتجعة أكبر من " +
                        "الكمية المتاحة للإرجاع.");
                }

                if (returnItem.UnitPrice < 0)
                {
                    throw new InvalidOperationException(
                        "سعر الوحدة غير صحيح.");
                }

                returnItem.CreatedAt =
                    DateTime.Now;
                returnItem.CreatedBy = purchaseReturn.CreatedBy;

                returnItem.IsActive = true;
                returnItem.IsDeleted = false;
            }

            purchaseReturn.Status =
                PurchaseReturnStatus.Draft;

            purchaseReturn.PostedAt = null;
            purchaseReturn.PostedBy = null;
            purchaseReturn.CancelledAt = null;
            purchaseReturn.CancelledBy = null;
            purchaseReturn.CancellationReason = null;

            purchaseReturn.CreatedAt =
                DateTime.Now;

            purchaseReturn.IsActive = true;
            purchaseReturn.IsDeleted = false;

            _unitOfWork.PurchaseReturns.Add(
                purchaseReturn);

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

            if (postedBy.Length > 256)
            {
                throw new InvalidOperationException(
                    "اسم المستخدم لا يمكن أن يتجاوز 256 حرفًا.");
            }
            PurchaseReturn purchaseReturn =
                GetById(id);

            if (purchaseReturn == null)
            {
                throw new InvalidOperationException(
                    "مرتجع المشتريات غير موجود.");
            }

            if (purchaseReturn.Status ==
                PurchaseReturnStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "لا يمكن ترحيل مرتجع ملغي.");
            }

            if (purchaseReturn.Status ==
                PurchaseReturnStatus.Posted)
            {
                throw new InvalidOperationException(
                    "تم ترحيل مرتجع المشتريات مسبقًا.");
            }

            if (purchaseReturn.Status !=
                PurchaseReturnStatus.Draft)
            {
                throw new InvalidOperationException(
                    "يمكن ترحيل المرتجع المسودة فقط.");
            }

            PurchaseReceipt receipt =
                _unitOfWork.PurchaseReceipts.GetById(
                    purchaseReturn.PurchaseReceiptId);

            if (receipt == null ||
                receipt.IsDeleted ||
                !receipt.IsPosted ||
                receipt.IsCancelled)
            {
                throw new InvalidOperationException(
                    "مستند الاستلام المرتبط غير صالح.");
            }

            var returnItems =
                purchaseReturn.Items
                    .Where(item =>
                        !item.IsDeleted)
                    .ToList();

            if (!returnItems.Any())
            {
                throw new InvalidOperationException(
                    "مرتجع المشتريات لا يحتوي على أصناف.");
            }
            ValidateReturnAgainstInvoices(
    purchaseReturn,
    receipt,
    returnItems);

            foreach (PurchaseReturnItem returnItem
                in returnItems)
            {
                PurchaseReceiptItem receiptItem =
                    _unitOfWork.PurchaseReceiptItems
                        .GetById(
                            returnItem
                                .PurchaseReceiptItemId);

                if (receiptItem == null ||
                    receiptItem.IsDeleted ||
                    receiptItem.PurchaseReceiptId !=
                        receipt.Id)
                {
                    throw new InvalidOperationException(
                        "أحد بنود المرتجع غير صالح.");
                }

                if (returnItem.ReturnedQuantity <= 0)
                {
                    throw new InvalidOperationException(
                        "الكمية المرتجعة يجب أن تكون " +
                        "أكبر من صفر.");
                }

                decimal remainingQuantity =
                    GetRemainingReturnableQuantity(
                        receiptItem.Id);

                if (returnItem.ReturnedQuantity >
                    remainingQuantity)
                {
                    throw new InvalidOperationException(
                        "الكمية المرتجعة أكبر من " +
                        "الكمية المتاحة للإرجاع.");
                }

                PurchaseOrderItem orderItem =
                    _unitOfWork.PurchaseOrderItems
                        .GetById(
                            receiptItem
                                .PurchaseOrderItemId);

                if (orderItem == null ||
                    orderItem.IsDeleted)
                {
                    throw new InvalidOperationException(
                        "بند أمر الشراء المرتبط غير موجود.");
                }

                StockMovement movement =
                    new StockMovement
                    {
                        WarehouseId =
                            purchaseReturn.WarehouseId,

                        ItemId =
                            orderItem.ItemId,

                        Quantity =
                            returnItem.ReturnedQuantity,

                        /*
                         * المرتجع يخرج الأصناف من المستودع.
                         */
                        MovementType =
                            StockMovementType.Issue,

                        MovementDate =
                            purchaseReturn.ReturnDate,

                        ReferenceNumber =
                            "PRET-" +
                            purchaseReturn.ReturnNumber,

                        PurchaseReturnId =
                            purchaseReturn.Id,

                        Notes =
                            "مرتجع مشتريات من مستند الاستلام " +
                            receipt.ReceiptNumber,

                        CreatedAt =
                            DateTime.Now,

                        CreatedBy = postedBy,

                        IsActive = true,
                        IsDeleted = false,
                        IsCancelled = false
                    };

                /*
                 * false لأن الحفظ النهائي سيتم مرة واحدة.
                 * CreateMovement يجب أن يتحقق من توفر المخزون.
                 */
                _stockMovementService.CreateMovement(
                    movement,
                    false);
            }

            purchaseReturn.Status =
                PurchaseReturnStatus.Posted;

            purchaseReturn.PostedAt =
                DateTime.Now;

            purchaseReturn.PostedBy = postedBy;

            purchaseReturn.UpdatedAt =
                DateTime.Now;

            purchaseReturn.UpdatedBy =
                purchaseReturn.PostedBy;

            _unitOfWork.PurchaseReturns.Update(
                purchaseReturn);

            /*
             * حفظ المرتجع والحركات والأرصدة معًا.
             */
            _unitOfWork.SaveChanges();
        }

        public void Cancel(
            int id,
            string cancellationReason,
            string cancelledBy)
        {
            PurchaseReturn purchaseReturn =
                GetById(id);

            if (purchaseReturn == null)
            {
                throw new InvalidOperationException(
                    "مرتجع المشتريات غير موجود.");
            }

            if (purchaseReturn.Status ==
                PurchaseReturnStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء مرتجع المشتريات مسبقًا.");
            }

            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                throw new InvalidOperationException(
                    "سبب إلغاء المرتجع مطلوب.");
            }

            cancellationReason =
                cancellationReason.Trim();

            if (cancellationReason.Length > 500)
            {
                throw new InvalidOperationException(
                    "سبب الإلغاء لا يمكن أن يتجاوز " +
                    "500 حرف.");
            }

            /*
             * إذا كان المرتجع مُرحّلًا،
             * نعكس حركات الصرف لإعادة المخزون.
             */
            if (purchaseReturn.Status ==
                PurchaseReturnStatus.Posted)
            {
                var stockMovements =
                    _unitOfWork.StockMovements
                        .GetAll()
                        .Where(movement =>
                            movement.PurchaseReturnId ==
                                purchaseReturn.Id &&
                            !movement.IsDeleted &&
                            !movement.IsCancelled &&
                            !movement.OriginalMovementId
                                .HasValue)
                        .ToList();

                if (!stockMovements.Any())
                {
                    throw new InvalidOperationException(
                        "لم يتم العثور على حركات المخزون " +
                        "الخاصة بمرتجع المشتريات.");
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

            purchaseReturn.Status =
                PurchaseReturnStatus.Cancelled;

            purchaseReturn.CancelledAt =
                DateTime.Now;

            purchaseReturn.CancelledBy =
                string.IsNullOrWhiteSpace(cancelledBy)
                    ? "System"
                    : cancelledBy.Trim();

            purchaseReturn.CancellationReason =
                cancellationReason;

            purchaseReturn.IsActive = false;

            purchaseReturn.UpdatedAt =
                DateTime.Now;

            purchaseReturn.UpdatedBy =
                purchaseReturn.CancelledBy;

            _unitOfWork.PurchaseReturns.Update(
                purchaseReturn);

            /*
             * حفظ إلغاء المرتجع والحركات العكسية
             * وتحديث الرصيد معًا.
             */
            _unitOfWork.SaveChanges();
        }

        private void ValidateReturnAgainstInvoices(
    PurchaseReturn purchaseReturn,
    PurchaseReceipt receipt,
    IEnumerable<PurchaseReturnItem> returnItems)
        {
            var quantitiesByOrderItem =
                new Dictionary<int, decimal>();

            foreach (PurchaseReturnItem returnItem in returnItems)
            {
                PurchaseReceiptItem receiptItem =
                    _unitOfWork.PurchaseReceiptItems.GetById(
                        returnItem.PurchaseReceiptItemId);

                if (receiptItem == null ||
                    receiptItem.IsDeleted ||
                    receiptItem.PurchaseReceiptId != receipt.Id)
                {
                    throw new InvalidOperationException(
                        "أحد بنود المرتجع غير صالح.");
                }

                if (returnItem.ReturnedQuantity <= 0)
                {
                    throw new InvalidOperationException(
                        "الكمية المرتجعة يجب أن تكون أكبر من صفر.");
                }

                int orderItemId = receiptItem.PurchaseOrderItemId;

                if (!quantitiesByOrderItem.ContainsKey(orderItemId))
                {
                    quantitiesByOrderItem[orderItemId] = 0;
                }

                quantitiesByOrderItem[orderItemId] +=
                    returnItem.ReturnedQuantity;
            }

            foreach (var entry in quantitiesByOrderItem)
            {
                int orderItemId = entry.Key;
                decimal currentReturnQuantity = entry.Value;

                PurchaseOrderItem orderItem =
                    _unitOfWork.PurchaseOrderItems.GetById(
                        orderItemId);

                if (orderItem == null ||
                    orderItem.IsDeleted ||
                    orderItem.PurchaseOrderId != receipt.PurchaseOrderId)
                {
                    throw new InvalidOperationException(
                        "بند أمر الشراء المرتبط غير صالح.");
                }

                decimal receivedQuantity =
                    _unitOfWork.PurchaseReceiptItems
                        .GetAll()
                        .Where(item =>
                            !item.IsDeleted &&
                            item.PurchaseOrderItemId == orderItemId &&
                            item.PurchaseReceipt != null &&
                            !item.PurchaseReceipt.IsDeleted &&
                            item.PurchaseReceipt.IsPosted &&
                            !item.PurchaseReceipt.IsCancelled)
                        .Sum(item =>
                            (decimal?)item.ReceivedQuantity) ?? 0;

                decimal previouslyReturnedQuantity =
                    _unitOfWork.PurchaseReturnItems
                        .GetAll()
                        .Where(item =>
                            !item.IsDeleted &&
                            item.PurchaseReceiptItem != null &&
                            item.PurchaseReceiptItem.PurchaseOrderItemId ==
                                orderItemId &&
                            item.PurchaseReturn != null &&
                            item.PurchaseReturn.Id != purchaseReturn.Id &&
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
                            item.PurchaseOrderItemId == orderItemId &&
                            item.PurchaseInvoice != null &&
                            !item.PurchaseInvoice.IsDeleted &&
                            (item.PurchaseInvoice.Status ==
                                PurchaseInvoiceStatus.Posted ||
                             item.PurchaseInvoice.Status ==
                                PurchaseInvoiceStatus.Paid))
                        .Sum(item =>
                            (decimal?)item.Quantity) ?? 0;

                decimal netReceivedAfterReturn =
                    receivedQuantity -
                    previouslyReturnedQuantity -
                    currentReturnQuantity;

                if (netReceivedAfterReturn < invoicedQuantity)
                {
                    string itemName = orderItem.Item != null
                        ? orderItem.Item.NameAr
                        : orderItemId.ToString();

                    throw new InvalidOperationException(
                        "لا يمكن ترحيل المرتجع للصنف \"" +
                        itemName +
                        "\"؛ لأن صافي الكمية المستلمة بعد الإرجاع (" +
                        netReceivedAfterReturn.ToString("0.##") +
                        ") سيكون أقل من الكمية المفوترة (" +
                        invoicedQuantity.ToString("0.##") +
                        "). يجب معالجة الفاتورة أولًا.");
                }
            }
        }

        /*
        PurchaseReturnService مسؤولة عن دورة حياة مرتجع المشتريات من إنشائه حتى ترحيله أو إلغائه.
        GetAll()	تعرض جميع المرتجعات غير المحذوفة	
        GetById(int id)	تجلب مرتجعًا محددًا بواسطة رقمه الداخلي	
        GetRemainingReturnableQuantity(...)	تحسب الكمية المتبقية المسموح بإرجاعها	
        Create(...)	تحفظ مستند المرتجع كمسودة
        Post(...)	ترحّل المرتجع وتنشئ حركة صرف	إنقاص المخزون
        Cancel(...)	تلغي المرتجع وتعكس حركة الصرف إذا كان مُرحّلًا	إعادة المخزون
         */
    }
}