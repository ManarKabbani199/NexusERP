using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAll();

        Company GetById(int id);

        void Create(Company company);

        bool Update(Company company);

        bool Delete(int id);
    }
}