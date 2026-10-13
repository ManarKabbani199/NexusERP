using NexusERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NexusERP.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        public Warehouse()
        {
            WarehouseStocks = new HashSet<WarehouseStock>();
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

        [StringLength(250)]
        public string AddressAr { get; set; }

        [StringLength(250)]
        public string AddressEn { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        public int BranchId { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ICollection<WarehouseStock> WarehouseStocks
        {
            get;
            set;
        }

      
    }
}