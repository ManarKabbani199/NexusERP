using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IBranchService
    {
        IEnumerable<Branch> GetAll();

        Branch GetById(int id);

        void Create(Branch branch);

        bool Update(Branch branch);

        bool Delete(int id);
    }
}