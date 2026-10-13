using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusERP.Domain.Enums
{
    //مرتجع المشتريات
    namespace NexusERP.Domain.Enums
    {
        public enum PurchaseReturnStatus
        {
            Draft = 1,

            Posted = 2,

            Cancelled = 3
        }
    }
    /*
     - Draft: مسودة، لم تؤثر على المخزون.
- Posted: مُرحّل، وتم إنقاص الكميات من المخزون.
- Cancelled: ملغي؛ وإذا كان قد رُحّل سابقًا تُنشأ حركات عكسية تعيد الكميات للمخزون.
     */
}
