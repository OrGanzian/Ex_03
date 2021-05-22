using System;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        public enum eLicenseType
        {
            A = 1,
            B1,
            AA,
            BB,
        }

        private const float k_MaxBattery = 1.8f;

        public Motorcycle(string i_ModelName, string i_LicenseNumber, string i_WheelModel, Engine.eTypeOfEngine i_EngineTypeSource)
            : base(i_ModelName, i_LicenseNumber, i_EngineTypeSource)
        {
            const int k_NumOfWheels = 2;

            for (int i = 0; i < k_NumOfWheels; i++)
            {
                m_Wheels.Add(new Wheel(i_WheelModel, (float)Wheel.eMaxAirPressure.Motorcycle));
            }

            SetFuel();
        }

        public int EngineVolume { get; set; }

        public eLicenseType LicenseType { get; set; }

        public override void SetFuel()
        {
            if (EngineType is Fuel)
            {
                ((Fuel)EngineType).FuelType = Fuel.eFuelType.Octan98;
                EngineType.CurrCapacity = (float)Fuel.eMaximumFuelCapacity.Motorcycle;
            }
            else
            {
                ((Battery)EngineType).FuelType = Battery.eTypeOfFuel.ElectricalPower;
                EngineType.CurrCapacity = k_MaxBattery;
            }
        }

        public override string ToString()
        {
            string motorcycleInfo = string.Format("License type = {0}{2}Engine Volume = {1}{2}", LicenseType, EngineVolume, Environment.NewLine);
            return base.ToString() + motorcycleInfo;
        }
    }
}
