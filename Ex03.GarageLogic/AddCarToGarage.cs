using System;

namespace Ex03.GarageLogic
{
    public class AddCarToGarage
    {
        public Vehicle AddVehicle(string i_Model, string i_LicenseNumber, string i_WheelModel, Engine.eTypeOfEngine i_EngineTypeType, Vehicle.eVehicleType i_VehicleType)
        {
            Vehicle newVehicleToAdd = null;
            switch (i_VehicleType)
            {
                case Vehicle.eVehicleType.Car:
                    newVehicleToAdd = new Car(i_Model, i_LicenseNumber, i_EngineTypeType, i_WheelModel);
                    break;
                case Vehicle.eVehicleType.Motorcycle:
                    newVehicleToAdd = new Motorcycle(i_Model, i_LicenseNumber, i_WheelModel, i_EngineTypeType);
                    break;
                case Vehicle.eVehicleType.Truck:
                    newVehicleToAdd = new Truck(i_Model, i_LicenseNumber, i_EngineTypeType, i_WheelModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i_VehicleType), i_VehicleType, null);
            }

            return newVehicleToAdd;
        }
    }
}
