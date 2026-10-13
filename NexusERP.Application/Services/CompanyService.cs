using System.Collections.Generic;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Company> GetAll()
        {
            return _unitOfWork.Companies.GetAll();
        }

        public Company GetById(int id)
        {
            return _unitOfWork.Companies.GetById(id);
        }

        public void Create(Company company)
        {
            _unitOfWork.Companies.Add(company);
            _unitOfWork.SaveChanges();
        }

        public bool Update(Company company)
        {
            var existingCompany =
                _unitOfWork.Companies.GetById(company.Id);

            if (existingCompany == null)
            {
                return false;
            }

            existingCompany.Code = company.Code;
            existingCompany.NameAr = company.NameAr;
            existingCompany.NameEn = company.NameEn;
            existingCompany.AddressAr = company.AddressAr;
            existingCompany.AddressEn = company.AddressEn;
            existingCompany.Phone = company.Phone;
            existingCompany.IsActive = company.IsActive;

            _unitOfWork.Companies.Update(existingCompany);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var company =
                _unitOfWork.Companies.GetById(id);

            if (company == null)
            {
                return false;
            }

            _unitOfWork.Companies.Delete(company);
            _unitOfWork.SaveChanges();

            return true;
        }
    }
}