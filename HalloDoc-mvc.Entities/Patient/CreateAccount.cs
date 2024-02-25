
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HalloDoc.Entities.Patient
{
    public class CreateAccount
    {

        [Required(ErrorMessage = "Please enter an Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        [StringLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "character varying")]
        public string PasswordHash { get; set; }

        [Column(TypeName = "character varying")]
        public string ConfirmPasswordHash { get; set; }
    }
}