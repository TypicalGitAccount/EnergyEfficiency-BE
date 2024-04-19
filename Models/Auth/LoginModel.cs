using System.ComponentModel.DataAnnotations;

namespace EnergyEfficiencyBE.Models.Auth
{
    public class LoginModel
    {
        [Required][EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
