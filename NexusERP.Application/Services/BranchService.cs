using System.Collections.Generic;
using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Branch> GetAll()
        {
            return _unitOfWork.Branches.GetAll();
        }

        public Branch GetById(int id)
        {
            return _unitOfWork.Branches.GetById(id);
        }

        public void Create(Branch branch)
        {
            _unitOfWork.Branches.Add(branch);
            _unitOfWork.SaveChanges();
        }

        public bool Update(Branch branch)
        {
            var existingBranch =
                _unitOfWork.Branches.GetById(branch.Id);

            if (existingBranch == null)
            {
                return false;
            }

            existingBranch.Code = branch.Code;
            existingBranch.NameAr = branch.NameAr;
            existingBranch.NameEn = branch.NameEn;
            existingBranch.AddressAr = branch.AddressAr;
            existingBranch.AddressEn = branch.AddressEn;
            existingBranch.Phone = branch.Phone;
            existingBranch.CompanyId = branch.CompanyId;
            existingBranch.IsActive = branch.IsActive;

            _unitOfWork.Branches.Update(existingBranch);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var branch =
                _unitOfWork.Branches.GetById(id);

            if (branch == null)
            {
                return false;
            }

            _unitOfWork.Branches.Delete(branch);
            _unitOfWork.SaveChanges();

            return true;
        }
    }
}