using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        protected List<Wheel> m_Wheels; 

        public abstract void SetFuel();

        public enum eVehicleType
        {
            Car,
            Motorcycle,
            Truck,
        }

        protected Vehicle(string i_Model, string i_LicenseNumber, Engine.eTypeOfEngine i_EngineType)
        {
            this.Model = i_Model;
            this.LicenseNumber = i_LicenseNumber;
            this.m_Wheels = new List<Wheel>();

            if (i_EngineType == Engine.eTypeOfEngine.Fuel)
            {
                EngineType = new Fuel();
            }
            else
            {
                EngineType = new Battery();
            }
        }

        public bool FuelValidationType(Fuel.eFuelType i_type)
        {
            if (((Fuel)EngineType).FuelType == i_type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Wheel> Wheels
        {
            get
            {
                return m_Wheels;
            }

            set
            {
                m_Wheels = value;
            }
        }

        public string Model { get; set; }

        public string LicenseNumber { get; }

        public Engine EngineType { get; set; }

        public override string ToString()
        {
            string vehicleInfo = string.Format("Model = {0}{4}License Number = {1}{4}Energy(Fuel/Battery) left = {2}{4}Number of wheels = {3}{4}",
            Model,
            LicenseNumber,
            EngineType.CurrEnergy,
            m_Wheels.Count,
            Environment.NewLine);

            return vehicleInfo + m_Wheels[0].ToString() + EngineType.ToString();
        }
    }
}
