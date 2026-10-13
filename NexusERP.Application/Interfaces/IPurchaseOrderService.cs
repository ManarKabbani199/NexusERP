using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPurchaseOrderService
    {
        IEnumerable<PurchaseOrder> GetAll();

        PurchaseOrder GetById(int id);

        void Create(PurchaseOrder purchaseOrder);

        void Update(PurchaseOrder purchaseOrder);

        void Delete(int id);

        bool OrderNumberExists(string orderNumber, int? excludedId = null);

        void Approve(int id, string approvedBy);

        void Cancel( int id,  string cancellationReason, string cancelledBy);


    }
}