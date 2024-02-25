using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Entities.DataModels;

[Table("RequestWiseFile")]
public partial class RequestWiseFile
{
    [Key]
    [Column("RequestWiseFileID")]
    public int RequestWiseFileId { get; set; }

    public int? RequestId { get; set; }

    [StringLength(500)]
    public string? FileName { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    public int? PhysicianId { get; set; }

    public int? AdminId { get; set; }

    public short? DocType { get; set; }

    public bool? IsFrontSide { get; set; }

    public bool? IsCompensation { get; set; }

    [Column("IP")]
    [StringLength(20)]
    public string? Ip { get; set; }

    public bool? IsFinalize { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsPatientRecords { get; set; }

    [ForeignKey("RequestId")]
    [InverseProperty("RequestWiseFiles")]
    public virtual Request? Request { get; set; }
}
