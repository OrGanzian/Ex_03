using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private Dictionary<string, VehicleInGarage> m_CarsList;

        public Garage()
        {
            this.m_CarsList = new Dictionary<string, VehicleInGarage>();
        }

        public enum eStatus
        {
            InRepairs = 1,
            Repaired,
            Paid,
        }
        public Dictionary<string, VehicleInGarage> CarsInTheGarage
        {
            get { return this.m_CarsList; }
        }


        public eStatus CarStatus
        { get; private set; }


        public void AddAirPressure(string i_LicenseNumber)
        {
            VehicleInGarage vehicleToReapair = IsExistsInGarage(i_LicenseNumber);
            if (vehicleToReapair == null)
            {
                throw new MissingVehicleException(i_LicenseNumber);
            }
            else
            {
                foreach (Wheel wheels in vehicleToReapair.Vehicle.Wheels)
                {
                    wheels.FillToMaximum();
                }
            }
        }
        public void ChangeVehicleStatus(string i_LicenseNumber, eStatus i_CarStatus)
        {
            VehicleInGarage vehicleAlreadyInside = IsExistsInGarage(i_LicenseNumber);

            if (vehicleAlreadyInside == null)
            {
                throw new MissingVehicleException(i_LicenseNumber);
            }

            vehicleAlreadyInside.CurrentStatus = i_CarStatus;
        }

        public VehicleInGarage IsExistsInGarage(string i_LicenseNumber)
        {
            bool IsExistsInGarage = CarsInTheGarage.TryGetValue(i_LicenseNumber, out VehicleInGarage VehicleInGarage);
            if (IsExistsInGarage == false)
            {
                VehicleInGarage = null;
            }

            return VehicleInGarage;
        }


        public List<string> GetListOfAllLicensesNumber()
        {
            List<string> licenseListToReturn = new List<string>();

            foreach (string licenseNumberToAdd in CarsInTheGarage.Keys)
            {
                licenseListToReturn.Add(licenseNumberToAdd);
            }

            return licenseListToReturn;
        }

        public List<string> GetLicensesListByStatus(eStatus i_Status)
        {
            List<string> licenseListByStatusToReturn = new List<string>();

            foreach (VehicleInGarage licenseNumber in CarsInTheGarage.Values)
            {
                if (licenseNumber.CurrentStatus == i_Status)
                    licenseListByStatusToReturn.Add(licenseNumber.Vehicle.LicenseNumber);
            }

            return licenseListByStatusToReturn;
        }


        public void AddFuel(string i_VehicleLicenseNumber, Fuel.eFuelType i_FuelType, float i_FuelAmountToAdd)
        {
            VehicleInGarage vehicleInGarage = IsExistsInGarage(i_VehicleLicenseNumber);
            if (vehicleInGarage == null)
            {
                throw new MissingVehicleException(i_VehicleLicenseNumber);
            }
            else if (vehicleInGarage.Vehicle.EngineType is Battery)
            {
                throw new EngineTypeException();
            }
            else if (!vehicleInGarage.Vehicle.FuelValidationType(i_FuelType))
            {
                throw new FuelTypeException();
            }

            vehicleInGarage.Vehicle.EngineType.AddEnergy(i_FuelAmountToAdd);

        }
        public void AddNewVehicleToGarage(Vehicle i_NewVehicleToAdd, string i_ClientName, string i_ClientPhone)
        {
            VehicleInGarage newVehicleToAdd = new VehicleInGarage(i_ClientName, i_ClientPhone, i_NewVehicleToAdd);
            CarsInTheGarage.Add(i_NewVehicleToAdd.LicenseNumber, newVehicleToAdd);
        }
        public bool InsertVehicleToGarage(Vehicle i_Vehicle, string i_OwnersName, string i_OwnerPhone)
        {
            bool vehicleAlreadyInside = CarsInTheGarage.ContainsKey(i_Vehicle.LicenseNumber);

            if (vehicleAlreadyInside == true)
            {
                vehicleAlreadyInside = true;
                ChangeVehicleStatus(i_Vehicle.LicenseNumber, eStatus.InRepairs);

            }
            else
            {
                CarsInTheGarage.Add(i_Vehicle.LicenseNumber, new VehicleInGarage(i_OwnersName, i_OwnerPhone, i_Vehicle));
            }

            return vehicleAlreadyInside;
        }



        public string GetVehicleDetails(string i_LicenseNumber)
        {
            VehicleInGarage DemandVehicle = IsExistsInGarage(i_LicenseNumber);
            if (DemandVehicle == null)
            {
                throw new MissingVehicleException(i_LicenseNumber);
            }

            return DemandVehicle.ToString();
        }
        public void ChargeVehicle(string i_LicenseNumber, float i_TimeToCharge)
        {
            VehicleInGarage vehicleInGarage = IsExistsInGarage(i_LicenseNumber);

            if (vehicleInGarage == null)
            {
                throw new MissingVehicleException(i_LicenseNumber);
            }

            if (vehicleInGarage.Vehicle.EngineType is Fuel)
            {
                throw new EngineTypeException();
            }
            else
            {
                (vehicleInGarage?.Vehicle.EngineType)?.AddEnergy(i_TimeToCharge);
            }
        }
       
    }
}