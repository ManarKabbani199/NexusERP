using NexusERP.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting.Activation;


namespace NexusERP.Domain.Entities
{
    public class Company : BaseEntity
    {
        public Company()
        {
            Branches = new HashSet<Branch>();
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

        [StringLength(50)]
        public string CommercialRegistrationNumber { get; set; }

        [StringLength(50)]
        public string TaxNumber { get; set; }

        [StringLength(250)]
        public string AddressAr { get; set; }

        [StringLength(250)]
        public string AddressEn { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(200)]
        [System.ComponentModel.DataAnnotations.UrlAttribute]
        public string Website { get; set; }

        [StringLength(250)]
        public string LogoPath { get; set; }

        [Required]
        [StringLength(3)]
        public string BaseCurrencyCode { get; set; } = "KWD";

        [Required]
        [StringLength(10)]
        public string DefaultLanguage { get; set; } = "ar";

        public virtual ICollection<Branch> Branches { get; set; }
    }
}