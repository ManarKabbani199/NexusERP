using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using System;
using System.Collections.Generic;

namespace NexusERP.Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _unitOfWork.Warehouses.GetAll();
        }

        public Warehouse GetById(int id)
        {
            return _unitOfWork.Warehouses.GetById(id);
        }

        public void Create(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _unitOfWork.Warehouses.Add(warehouse);
            _unitOfWork.SaveChanges();
        }

        public void Update(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _unitOfWork.Warehouses.Update(warehouse);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var warehouse = _unitOfWork.Warehouses.GetById(id);

            if (warehouse == null)
                throw new InvalidOperationException("المستودع غير موجود.");

            _unitOfWork.Warehouses.Delete(warehouse);
            _unitOfWork.SaveChanges();
        }
    }
}