using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();

        Employee GetById(int id);

        void Create(Employee employee);

        bool Update(Employee employee);

        bool Delete(int id);
    }
}