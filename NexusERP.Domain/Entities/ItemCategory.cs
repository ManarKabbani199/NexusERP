using NexusERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NexusERP.Domain.Entities
{
    public class ItemCategory : BaseEntity
    {

        public ItemCategory()
        {
            Items = new HashSet<Item>();
        }

        [Required]
        [StringLength(20)]
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

        public virtual ICollection<Item> Items { get; set; }
    }
}