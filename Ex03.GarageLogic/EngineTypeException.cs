using System;
namespace Ex03.GarageLogic
{
    public class EngineTypeException : Exception
    {
        public EngineTypeException() : base(string.Format("The engine type is not suit!"))
        {

        }
    }
}
