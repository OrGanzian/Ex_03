using System;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        public float MaximumCarryWeight { get; set; }

        public Truck(string i_ModelName, string i_LicenseNumber, Engine.eTypeOfEngine i_EngineTypeSource, string i_WheelsModel)
             : base(i_ModelName, i_LicenseNumber, i_EngineTypeSource)
        {
            const int k_NumOfWheels = 16;

            for (int i = 0; i < k_NumOfWheels; i++)
            {
                m_Wheels.Add(new Wheel(i_WheelsModel, (float)Wheel.eMaxAirPressure.Truck));
            }

            EngineType.CurrCapacity = (float)Fuel.eMaximumFuelCapacity.Truck;
            SetFuel();
        }

        public bool CarryingDangerousMaterials { get; set; }

        public override void SetFuel()
        {
            ((Fuel)EngineType).FuelType = Fuel.eFuelType.Soler;
        }

        public override string ToString()
        {
            string isDangerousMaterials;
            if (CarryingDangerousMaterials)
            {
                isDangerousMaterials = string.Format("Truck is carrying dangerous materials{0}", Environment.NewLine);
            }
            else
            {
                isDangerousMaterials = string.Format("Truck is not carrying dangerous materials{0}", Environment.NewLine);
            }

            return isDangerousMaterials + base.ToString();
        }
    }
}
