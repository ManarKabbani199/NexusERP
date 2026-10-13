using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Interfaces
{
    public interface IWarehouseService
    {
        IEnumerable<Warehouse> GetAll();

        Warehouse GetById(int id);

        void Create(Warehouse warehouse);

        void Update(Warehouse warehouse);

        void Delete(int id);
    }
}