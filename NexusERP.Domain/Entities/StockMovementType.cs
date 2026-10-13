using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusERP.Domain.Enums
{
    public enum StockMovementType
    {
        OpeningBalance = 1, // رصيد افتتاحي
        Receipt = 2,        // استلام
        Issue = 3,          // صرف
        Transfer = 4,       // تحويل بين المستودعات
        StockAdjustment = 5 // تسوية جرد
    }
}
/*

OpeningBalance	إدخال كمية الصنف الموجودة عند بدء استخدام النظام
Receipt	زيادة المخزون بسبب شراء أو استلام
Issue	إنقاص المخزون بسبب الصرف
Transfer	نقل الكمية من مستودع إلى مستودع آخر
StockAdjustment	تصحيح الكمية بعد الجرد الفعلي 
 */
