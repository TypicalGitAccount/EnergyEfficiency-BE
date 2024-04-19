using System.ComponentModel.DataAnnotations;

namespace EnergyEfficiencyBE.Dtos
{
    public class UserUpdateDto : UserBaseDto
    {
        [Required] public override Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public string? AvatarId { get; set; }

        public string? Phone { get; set; }
        public string? Telegram { get; set; }
    }
}
