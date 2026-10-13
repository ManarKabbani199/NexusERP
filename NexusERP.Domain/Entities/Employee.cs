using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;
using NexusERP.Domain.Enums;

namespace NexusERP.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Employee()
        {
            DirectReports = new HashSet<Employee>();
        }

        [Required]
        [StringLength(30)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string LastNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstNameEn { get; set; }

        [Required]
        [StringLength(100)]
        public string LastNameEn { get; set; }

        [StringLength(20)]
        public string CivilId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [StringLength(100)]
        public string Nationality { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }

        public decimal BasicSalary { get; set; }

        public int BranchId { get; set; }

        public virtual Branch Branch { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public int? ManagerEmployeeId { get; set; }

        public virtual Employee Manager { get; set; }

        public virtual ICollection<Employee> DirectReports
        {
            get;
            set;
        }

        [StringLength(128)]
        public string IdentityUserId { get; set; }
    }
}