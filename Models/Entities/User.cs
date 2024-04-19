namespace EnergyEfficiencyBE.Models.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string IdentityId { get; set; }

        public string Name { get; set; } = null!;
        public string? AvatarId { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Telegram { get; set; }
    }
}
