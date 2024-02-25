using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Entities.DataModels;

public partial class AspNetUserRole
{
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Key]
    [StringLength(128)]
    public string RoleId { get; set; } = null!;
}
