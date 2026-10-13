using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Interfaces
{
    public interface IWarehouseStockService
    {
        IEnumerable<WarehouseStock> GetAll();

        WarehouseStock GetById(int id);

        WarehouseStock GetByWarehouseAndItem(
            int warehouseId,
            int itemId);

        void Create(WarehouseStock warehouseStock);

        void Update(WarehouseStock warehouseStock);

        void Delete(int id);
    }
}