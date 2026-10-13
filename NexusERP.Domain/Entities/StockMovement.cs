using System;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;
using NexusERP.Domain.Enums;

namespace NexusERP.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public StockMovementType MovementType { get; set; }

        public int WarehouseId { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

        public decimal Quantity { get; set; }

        public DateTime MovementDate { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public int? DestinationWarehouseId { get; set; }

        public virtual Warehouse DestinationWarehouse { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledAt { get; set; }

        [StringLength(256)]
        public string CancelledBy { get; set; }

        [StringLength(500)]
        public string CancellationReason { get; set; }

        public int? OriginalMovementId { get; set; }

        public virtual StockMovement OriginalMovement { get; set; }

        public int? PurchaseReceiptId { get; set; }

        public virtual PurchaseReceipt PurchaseReceipt
        {
            get;
            set;
        }
        public int? PurchaseReturnId
        {
            get;
            set;
        }

        public virtual PurchaseReturn PurchaseReturn
        {
            get;
            set;
        }
    }
}