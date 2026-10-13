using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IPositionService
    {
        IEnumerable<Position> GetAll();

        Position GetById(int id);

        IEnumerable<Position> GetByDepartmentId(int departmentId);

        void Create(Position position);

        bool Update(Position position);

        bool Delete(int id);
    }
}