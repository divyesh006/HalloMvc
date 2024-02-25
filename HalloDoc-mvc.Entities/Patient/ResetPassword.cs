
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HalloDoc_mvc.Entities.DataModels;


namespace HalloDoc.Entities.Patient
{
    public class ResetPassword
    {

        [StringLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "character varying")]
        public string PasswordHash { get; set; }

        [Column(TypeName = "character varying")]
        public string ConfirmPasswordHash { get; set; }

        public AspNetUser Aspuser { get; set; }
    }
}