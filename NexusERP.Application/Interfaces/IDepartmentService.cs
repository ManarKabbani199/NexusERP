using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAll();

        Department GetById(int id);

        void Create(Department department);

        bool Update(Department department);

        bool Delete(int id);
    }
}