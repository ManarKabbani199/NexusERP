using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using NexusERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NexusERP.Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Position> GetAll()
        {
            return _unitOfWork.Positions.GetAll();
        }

        public Position GetById(int id)
        {
            return _unitOfWork.Positions.GetById(id);
        }

        public IEnumerable<Position> GetByDepartmentId(int departmentId)
        {
            return _unitOfWork.Positions
                .GetAll()
                .Where(p => p.DepartmentId == departmentId)
                .ToList();
        }

        public void Create(Position position)
        {
            _unitOfWork.Positions.Add(position);
            _unitOfWork.SaveChanges();
        }

        public bool Update(Position position)
        {
            var existingPosition =
                _unitOfWork.Positions.GetById(position.Id);

            if (existingPosition == null)
            {
                return false;
            }

            existingPosition.Code = position.Code;
            existingPosition.TitleAr = position.TitleAr;
            existingPosition.TitleEn = position.TitleEn;
            existingPosition.DescriptionAr = position.DescriptionAr;
            existingPosition.DescriptionEn = position.DescriptionEn;
            existingPosition.JobLevel = position.JobLevel;
            existingPosition.IsManagerial = position.IsManagerial;
            existingPosition.ApprovedHeadCount =
                position.ApprovedHeadCount;
            existingPosition.DepartmentId = position.DepartmentId;

            _unitOfWork.Positions.Update(existingPosition);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var position =
                _unitOfWork.Positions.GetById(id);

            if (position == null)
            {
                return false;
            }

            _unitOfWork.Positions.Delete(position);
            _unitOfWork.SaveChanges();

            return true;
        }

        public void Approve(int id)
        {
            var purchaseOrder =
                _unitOfWork.PurchaseOrders.GetById(id);

            if (purchaseOrder == null ||
                purchaseOrder.IsDeleted)
            {
                throw new InvalidOperationException(
                    "أمر الشراء غير موجود.");
            }

            if (purchaseOrder.Status !=
                PurchaseOrderStatus.Draft)
            {
                throw new InvalidOperationException(
                    "يمكن اعتماد أوامر الشراء المسودة فقط.");
            }

            if (purchaseOrder.Items == null ||
                !purchaseOrder.Items.Any(
                    item => !item.IsDeleted))
            {
                throw new InvalidOperationException(
                    "لا يمكن اعتماد أمر شراء لا يحتوي على أصناف.");
            }

            purchaseOrder.Status =
                PurchaseOrderStatus.Approved;

            purchaseOrder.UpdatedAt = DateTime.Now;

            _unitOfWork.PurchaseOrders.Update(
                purchaseOrder);

            _unitOfWork.SaveChanges();
        }
    }
}