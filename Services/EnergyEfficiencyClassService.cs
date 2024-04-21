using EnergyEfficiencyBE.Repositories.EfficiencyClass;
using Microsoft.IdentityModel.Tokens;
using NCalc;
using static EnergyEfficiencyBE.Constants;

namespace EnergyEfficiencyBE.Services
{
    //there's always a parameter - public/private building --> PublicBuilding and PrivateBuilding service implementation?
    public class EnergyEfficiencyClassService : IEnergyEfficiencyClassService
    {
        protected IPeakEnergyConsumptionRepository _peakEnergyConsumptionRepository;

        public EnergyEfficiencyClassService(IPeakEnergyConsumptionRepository peakEnergyConsumptionRepository)
        {
            _peakEnergyConsumptionRepository = peakEnergyConsumptionRepository;
        }

        //getEfficiencyValue in range from db
        public EnergyEfficiencyClass getClass() { throw new NotImplementedException(); }

        protected int getEfficiencyValue(string building, TemperatureZone tempZone, decimal totalInnerArea, int stories)
        {
            decimal peak = getPeakEnergyConsumption(building, tempZone, totalInnerArea, stories);
            return (int)(( (getEnergyConsumption() - peak) / peak ) * 100);
        }

        protected decimal getPeakEnergyConsumption(string building, TemperatureZone tempZone, decimal totalInnerArea, int stories)
        {
            var formula = _peakEnergyConsumptionRepository
                .FindByConditionAsync(val => val.BuildingType == building && val.TemperatureZone == tempZone && val.StoriesMin <= stories && stories <= val.StoriesMax)
                .Result?.Select(val => val.Formula).FirstOrDefault();

            if (formula.IsNullOrEmpty())
            {
                throw new Exception($"No formula found for {building} building type!");
            }

            var expression = new Expression(formula);
            if (expression.Parameters["factor"] != null)
            {
                expression.Parameters["factor"] = getBuildingCompactnessFactor(totalInnerArea);
            }

            return (decimal)expression.Evaluate();
        }

        //=згідно з ДБН В.2.6-31:2016 "Теплова ізоляція будівель". = площа внутрішніх поверхонь, включаючи перекриття і дах / getHeatedOrCooledArea
        protected decimal getBuildingCompactnessFactor(decimal totalInnerArea) { return totalInnerArea / getHeatedOrCooledArea(); }

        protected decimal getEnergyConsumption() { return getCoolingEnergyConsumption() + getHeatingEnergyConsumption(); }

        protected decimal getHeatingEnergyConsumption() { return getYearlyHeatingEnergyConsumption() / getHeatedOrCooledArea(); }

        protected decimal getHeatedOrCooledArea() { throw new NotImplementedException(); }
        //or (both from user input, by transforming it according to https://zakon.rada.gov.ua/laws/show/z0822-18#n52
        protected decimal getHeatedOrCooledVolume() { throw new NotImplementedException(); }

        protected decimal getYearlyHeatingEnergyConsumption() { return getYearlyWarmthSystemExitEnergy() + getYearlyWarmthSystemHeatLoss(); }

        protected decimal getYearlyWarmthSystemExitEnergy() { return getYearlyUnutilizedDistributionHeatLoss() + getYearlyDistributionSystemExitEnergy(); }

        protected decimal getYearlyUnutilizedDistributionHeatLoss() { return getUtilisationalHeatLoss() + (getUtilisationalHeatLoss() - getUtilisedHeatLoss()); }

        //= sum of each mont EEE(linear coefficient from https://zakon.rada.gov.ua/laws/show/z0822-18#n248 * ( середня температура теплоносія в зоні згідно з таблицею А.2 ДСТУ 9190 - температура оточуючого середовища упродовж і-го місяця, °C) * довжина j-го трубопроводу, м * години опалення упродовж і-го місяця
        protected decimal getUtilisationalHeatLoss() { throw new NotImplementedException(); }

        //= getUtilisationalHeatLoss() * 0.9 * heating usage coefficient розрахований згідно з пунктом 12.2 ДСТУ 9190.
        protected decimal getUtilisedHeatLoss() { throw new NotImplementedException(); }

