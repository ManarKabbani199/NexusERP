using System;
using System.Collections.Generic;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _unitOfWork.Suppliers.GetAll();
        }

        public Supplier GetById(int id)
        {
            return _unitOfWork.Suppliers.GetById(id);
        }

        public void Add(Supplier supplier)
        {
            _unitOfWork.Suppliers.Add(supplier);
            _unitOfWork.SaveChanges();
        }

        public void Update(Supplier supplier)
        {
            _unitOfWork.Suppliers.Update(supplier);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var supplier = _unitOfWork.Suppliers.GetById(id);

            if (supplier == null)
                return;

            _unitOfWork.Suppliers.Delete(supplier);
            _unitOfWork.SaveChanges();
        }
    }
}