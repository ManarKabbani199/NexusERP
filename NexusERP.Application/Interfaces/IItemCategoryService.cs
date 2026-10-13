using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Interfaces
{
    public interface IItemCategoryService
    {
        IEnumerable<ItemCategory> GetAll();

        ItemCategory GetById(int id);

        void Create(ItemCategory itemCategory);

        void Update(ItemCategory itemCategory);

        void Delete(int id);
    }
}