        //= getYearlyHeatingSubsystemExitEnergy + (1 - 0.8 * heating usage coefficient розрахований згідно з пунктом 12.2 ДСТУ 9190.) * getYearlyUtlisableHeatingSubsystemHeatLoss
        protected decimal getYearlyDistributionSystemExitEnergy() { throw new NotImplementedException(); }

        //= теплота, яку необхідно подати до кондиціонованого об’єму для підтримки температури упродовж визначеного періоду часу, by кВт×год визначається згідно з підпунктом 7.2.1 розділу 7 ДСТУ 9190
        protected decimal getYearlyHeatingSubsystemExitEnergy() { throw new NotImplementedException(); }
        //= ((hidraulic coherence coeff from https://zakon.rada.gov.ua/laws/show/z0822-18#n260 * коефіцієнт, що враховує застосування періодичного теплового режиму приміщення(1 for constant heaing mode, 0.98 for regulable mode, 0.97 for regulable mode with integrated feedback) * only for rediant heating systems, a coef from https://zakon.rada.gov.ua/laws/show/z0822-18#n260 )/getHeatingSystemComponentEfficiency-1)*getYearlyHeatingSubsystemExitEnergy
        protected decimal getYearlyUtlisableHeatingSubsystemHeatLoss() { throw new NotImplementedException(); }

        //= 1 / (4 - (https://zakon.rada.gov.ua/laws/show/z0822-18#n260  ηstr + ηctr + ηemb ))
        protected decimal getHeatingSystemComponentEfficiency() { throw new NotImplementedException(); }

        //= getYearlyWarmthSystemExitEnergy() * ( 1 - https://zakon.rada.gov.ua/laws/show/z0822-18#n236) / https://zakon.rada.gov.ua/laws/show/z0822-18#n236
        protected decimal getYearlyWarmthSystemHeatLoss() { throw new NotImplementedException(); }



        protected decimal getCoolingEnergyConsumption() { return getYearlyCoolingEnergyConsumption() / getHeatedOrCooledArea(); }

        protected decimal getYearlyCoolingEnergyConsumption() { return getGeneralGeneratingSubsystemHeatLoss() + getGeneratingSubsystemExitEnergy(); }

        //= getGeneratingSubsystemExitEnergy() * ( 1 - https://zakon.rada.gov.ua/laws/show/z0822-18#n292) / https://zakon.rada.gov.ua/laws/show/z0822-18#n292
        protected decimal getGeneralGeneratingSubsystemHeatLoss() { throw new NotImplementedException(); }

        //= getDistributionSubsystemEnterEnergy() / 	
//        ефективність автоматичного управління/регулювання, залежно від класу ефективності системи управління/регулювання приймають наступні значення:
//для систем класу А - ηC,ac = 0,99;
//для систем класу B - ηC,ac = 0,93;
//для систем класу C - ηC,ac = 0,88;
//для систем класу D - ηC,ac = 0,82;
        protected decimal getGeneratingSubsystemExitEnergy() { throw new NotImplementedException(); }

        //sum of(енергію виходу для підсистеми розподілення упродовж і-го місяця, Вт×год, приймають рівною енергопотребі для охолодження у даному місяці QC,nd,i та для даної комбінації зон, яку обслуговує та сама підсистема виділення/тепловіддачі та розподілення, Вт×год, визначена згідно з підрозділом 7.2.2 розділу 7 ДСТУ 9190) / 1000 + getYearlyCooledAirDistributionSubsystemHeatLoss()
        protected decimal getDistributionSubsystemEnterEnergy() { throw new NotImplementedException(); }

        //річні енергопотреби для охолодження, кВт×год, визначені згідно з розділом 14 ДСТУ 9190; * ((1 - ступінь утилізації теплообміну при охолодженні https://zakon.rada.gov.ua/laws/show/z0822-18#n296) + (ступінь явної утилізації теплообміну при охолодженні1 - https://zakon.rada.gov.ua/laws/show/z0822-18#n296) + (ступінь утилізації підсистеми розподілення1 - https://zakon.rada.gov.ua/laws/show/z0822-18#n296))
        protected decimal getYearlyCooledAirDistributionSubsystemHeatLoss() { throw new NotImplementedException(); }
    }
}
