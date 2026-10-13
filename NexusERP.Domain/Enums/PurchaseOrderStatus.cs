using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusERP.Domain.Enums
{
    public enum PurchaseOrderStatus
    {
        Draft = 1,

        Approved = 2,

        PartiallyReceived = 3,

        Received = 4,

        Cancelled = 5
    }

    /*
     Draft: مسودة، ويمكن تعديلها أو حذفها.
     Approved: معتمد وجاهز للاستلام.
     PartiallyReceived: استُلم جزء من الكميات.
     Received: استُلمت جميع الكميات.
     Cancelled: أمر ملغي ولا يمكن استلام أصناف منه.
     
     */
}
