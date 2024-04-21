using EnergyEfficiencyBE.Context;
using EnergyEfficiencyBE.Models.Entities.EfficiencyClass;

namespace EnergyEfficiencyBE.Repositories.EfficiencyClass
{
    public class PeakEnergyConsumptionRepository : BaseRepository<PeakEnergyConsumption>, IPeakEnergyConsumptionRepository
    {
        public PeakEnergyConsumptionRepository(RelationalContext context) : base(context) {}
    }
}
