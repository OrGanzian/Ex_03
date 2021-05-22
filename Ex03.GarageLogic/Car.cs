using System;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private const int k_NumOfWheels = 4;
        private const float k_MaxBatterySize = 3.2f;

        public enum eColor
        {
            Red = 1,
            Silver,
            White,
            Black,
        }

        public enum eNumOfDoors
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        public Car(string i_Model, string i_LicenseNumber, Engine.eTypeOfEngine i_EngineType, string i_WheelsModel) :
            base(i_Model, i_LicenseNumber, i_EngineType)
        {
            for (int i = 0; i < k_NumOfWheels; i++)
            {
                Wheel wheelToAdd = new Wheel(i_WheelsModel, (float)Wheel.eMaxAirPressure.Car);
                m_Wheels.Add(wheelToAdd);
            }

            SetFuel();
        }

        public eColor CarColor { get; set; }

        public eNumOfDoors NumbersOfDoors { get; set; }

        public override void SetFuel()
        {
            if (EngineType is Fuel)
            {
                ((Fuel)EngineType).FuelType = Fuel.eFuelType.Octan95;
                EngineType.CurrCapacity = (float)Fuel.eMaximumFuelCapacity.Car;
            }
            else
            {
                ((Battery)EngineType).FuelType = Battery.eTypeOfFuel.ElectricalPower;
                EngineType.CurrCapacity = k_MaxBatterySize;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("Car color = {0}{2}Number of doors = {1}{2}", CarColor, NumbersOfDoors, Environment.NewLine);
        }
    }
}
