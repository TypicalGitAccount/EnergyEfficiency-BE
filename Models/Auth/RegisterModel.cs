using System.ComponentModel.DataAnnotations;

namespace EnergyEfficiencyBE.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Name { get; set; }

        [Required][EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
