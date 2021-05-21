using System;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        public enum eMaxAirPressure
        {
            Truck = 26,
            Motorcycle = 30,
            Car = 32,
        }

        public Wheel(string i_Manufacturer, float i_MaxAirPressure)
        {
            this.MaxAirPressure = i_MaxAirPressure;
            this.ManufacturerName = i_Manufacturer;
        }

        public float MaxAirPressure { get; }

        public float CurrentAirPressure { get; set; }

        public string ManufacturerName { get; }


        public void FillToMaximum()
        {
            this.CurrentAirPressure = this.MaxAirPressure;
        }

        public void AddAirPressure(float i_AmountOFAirToAdd)
        {
            if (i_AmountOFAirToAdd + this.CurrentAirPressure > this.MaxAirPressure || i_AmountOFAirToAdd < 0)
            {
                throw new ValueOutOfRangeException(0, this.MaxAirPressure);
            }
            else
            {
                this.CurrentAirPressure = this.CurrentAirPressure + i_AmountOFAirToAdd;
            }
        }
        public override string ToString()
        {
            return string.Format("Wheel manufacturer company name = {0}{3}Current air pressure = {1}{3}Maximum air pressure = {2}{3}", ManufacturerName, CurrentAirPressure, MaxAirPressure, Environment.NewLine);
        }
    }
}
