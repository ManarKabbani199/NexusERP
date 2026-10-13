using System.ComponentModel.DataAnnotations;
using NexusERP.Domain.Common;

namespace NexusERP.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(150)]
        public string NameEn { get; set; }

        [StringLength(150)]
        public string ContactPerson { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string TaxNumber { get; set; }

        [StringLength(300)]
        public string AddressAr { get; set; }

        [StringLength(300)]
        public string AddressEn { get; set; }

        public int PaymentTermsDays { get; set; }
    }
}