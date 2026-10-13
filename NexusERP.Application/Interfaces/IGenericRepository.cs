using System.Collections.Generic;
using NexusERP.Domain.Common;

namespace NexusERP.Application.Interfaces
{
    public interface IGenericRepository<T>
        where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}