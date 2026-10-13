using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    //عناصر مرتجع المشتريات
    public class PurchaseReturnItem : BaseEntity
    {
        public int PurchaseReturnId { get; set; }

        public virtual PurchaseReturn PurchaseReturn
        {
            get;
            set;
        }

        /*
         * الربط ببند الاستلام يمنع إرجاع
         * كمية أكبر من الكمية المستلمة.
         */
        public int PurchaseReceiptItemId { get; set; }

        public virtual PurchaseReceiptItem PurchaseReceiptItem
        {
            get;
            set;
        }

        public decimal ReturnedQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}