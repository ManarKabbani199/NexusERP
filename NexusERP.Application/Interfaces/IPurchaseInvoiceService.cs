using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPurchaseInvoiceService
    {
        IEnumerable<PurchaseInvoice> GetAll();

        PurchaseInvoice GetById(int id);

        void CreateDraft(
            PurchaseInvoice purchaseInvoice);

        void Post(
            int id,
            string postedBy);

        void MarkAsPaid(
            int id);

        void Cancel(
            int id,
            string cancellationReason,
            string cancelledBy);

        bool InvoiceNumberExists(
            string invoiceNumber,
            int? excludedId = null);

        decimal GetReceivedQuantity(
    int purchaseOrderItemId);

        decimal GetInvoicedQuantity(
            int purchaseOrderItemId);

        decimal GetAvailableQuantity(
            int purchaseOrderItemId);
    }
}


/*
 
GetAll: جلب جميع الفواتير.
GetById: جلب فاتورة محددة.
CreateDraft: إنشاء فاتورة بحالة مسودة.
Post: ترحيل واعتماد الفاتورة ماليًا، دون تغيير المخزون.
MarkAsPaid: تحويل حالة الفاتورة إلى مدفوعة.
Cancel: إلغاء الفاتورة وحفظ السبب والمستخدم.
InvoiceNumberExists: منع تكرار رقم الفاتورة.

 */