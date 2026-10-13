using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPurchaseReturnService
    {
        IEnumerable<PurchaseReturn> GetAll();

        PurchaseReturn GetById(int id);

        void Create(PurchaseReturn purchaseReturn);

        void Post(
            int id,
            string postedBy);

        void Cancel(
            int id,
            string cancellationReason,
            string cancelledBy);

        decimal GetRemainingReturnableQuantity(
            int purchaseReceiptItemId);
    }

    /*
     GetAll: عرض جميع مستندات المرتجع.
     GetById: جلب تفاصيل مرتجع معين.
     Create: حفظ المرتجع كمسودة، دون تغيير المخزون.
     Post: ترحيل المرتجع وإنقاص المخزون.
     Cancel: إلغاء المرتجع، وإذا كان مُرحّلًا تُعاد الكميات للمخزون.
     GetRemainingReturnableQuantity: حساب الكمية التي لا يزال مسموحًا بإرجاعها من بند الاستلام.
     */
}