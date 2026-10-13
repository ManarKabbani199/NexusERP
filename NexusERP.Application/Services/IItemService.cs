using NexusERP.Application.Interfaces;
using NexusERP.Domain.Entities;
using System;
using System.Collections.Generic;

namespace NexusERP.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Item> GetAll()
        {
            return _unitOfWork.Items.GetAll();
        }

        public Item GetById(int id)
        {
            return _unitOfWork.Items.GetById(id);
        }

        public void Create(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _unitOfWork.Items.Add(item);
            _unitOfWork.SaveChanges();
        }

        public void Update(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _unitOfWork.Items.Update(item);
            _unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            Item item = _unitOfWork.Items.GetById(id);

            if (item == null)
            {
                return;
            }

            _unitOfWork.Items.Delete(item);
            _unitOfWork.SaveChanges();
        }
    }
}