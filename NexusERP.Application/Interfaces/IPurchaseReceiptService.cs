using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPurchaseReceiptService
    {
        IEnumerable<PurchaseReceipt> GetAll();

        PurchaseReceipt GetById(int id);

        void CreateDraft(
            PurchaseReceipt purchaseReceipt);

        void Post(int id, string postedBy);

        decimal GetReceivedQuantity(
            int purchaseOrderItemId);

        decimal GetRemainingQuantity(
            int purchaseOrderItemId);

        bool ReceiptNumberExists(
            string receiptNumber,
            int? excludedId = null);

        void Cancel(
    int id,
    string cancellationReason,
    string cancelledBy);
    }
}