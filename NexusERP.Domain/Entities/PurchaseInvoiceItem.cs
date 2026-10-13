using System.ComponentModel.DataAnnotations.Schema;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class PurchaseInvoiceItem : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }

        public virtual PurchaseInvoice PurchaseInvoice
        {
            get;
            set;
        }

        public int PurchaseOrderItemId { get; set; }

        public virtual PurchaseOrderItem PurchaseOrderItem
        {
            get;
            set;
        }

        [Column(TypeName = "decimal")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TaxRate { get; set; }

        [NotMapped]
        public decimal SubTotal
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        [NotMapped]
        public decimal TaxAmount
        {
            get
            {
                return SubTotal *
                       TaxRate / 100;
            }
        }

        [NotMapped]
        public decimal LineTotal
        {
            get
            {
                return SubTotal +
                       TaxAmount;
            }
        }
    }
}