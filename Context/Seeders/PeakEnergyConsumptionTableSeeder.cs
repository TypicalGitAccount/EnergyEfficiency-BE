using EnergyEfficiencyBE.Models.Entities.EfficiencyClass;
using EnergyEfficiencyBE.Repositories.EfficiencyClass;
using static EnergyEfficiencyBE.Constants;

namespace EnergyEfficiencyBE.Context.Seeders
{
    public class PeakEnergyConsumptionTableSeeder
    {
        public static async Task SeedTableAsync(IPeakEnergyConsumptionRepository repository)
        {
            if (repository.FindAllAsync().Result?.Count() ==
                (typeof(BuildingType).GetProperties().Where(val => val.PropertyType == typeof(string)).Count() + BuildingType.PrivateStoriesClasses - 1 + BuildingType.Public.CommonStoriesClasses - 1)
                * typeof(TemperatureZone).GetProperties().Length)
            {
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.First, StoriesMin = 1, StoriesMax = 3, Formula = "120" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.First, StoriesMin = 4, StoriesMax = 9, Formula = "85" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.First, StoriesMin = 10, StoriesMax = 16, Formula = "75" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.First, StoriesMin = 17, StoriesMax = int.MaxValue, Formula = "70" });

                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.Second, StoriesMin = 1, StoriesMax = 3, Formula = "110" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.Second, StoriesMin = 4, StoriesMax = 9, Formula = "75" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.Second, StoriesMin = 10, StoriesMax = 16, Formula = "70" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Private, TemperatureZone = TemperatureZone.Second, StoriesMin = 17, StoriesMax = int.MaxValue, Formula = "65" });

                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.First, StoriesMin = 1, StoriesMax = 3, Formula = "38 * factor + 15" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.First, StoriesMin = 4, StoriesMax = 9, Formula = "30" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.First, StoriesMin = 10, StoriesMax = int.MaxValue, Formula = "25" });

                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.Second, StoriesMin = 1, StoriesMax = 3, Formula = "34 * factor + 13" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.Second, StoriesMin = 4, StoriesMax = 9, Formula = "25" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Common, TemperatureZone = TemperatureZone.Second, StoriesMin = 10, StoriesMax = int.MaxValue, Formula = "20" });

                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Hotel, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "57 * factor + 60" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Educational, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "55 * factor + 24" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Preschool, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "32" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Healthcare, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "30" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Trading, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "33 * factor + 17" });

                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Hotel, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "50 * factor + 55" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Educational, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "52 * factor + 23" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Preschool, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "28" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Healthcare, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "26" });
                await repository.CreateAsync(new PeakEnergyConsumption() { BuildingType = BuildingType.Public.Trading, TemperatureZone = TemperatureZone.Second, StoriesMin = 0, StoriesMax = int.MaxValue, Formula = "26 * factor + 15" });
            }
        }
    }
}
