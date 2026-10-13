using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using System;
using System.Collections.Generic;

namespace NexusERP.Application.Services
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ItemCategory> GetAll()
        {
            return _unitOfWork.ItemCategories.GetAll();
        }

        public ItemCategory GetById(int id)
        {
            return _unitOfWork.ItemCategories.GetById(id);
        }

        public void Create(ItemCategory itemCategory)
        {
            if (itemCategory == null)
            {
                throw new ArgumentNullException(nameof(itemCategory));
            }

            _unitOfWork.ItemCategories.Add(itemCategory);
            _unitOfWork.SaveChanges();
        }

        public void Update(ItemCategory itemCategory)
        {
            if (itemCategory == null)
            {
                throw new ArgumentNullException(nameof(itemCategory));
            }

            _unitOfWork.ItemCategories.Update(itemCategory);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            ItemCategory itemCategory =
                _unitOfWork.ItemCategories.GetById(id);

            if (itemCategory == null)
            {
                return;
            }

            _unitOfWork.ItemCategories.Delete(itemCategory);
            _unitOfWork.SaveChanges();
        }
    }
}