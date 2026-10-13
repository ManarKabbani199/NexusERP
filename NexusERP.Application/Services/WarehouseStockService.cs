using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Application.Services
{
    public class WarehouseStockService : IWarehouseStockService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseStockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<WarehouseStock> GetAll()
        {
            return _unitOfWork.WarehouseStocks.GetAll();
        }

        public WarehouseStock GetById(int id)
        {
            return _unitOfWork.WarehouseStocks.GetById(id);
        }

        public WarehouseStock GetByWarehouseAndItem(
            int warehouseId,
            int itemId)
        {
            return _unitOfWork.WarehouseStocks
                .GetAll()
                .FirstOrDefault(stock =>
                    stock.WarehouseId == warehouseId &&
                    stock.ItemId == itemId);
        }

        public void Create(WarehouseStock warehouseStock)
        {
            if (warehouseStock == null)
            {
                throw new ArgumentNullException(
                    nameof(warehouseStock));
            }

            if (warehouseStock.Quantity < 0)
            {
                throw new InvalidOperationException(
                    "الكمية لا يمكن أن تكون سالبة.");
            }

            WarehouseStock existingStock =
                GetByWarehouseAndItem(
                    warehouseStock.WarehouseId,
                    warehouseStock.ItemId);

            if (existingStock != null)
            {
                throw new InvalidOperationException(
                    "هذا الصنف مسجل مسبقًا داخل المستودع المحدد.");
            }

            _unitOfWork.WarehouseStocks.Add(warehouseStock);
            _unitOfWork.SaveChanges();
        }

        public void Update(WarehouseStock warehouseStock)
        {
            if (warehouseStock == null)
            {
                throw new ArgumentNullException(
                    nameof(warehouseStock));
            }

            if (warehouseStock.Quantity < 0)
            {
                throw new InvalidOperationException(
                    "الكمية لا يمكن أن تكون سالبة.");
            }

            WarehouseStock duplicateStock =
                _unitOfWork.WarehouseStocks
                    .GetAll()
                    .FirstOrDefault(stock =>
                        stock.WarehouseId ==
                            warehouseStock.WarehouseId &&
                        stock.ItemId ==
                            warehouseStock.ItemId &&
                        stock.Id != warehouseStock.Id);

            if (duplicateStock != null)
            {
                throw new InvalidOperationException(
                    "هذا الصنف مسجل مسبقًا داخل المستودع المحدد.");
            }

            _unitOfWork.WarehouseStocks.Update(warehouseStock);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            WarehouseStock warehouseStock =
                _unitOfWork.WarehouseStocks.GetById(id);

            if (warehouseStock == null)
            {
                return;
            }

            _unitOfWork.WarehouseStocks.Delete(warehouseStock);
            _unitOfWork.SaveChanges();
        }
    }
}