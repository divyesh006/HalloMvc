using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_mvc.Entities.DataModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HalloDoc_mvc.Entities.Admin
{
    public class AdminDashboard
    {
        public int? New { get; set; }

        public int? Pending { get; set; }

        public int? Active { get; set; }

        public int? Conclude { get; set; }

        public int? Close { get; set; }

        public int? Unpaid { get; set; }

        public List<Region> region { get; set; }

    }
}
