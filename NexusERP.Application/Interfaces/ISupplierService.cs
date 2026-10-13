using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
        Supplier GetById(int id);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int id);
    }
}