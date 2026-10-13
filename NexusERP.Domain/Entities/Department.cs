using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class Department : BaseEntity
    {
        public Department()
        {
            ChildDepartments = new HashSet<Department>();
            Positions = new HashSet<Position>();
            Employees = new HashSet<Employee>();
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

        public int BranchId { get; set; }

        public virtual Branch Branch { get; set; }

        public int? ParentDepartmentId { get; set; }

        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Department> ChildDepartments
        {
            get;
            set;
        }

        public virtual ICollection<Position> Positions
        {
            get;
            set;
        }

        public virtual ICollection<Employee> Employees
        {
            get;
            set;
        }
    }
}