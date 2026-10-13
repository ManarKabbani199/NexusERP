using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;
using System.Collections.Generic;

namespace NexusERP.Domain.Entities
{
    public class Position : BaseEntity
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string TitleAr { get; set; }

        [Required]
        [StringLength(150)]
        public string TitleEn { get; set; }

        [StringLength(1000)]
        public string DescriptionAr { get; set; }

        [StringLength(1000)]
        public string DescriptionEn { get; set; }

        [StringLength(50)]
        public string JobLevel { get; set; }

        public bool IsManagerial { get; set; }

        public int ApprovedHeadCount { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Employee> Employees
        {
            get;
            set;
        }
    }
}