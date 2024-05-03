using System.ComponentModel.DataAnnotations;

namespace Garage3._0.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage="Enter personnumer")]
        [Display(Name ="Personnumer")]
        [RegularExpression("d{6}(?:\\d{2})?[-\\s]?\\d{4}", ErrorMessage="Uncorrect personnumer")]
        public string SocialSecurityNr { get; set; }

        [Required(ErrorMessage = "Please enter firstname!")]
        [Display(Name = "Firstname")]
        [MinLength(3)]
        [MaxLength(10)]
      
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Enter surname")]
        [Display(Name = "Surname")]
        [MinLength(3)]
        [MaxLength(20)]
       
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid surname")]
        public string Surname { get; set; }
        public ICollection<Vehicle> VehicleList { get; set; }
    }
}
