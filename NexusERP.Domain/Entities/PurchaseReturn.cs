using NexusERP.Domain.Common;
using NexusERP.Domain.Enums.NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusERP.Domain.Entities
{
    //مرتجع المشتريات
        public class PurchaseReturn : BaseEntity
        {
            public string ReturnNumber { get; set; }

            public DateTime ReturnDate { get; set; }

            /*
             * المرتجع مرتبط بمستند استلام محدد،
             * حتى لا نعيد كمية لم يتم استلامها.
             */
            public int PurchaseReceiptId { get; set; }

            public virtual PurchaseReceipt PurchaseReceipt
            {
                get;
                set;
            }

            /*
             * المستودع الذي ستخرج منه الأصناف.
             */
            public int WarehouseId { get; set; }

            public virtual Warehouse Warehouse
            {
                get;
                set;
            }

            public PurchaseReturnStatus Status
            {
                get;
                set;
            }

            public DateTime? PostedAt { get; set; }

            public string PostedBy { get; set; }

            public DateTime? CancelledAt { get; set; }

            public string CancelledBy { get; set; }

            public string CancellationReason { get; set; }

            public string Notes { get; set; }

            public virtual ICollection<PurchaseReturnItem> Items
            {
                get;
                set;
            } = new HashSet<PurchaseReturnItem>();
        }
 
}
