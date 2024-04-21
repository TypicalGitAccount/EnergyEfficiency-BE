
using static EnergyEfficiencyBE.Constants;

namespace EnergyEfficiencyBE.Models.Entities.EfficiencyClass
{
    public class PeakEnergyConsumption : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string BuildingType {  get; set; }
        public int StoriesMin { get; set; }
        public int StoriesMax { get; set; }
        public TemperatureZone TemperatureZone { get; set; }
        public string Formula { get; set; }
    }
}
