using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Entities
{
    public class Audit
    {
        [ForeignKey(nameof(CreatedBy))]
        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UpdatedBy))]
        public string? UpdatedByUserId { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
