using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _unitOfWork.Employees.GetAll();
        }

        public Employee GetById(int id)
        {
            return _unitOfWork.Employees.GetById(id);
        }

        public void Create(Employee employee)
        {
            _unitOfWork.Employees.Add(employee);
            _unitOfWork.SaveChanges();
        }

        public bool Update(Employee employee)
        {
            var existingEmployee =
                _unitOfWork.Employees.GetById(employee.Id);

            if (existingEmployee == null)
            {
                return false;
            }

            existingEmployee.EmployeeNumber = employee.EmployeeNumber;

            existingEmployee.FirstNameAr = employee.FirstNameAr;
            existingEmployee.LastNameAr = employee.LastNameAr;

            existingEmployee.FirstNameEn = employee.FirstNameEn;
            existingEmployee.LastNameEn = employee.LastNameEn;

            existingEmployee.CivilId = employee.CivilId;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.Nationality = employee.Nationality;

            existingEmployee.Phone = employee.Phone;
            existingEmployee.Email = employee.Email;

            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.TerminationDate = employee.TerminationDate;

            existingEmployee.EmploymentType = employee.EmploymentType;
            existingEmployee.EmploymentStatus = employee.EmploymentStatus;

            existingEmployee.BasicSalary = employee.BasicSalary;

            existingEmployee.BranchId = employee.BranchId;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.PositionId = employee.PositionId;
            existingEmployee.ManagerEmployeeId =
                employee.ManagerEmployeeId;

            existingEmployee.IdentityUserId =
                employee.IdentityUserId;

            _unitOfWork.Employees.Update(existingEmployee);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var employee = _unitOfWork.Employees.GetById(id);

            if (employee == null)
            {
                return false;
            }

            _unitOfWork.Employees.Delete(employee);
            _unitOfWork.SaveChanges();

            return true;
        }
    }
}