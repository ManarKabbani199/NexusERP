using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class PurchaseReceipt : BaseEntity
    {
        public PurchaseReceipt()
        {
            Items =
                new HashSet<PurchaseReceiptItem>();

            IsPosted = false;
            IsCancelled = false;
        }

        [Required]
        [StringLength(30)]
        public string ReceiptNumber { get; set; }

        public DateTime ReceiptDate { get; set; }

        public int PurchaseOrderId { get; set; }

        public virtual PurchaseOrder PurchaseOrder
        {
            get;
            set;
        }

        public int WarehouseId { get; set; }

        public virtual Warehouse Warehouse
        {
            get;
            set;
        }

        public bool IsPosted { get; set; }

        public DateTime? PostedAt { get; set; }

        [StringLength(256)]
        public string PostedBy { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledAt { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual ICollection<PurchaseReceiptItem> Items
        {
            get;
            set;
        }

        [StringLength(256)]
        public string CancelledBy { get; set; }

        [StringLength(500)]
        public string CancellationReason { get; set; }

    }
}