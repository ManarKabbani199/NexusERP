using System.ComponentModel.DataAnnotations.Schema;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class PurchaseReceiptItem : BaseEntity
    {
        public int PurchaseReceiptId { get; set; }

        public virtual PurchaseReceipt PurchaseReceipt
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
        public decimal ReceivedQuantity { get; set; }
    }
}