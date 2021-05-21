using System;

namespace Ex03.GarageLogic
{
    public class Fuel : Engine
    {

        public enum eMaximumFuelCapacity
        {
            Motorcycle = 6,
            Car = 45,
            Truck = 120,
        }
        public enum eFuelType
        {
            Soler,
            Octan95,
            Octan96,
            Octan98,
        }

        public eFuelType FuelType
        { get; set; }

        public override string ToString()
        {
            return string.Format("EngineType type = Fuel{1}Fuel type = {0}{1}", FuelType, Environment.NewLine);
        }
    }
}
