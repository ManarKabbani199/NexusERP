using System;
using NexusERP.Domain.Common;
using NexusERP.Domain.Enums;

namespace NexusERP.Domain.Entities
{
    public class PurchasePayment : BaseEntity
    {
        public string PaymentNumber { get; set; }

        public DateTime PaymentDate { get; set; }

        public int PurchaseInvoiceId { get; set; }

        public virtual PurchaseInvoice PurchaseInvoice
        {
            get;
            set;
        }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string ReferenceNumber { get; set; }

        public string Notes { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledAt { get; set; }

        public string CancelledBy { get; set; }

        public string CancellationReason { get; set; }
    }
}
/*
 
 دفع الفاتورة كاملة أو جزئيًا.
تسجيل أكثر من دفعة للفاتورة نفسها.
تحديد طريقة الدفع.
إلغاء الدفعة مع الاحتفاظ بسجلها.
اعتبار الفاتورة مدفوعة فقط عندما يصل مجموع الدفعات إلى إجمالي الفاتورة.
 
 */