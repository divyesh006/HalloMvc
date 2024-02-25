
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace HalloDoc_mvc.Entities.Patient
{
    public class FamilyForm
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "First name cannot contain numbers")]
        [StringLength(100)]
        public string FamilyFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "First name cannot contain numbers")]
        [StringLength(100)]
        public string FamilyLastName { get; set; }

        [Required(ErrorMessage = "Please enter a Mobile Number.")]
        [RegularExpression(@"([0-9]*)", ErrorMessage = "Please enter only numbers.")]
        [StringLength(100)]
        public string FamilyMobile { get; set; }

        [Required(ErrorMessage = "Please enter an Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        [StringLength(50)]
        public string FamilyEmail { get; set; }

        [StringLength(100)]
        public string Relation{ get; set; }

        [StringLength(100)]
        public string? Symptomos { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Last name cannot contain numbers")]
        [StringLength(100)]
        public string PatientFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Last name cannot contain numbers")]
        [StringLength(100)]
        public string PatientLastName { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please enter a valid Street name")]
        [StringLength(100)]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please enter a valid City name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        [StringLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a valid State name")]
        [RegularExpression(@"([a-zA-Z]*)", ErrorMessage = "Please enter only alphabets.")]
        [StringLength(100)]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter a valid ZipCode")]
        [StringLength(6, ErrorMessage = "Zipcode must be length of 6.")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter an Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Email is not valid.")]
        [StringLength(50)]
        public string PatientEmail { get; set; }

        [Required(ErrorMessage = "Please enter a Mobile Number.")]
        [RegularExpression(@"([0-9]*)", ErrorMessage = "Please enter only numbers.")]
        [StringLength(20)]
        public string PatientMobile { get; set; }

        [StringLength(10)]
        public string? Room { get; set; }

        public IFormFile? file { get; set; }

    }
}
