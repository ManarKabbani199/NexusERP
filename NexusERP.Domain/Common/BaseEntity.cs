using System;
using System.ComponentModel.DataAnnotations;

namespace NexusERP.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(256)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }


        [StringLength(256)]
        public string UpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }

    }
}