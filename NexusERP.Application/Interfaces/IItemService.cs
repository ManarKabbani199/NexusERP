using NexusERP.Domain.Entities;
using System.Collections.Generic;

namespace NexusERP.Application.Interfaces
{
    public interface IItemService
    {
        IEnumerable<Item> GetAll();

        Item GetById(int id);

        void Create(Item item);

        void Update(Item item);

        void Delete(int id);
    }
}