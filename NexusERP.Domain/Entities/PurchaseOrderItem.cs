using System.ComponentModel.DataAnnotations.Schema;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal LineTotal
        {
            get { return Quantity * UnitPrice; }
        }
    }
}