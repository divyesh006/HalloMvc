using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_mvc.Entities.DataModels;

namespace HalloDoc_mvc.Entities.Admin
{
    public class PatientData
    {
        [StringLength(100)]
        public string Notes { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime BirthDate { get; set; }

        public string Birthdate { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public Request request { get; set; }

        public RequestClient client { get; set; }
    }
}
