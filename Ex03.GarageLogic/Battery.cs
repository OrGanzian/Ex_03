using System;

namespace Ex03.GarageLogic
{
    public class Battery : Engine
    {
        public enum eTypeOfFuel
        {
            ElectricalPower,
        }

        public eTypeOfFuel FuelType { get; set; }

        public override string ToString()
        {
            return string.Format("Type of engine = Battery{1} Battery type = {0}{1}", FuelType, Environment.NewLine);
        }

        public void FuelTypeValidation(eTypeOfFuel i_FuelType)
        {
            if (FuelType != i_FuelType)
            {
                throw new ArgumentException();
            }
        }
    }
}