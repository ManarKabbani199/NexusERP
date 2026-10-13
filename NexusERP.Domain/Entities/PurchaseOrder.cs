using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;
using NexusERP.Domain.Enums;

namespace NexusERP.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public PurchaseOrder()
        {
            Status = PurchaseOrderStatus.Draft;
            Items = new HashSet<PurchaseOrderItem>();
        }

        [Required]
        [StringLength(30)]
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual ICollection<PurchaseOrderItem> Items
        {
            get;
            set;
        }

        public DateTime? CancelledAt { get; set; }

        [StringLength(100)]
        public string CancelledBy { get; set; }

        [StringLength(500)]
        public string CancellationReason { get; set; }

        [StringLength(256)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}