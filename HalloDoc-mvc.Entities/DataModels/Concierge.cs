using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Entities.DataModels;

[Table("Concierge")]
public partial class Concierge
{
    [Key]
    public int ConciergeId { get; set; }

    [StringLength(100)]
    public string? ConciergeName { get; set; }

    [StringLength(150)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Street { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(50)]
    public string? ZipCode { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    public int? RegionId { get; set; }

    public int? RoleId { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Mobile { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("Concierges")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Concierge")]
    public virtual ICollection<RequestConcierge> RequestConcierges { get; set; } = new List<RequestConcierge>();
}
