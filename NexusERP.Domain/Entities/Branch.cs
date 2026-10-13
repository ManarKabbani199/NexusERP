using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public Branch()
        {
            Departments = new HashSet<Department>();
            Employees = new HashSet<Employee>();
            Warehouses = new HashSet<Warehouse>();
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

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Department> Departments
        {
            get;
            set;
        }

        public virtual ICollection<Employee> Employees
        {
            get;
            set;
        }
        public virtual ICollection<Warehouse> Warehouses
        {
            get;
            set;
        }
    }
}