using System;

namespace Ex03.GarageLogic
{
    public class FuelTypeException : Exception
    {
        public FuelTypeException()
            : base(string.Format("Fail! This type of fuel can not be added to this vehicle"))
        {
        }
    }
}
