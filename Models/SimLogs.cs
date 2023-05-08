using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sim7600collector.Models
{
    public class SimLogs
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Device { get; set; }

        [Required]
        public string? Logitem { get; set; }

        [Required]
        public string? Message { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }
}