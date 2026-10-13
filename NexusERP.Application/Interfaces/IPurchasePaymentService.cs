using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPurchasePaymentService
    {
        IEnumerable<PurchasePayment> GetAll();

        PurchasePayment GetById(int id);

        IEnumerable<PurchasePayment>
            GetByInvoiceId(int purchaseInvoiceId);

        decimal GetInvoiceTotal(
            int purchaseInvoiceId);

        decimal GetPaidAmount(
            int purchaseInvoiceId);

        decimal GetRemainingAmount(
            int purchaseInvoiceId);

        int Create(
            PurchasePayment payment);

        void Cancel(
            int paymentId,
            string cancellationReason,
            string cancelledBy);
    }
}

/*
 
GetAll: جميع الدفعات.
GetById: تفاصيل دفعة واحدة.
GetByInvoiceId: دفعات فاتورة محددة.
GetInvoiceTotal: إجمالي قيمة الفاتورة.
GetPaidAmount: مجموع الدفعات غير الملغاة.
GetRemainingAmount: المبلغ المتبقي.
Create: تسجيل دفعة جديدة.
Cancel: إلغاء دفعة وإعادة احتساب حالة الفاتورة. 

 */