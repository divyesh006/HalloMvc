using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Entities.DataModels;

[Table("RequestClient")]
public partial class RequestClient
{
    [Key]
    public int RequestClientId { get; set; }

    public int? RequestId { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public int? RegionId { get; set; }

    [StringLength(20)]
    public string? NotiMobile { get; set; }

    [StringLength(50)]
    public string? NotiEmail { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [Column("strMonth")]
    [StringLength(20)]
    public string? StrMonth { get; set; }

    [Column("intYear")]
    public int? IntYear { get; set; }

    [Column("intDate")]
    public int? IntDate { get; set; }

    public bool? IsMobile { get; set; }

    [StringLength(100)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? ZipCode { get; set; }

    public short? CommunicationType { get; set; }

    public short? RemindReservationCount { get; set; }

    public short? RemindHouseCallCount { get; set; }

    public short? IsSetFollowupSent { get; set; }

    [Column("IP")]
    [StringLength(20)]
    public string? Ip { get; set; }

    public short? IsReservationReminderSent { get; set; }

    public short? Latitude { get; set; }

    public short? Longitude { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("RequestClients")]
    public virtual Region? Region { get; set; }

    [ForeignKey("RequestId")]
    [InverseProperty("RequestClients")]
    public virtual Request? Request { get; set; }
}
