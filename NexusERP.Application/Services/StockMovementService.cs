using System;
using System.Collections.Generic;
using System.Linq;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;

namespace NexusERP.Application.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockMovementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<StockMovement> GetAll()
        {
            return _unitOfWork.StockMovements
                .GetAll()
                .Where(movement => !movement.IsDeleted)
                .OrderByDescending(movement => movement.MovementDate)
                .ToList();
        }

        public StockMovement GetById(int id)
        {
            return _unitOfWork.StockMovements.GetById(id);
        }
        
        //من اجل منع أضافه رصيد افتتاحي اكثر من مرة
        private bool HasOpeningBalance(
    int warehouseId,
    int itemId)
        {
            return _unitOfWork.StockMovements
                .GetAll()
                .Any(movement =>
                    movement.WarehouseId == warehouseId &&
                    movement.ItemId == itemId &&
                    movement.MovementType ==
                        StockMovementType.OpeningBalance &&
                    !movement.IsDeleted);
        }

        public void CreateMovement(StockMovement movement, bool saveChanges = true)
        {
            ValidateMovement(movement);

            if (movement.MovementType ==
               StockMovementType.OpeningBalance &&
               HasOpeningBalance(  movement.WarehouseId,  movement.ItemId))
            {
                throw new InvalidOperationException(
                    "تم تسجيل رصيد افتتاحي مسبقًا لهذا الصنف " +
                    "في هذا المستودع. استخدمي تسوية جرد لتصحيح الرصيد.");
            }

            WarehouseStock sourceStock = GetStock(
                movement.WarehouseId,
                movement.ItemId);

            switch (movement.MovementType)
            {
                case StockMovementType.OpeningBalance:
                    sourceStock = GetOrCreateStock(
                        movement.WarehouseId,
                        movement.ItemId);

                    sourceStock.Quantity = movement.Quantity;

                    _unitOfWork.WarehouseStocks.Update(sourceStock);
                    break;

                case StockMovementType.Receipt:
                    sourceStock = GetOrCreateStock(
                        movement.WarehouseId,
                        movement.ItemId);

                    sourceStock.Quantity += movement.Quantity;

                    _unitOfWork.WarehouseStocks.Update(sourceStock);
                    break;

                case StockMovementType.Issue:
                    EnsureSufficientQuantity(
                        sourceStock,
                        movement.Quantity);

                    sourceStock.Quantity -= movement.Quantity;
                    _unitOfWork.WarehouseStocks.Update(sourceStock);
                    break;

                case StockMovementType.Transfer:
                    ProcessTransfer(movement, sourceStock);
                    break;

                case StockMovementType.StockAdjustment:
                    sourceStock = GetOrCreateStock(
                        movement.WarehouseId,
                        movement.ItemId);

                    // تمثل الكمية هنا الكمية الفعلية الناتجة عن الجرد
                    sourceStock.Quantity = movement.Quantity;
                    _unitOfWork.WarehouseStocks.Update(sourceStock);
                    break;

                default:
                    throw new InvalidOperationException(
                        "نوع حركة المخزون غير صالح.");
            }

            if (movement.MovementDate == default(DateTime))
            {
                movement.MovementDate = DateTime.Now;
            }

            movement.CreatedAt = DateTime.Now;
            movement.IsActive = true;
            movement.IsDeleted = false;

            _unitOfWork.StockMovements.Add(movement);

            if (saveChanges)
            {
                _unitOfWork.SaveChanges();
            }
        }

        private void ProcessTransfer(
            StockMovement movement,
            WarehouseStock sourceStock)
        {
            EnsureSufficientQuantity(
                sourceStock,
                movement.Quantity);

            WarehouseStock destinationStock = GetOrCreateStock(
                movement.DestinationWarehouseId.Value,
                movement.ItemId);

            sourceStock.Quantity -= movement.Quantity;
            destinationStock.Quantity += movement.Quantity;

            _unitOfWork.WarehouseStocks.Update(sourceStock);
            _unitOfWork.WarehouseStocks.Update(destinationStock);
        }

        private WarehouseStock GetStock(int warehouseId, int itemId)
        {
            return _unitOfWork.WarehouseStocks
                .GetAll()
                .FirstOrDefault(stock =>
                    stock.WarehouseId == warehouseId &&
                    stock.ItemId == itemId &&
                    !stock.IsDeleted);
        }

        private WarehouseStock GetOrCreateStock(
            int warehouseId,
            int itemId)
        {
            WarehouseStock stock = GetStock(warehouseId, itemId);

            if (stock != null)
            {
                return stock;
            }

            stock = new WarehouseStock
            {
                WarehouseId = warehouseId,
                ItemId = itemId,
                Quantity = 0,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.WarehouseStocks.Add(stock);

            return stock;
        }

        private void EnsureSufficientQuantity(
            WarehouseStock stock,
            decimal requiredQuantity)
        {
            if (stock == null)
            {
                throw new InvalidOperationException(
                    "لا يوجد رصيد لهذا الصنف في المستودع.");
            }

            if (stock.Quantity < requiredQuantity)
            {
                throw new InvalidOperationException(
                    "الكمية المتوفرة في المستودع غير كافية.");
            }
        }

        private void ValidateMovement(StockMovement movement)
        {
            if (movement == null)
            {
                throw new ArgumentNullException(nameof(movement));
            }

            if (movement.WarehouseId <= 0)
            {
                throw new InvalidOperationException(
                    "يجب اختيار المستودع.");
            }

            if (movement.ItemId <= 0)
            {
                throw new InvalidOperationException(
                    "يجب اختيار الصنف.");
            }

            if (movement.Quantity <= 0)
            {
                throw new InvalidOperationException(
                    "يجب أن تكون الكمية أكبر من صفر.");
            }

            if (movement.MovementType ==
                    StockMovementType.Transfer)
            {
                if (!movement.DestinationWarehouseId.HasValue)
                {
                    throw new InvalidOperationException(
                        "يجب اختيار مستودع الوجهة.");
                }

                if (movement.WarehouseId ==
                    movement.DestinationWarehouseId.Value)
                {
                    throw new InvalidOperationException(
                        "يجب أن يختلف مستودع الوجهة عن المستودع المصدر.");
                }
            }
            else
            {
                movement.DestinationWarehouseId = null;
            }
        }


        //ألغاء الحركة

        public void CancelMovement(
       int movementId,
       string cancellationReason,
       string cancelledBy,
       bool saveChanges = true)
        {
            StockMovement originalMovement =
                _unitOfWork.StockMovements
                    .GetById(movementId);

            if (originalMovement == null ||
                originalMovement.IsDeleted)
            {
                throw new InvalidOperationException(
                    "حركة المخزون غير موجودة.");
            }

            if (originalMovement.IsCancelled)
            {
                throw new InvalidOperationException(
                    "تم إلغاء هذه الحركة مسبقًا.");
            }

            if (originalMovement.OriginalMovementId.HasValue)
            {
                throw new InvalidOperationException(
                    "لا يمكن إلغاء حركة عكسية.");
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

            /*
             * عند إلغاء حركة استلام:
             * الحركة العكسية ستُنقص المخزون.
             * لذلك يجب التأكد من توفر الكمية قبل الإلغاء.
             */
            if (originalMovement.MovementType ==
                StockMovementType.Receipt)
            {
                WarehouseStock warehouseStock =
                    _unitOfWork.WarehouseStocks
                        .GetAll()
                        .FirstOrDefault(stock =>
                            !stock.IsDeleted &&
                            stock.WarehouseId ==
                                originalMovement.WarehouseId &&
                            stock.ItemId ==
                                originalMovement.ItemId);

                if (warehouseStock == null)
                {
                    throw new InvalidOperationException(
                        "لا يمكن إلغاء الاستلام لأن رصيد " +
                        "الصنف غير موجود في المستودع.");
                }

                if (warehouseStock.Quantity <
                    originalMovement.Quantity)
                {
                    throw new InvalidOperationException(
                        "لا يمكن إلغاء الاستلام لأن جزءًا من " +
                        "الكمية تم صرفه، والرصيد الحالي غير كافٍ. " +
                        "الرصيد الحالي: " +
                        warehouseStock.Quantity.ToString("N2") +
                        "، والكمية المطلوب عكسها: " +
                        originalMovement.Quantity.ToString("N2") +
                        ".");
                }
            }

            StockMovement reversalMovement =
                CreateReversalMovement(
                    originalMovement,
                    cancellationReason,
                    cancelledBy);

            originalMovement.IsCancelled = true;
            originalMovement.CancelledAt =
                DateTime.Now;

            originalMovement.CancelledBy =
                string.IsNullOrWhiteSpace(cancelledBy)
                    ? null
                    : cancelledBy.Trim();

            originalMovement.CancellationReason =
                cancellationReason;

            originalMovement.UpdatedAt =
                DateTime.Now;

            originalMovement.UpdatedBy =
                originalMovement.CancelledBy;

            _unitOfWork.StockMovements.Update(
                originalMovement);

            _unitOfWork.StockMovements.Add(
                reversalMovement);

            // حفظ تعديل الرصيد والحركة الأصلية
            // والحركة العكسية معًا.
            if (saveChanges)
            {
                _unitOfWork.SaveChanges();
            }
        }

        private StockMovement CreateReversalMovement(
    StockMovement originalMovement,
    string cancellationReason,
    string cancelledBy)
        {
            StockMovement reversalMovement =
                new StockMovement
                {
                    WarehouseId =
                        originalMovement.WarehouseId,

                    DestinationWarehouseId =
    originalMovement.DestinationWarehouseId,

                    ItemId =
                        originalMovement.ItemId,

                    Quantity =
                        originalMovement.Quantity,

                    MovementDate = DateTime.Now,

                    ReferenceNumber =
                        "REV-" + originalMovement.Id,

                    Notes =
                        "حركة عكسية لإلغاء الحركة رقم " +
                        originalMovement.Id +
                        ". السبب: " +
                        cancellationReason,

                    OriginalMovementId =
                        originalMovement.Id,

                    PurchaseReceiptId =
    originalMovement.PurchaseReceiptId,

                    PurchaseReturnId =
    originalMovement.PurchaseReturnId,

                    CreatedAt = DateTime.Now,

                    CreatedBy =
                        string.IsNullOrWhiteSpace(cancelledBy)
                            ? null
                            : cancelledBy.Trim(),

                    IsActive = true,
                    IsDeleted = false,
                    IsCancelled = false
                };

            switch (originalMovement.MovementType)
            {
                case StockMovementType.Receipt:
                    ReverseReceipt(
                        originalMovement,
                        reversalMovement);
                    break;

                case StockMovementType.Issue:
                    ReverseIssue(
                        originalMovement,
                        reversalMovement);
                    break;

                case StockMovementType.Transfer:
                    ReverseTransfer(
                        originalMovement,
                        reversalMovement);
                    break;

                case StockMovementType.OpeningBalance:
                case StockMovementType.StockAdjustment:
                    throw new InvalidOperationException(
                        "لا يمكن إلغاء الرصيد الافتتاحي " +
                        "أو تسوية الجرد. استخدمي حركة " +
                        "تسوية جرد جديدة.");

                default:
                    throw new InvalidOperationException(
                        "نوع حركة المخزون غير صالح للإلغاء.");
            }

            return reversalMovement;
        }

        private void ReverseReceipt(
            StockMovement originalMovement,
            StockMovement reversalMovement)
        {
            WarehouseStock stock = GetStock(
                originalMovement.WarehouseId,
                originalMovement.ItemId);

            EnsureSufficientQuantity(
                stock,
                originalMovement.Quantity);

            stock.Quantity -= originalMovement.Quantity;

            _unitOfWork.WarehouseStocks.Update(stock);

            // عكس الاستلام هو الصرف
            reversalMovement.MovementType =
                StockMovementType.Issue;

            reversalMovement.DestinationWarehouseId = null;
        }

        private void ReverseIssue(
            StockMovement originalMovement,
            StockMovement reversalMovement)
        {
            WarehouseStock stock = GetOrCreateStock(
                originalMovement.WarehouseId,
                originalMovement.ItemId);

            stock.Quantity += originalMovement.Quantity;

            _unitOfWork.WarehouseStocks.Update(stock);

            // عكس الصرف هو الاستلام
            reversalMovement.MovementType =
                StockMovementType.Receipt;

            reversalMovement.DestinationWarehouseId = null;
        }

        private void ReverseTransfer(
            StockMovement originalMovement,
            StockMovement reversalMovement)
        {
            if (!originalMovement
                    .DestinationWarehouseId.HasValue)
            {
                throw new InvalidOperationException(
                    "حركة التحويل الأصلية لا تحتوي " +
                    "على مستودع وجهة.");
            }

            WarehouseStock previousDestinationStock =
                GetStock(
                    originalMovement
                        .DestinationWarehouseId.Value,
                    originalMovement.ItemId);

            EnsureSufficientQuantity(
                previousDestinationStock,
                originalMovement.Quantity);

            WarehouseStock previousSourceStock =
                GetOrCreateStock(
                    originalMovement.WarehouseId,
                    originalMovement.ItemId);

            previousDestinationStock.Quantity -=
                originalMovement.Quantity;

            previousSourceStock.Quantity +=
                originalMovement.Quantity;

            _unitOfWork.WarehouseStocks.Update(
                previousDestinationStock);

            _unitOfWork.WarehouseStocks.Update(
                previousSourceStock);

            reversalMovement.MovementType =
                StockMovementType.Transfer;

            // الوجهة القديمة تصبح المصدر الجديد
            reversalMovement.WarehouseId =
                originalMovement
                    .DestinationWarehouseId.Value;

            // المصدر القديم يصبح الوجهة الجديدة
            reversalMovement.DestinationWarehouseId =
                originalMovement.WarehouseId;
        }
    }
}
