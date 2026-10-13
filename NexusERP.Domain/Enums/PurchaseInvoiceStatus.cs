namespace NexusERP.Domain.Enums
{
    public enum PurchaseInvoiceStatus
    {
        Draft = 1,
        Posted = 2,
        Paid = 3,
        Cancelled = 4
    }
}
/*
 
1	Draft	مسودة: تم إنشاء الفاتورة، وما زال يمكن مراجعتها قبل الترحيل
2	Posted	مُرحّلة: تم اعتماد الفاتورة وأصبحت التزامًا ماليًا على الشركة للمورد
3	Paid	مدفوعة: تم دفع قيمة الفاتورة للمورد
4	Cancelled	ملغاة: تم إلغاء الفاتورة مع حفظ سبب الإلغاء
 
 */
