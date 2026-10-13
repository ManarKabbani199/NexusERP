using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class WarehouseStock : BaseEntity
    {
        public int WarehouseId { get; set; }

        public int ItemId { get; set; }

        public decimal Quantity { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Item Item { get; set; }
    }
}