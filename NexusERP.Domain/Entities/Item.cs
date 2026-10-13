using NexusERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NexusERP.Domain.Entities
{
    public class Item : BaseEntity
    {

        public Item()
        {
            WarehouseStocks = new HashSet<WarehouseStock>();
        }

        [Required]
        [StringLength(30)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(150)]
        public string NameEn { get; set; }

        [StringLength(500)]
        public string DescriptionAr { get; set; }

        [StringLength(500)]
        public string DescriptionEn { get; set; }

        [Required]
        [StringLength(30)]
        public string UnitOfMeasure { get; set; }

        public decimal MinimumStockLevel { get; set; }

        public int ItemCategoryId { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }

        public virtual ICollection<WarehouseStock> WarehouseStocks
        {
            get;
            set;
        }
    }
}