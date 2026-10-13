using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;
using NexusERP.Domain.Enums;

namespace NexusERP.Domain.Entities
{
    public class PurchaseInvoice : BaseEntity
    {
        [Required]
        [StringLength(30)]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public int PurchaseOrderId { get; set; }

        public decimal DiscountAmount
        {
            get;
            set;
        }

        public virtual PurchaseOrder PurchaseOrder
        {
            get;
            set;
        }

        public PurchaseInvoiceStatus Status
        {
            get;
            set;
        }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime? PostedAt { get; set; }

        [StringLength(100)]
        public string PostedBy { get; set; }

        public DateTime? CancelledAt { get; set; }

        [StringLength(100)]
        public string CancelledBy { get; set; }

        [StringLength(500)]
        public string CancellationReason { get; set; }

        public virtual ICollection<PurchaseInvoiceItem>
            Items
        { get; set; } =
                new HashSet<PurchaseInvoiceItem>();

        public virtual ICollection<PurchasePayment> Payments
        {
            get;
            set;
        }

        public PurchaseInvoice()
        {
            Payments =
                new HashSet<PurchasePayment>();
        }
    }
}