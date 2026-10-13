using System.Collections.Generic;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Department> GetAll()
        {
            return _unitOfWork.Departments.GetAll();
        }

        public Department GetById(int id)
        {
            return _unitOfWork.Departments.GetById(id);
        }

        public void Create(Department department)
        {
            _unitOfWork.Departments.Add(department);
            _unitOfWork.SaveChanges();
        }

        public bool Update(Department department)
        {
            var existingDepartment =
                _unitOfWork.Departments.GetById(department.Id);

            if (existingDepartment == null)
            {
                return false;
            }

            existingDepartment.Code = department.Code;
            existingDepartment.NameAr = department.NameAr;
            existingDepartment.NameEn = department.NameEn;
            existingDepartment.DescriptionAr =
                department.DescriptionAr;
            existingDepartment.DescriptionEn =
                department.DescriptionEn;
            existingDepartment.BranchId = department.BranchId;
            existingDepartment.ParentDepartmentId =
                department.ParentDepartmentId;
            existingDepartment.IsActive = department.IsActive;

            _unitOfWork.Departments.Update(existingDepartment);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var department =
                _unitOfWork.Departments.GetById(id);

            if (department == null)
            {
                return false;
            }

            _unitOfWork.Departments.Delete(department);
            _unitOfWork.SaveChanges();

            return true;
        }
    }
}