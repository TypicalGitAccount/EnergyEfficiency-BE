using System.ComponentModel.DataAnnotations;

namespace EnergyEfficiencyBE.Models.Auth
{
    public class RefreshTokenModel : TokenModel
    {
        [Required] public string Email { get; set; }
    }
}
