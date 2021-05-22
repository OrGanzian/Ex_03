using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public class VehicleInGarage
    {
        public VehicleInGarage(string i_ClicentName, string i_ClientPhone, Vehicle i_Vehicle)
        {
            this.Vehicle = i_Vehicle;
            this.ClicentName = i_ClicentName;
            this.ClientPhone = i_ClientPhone;
            this.CurrentStatus = Garage.eStatus.InRepairs;
        }

        public string ClicentName
        { get; }

        public string ClientPhone
        { get; set; }

        public Vehicle Vehicle
        { get; }

        public Garage.eStatus CurrentStatus
        { get; set; }

        public override string ToString()
        {
            return string.Format("{3}Client Name:{0}{3}Client Phone:{1}{3}Vehicle's Status:{2}{3}{4}", ClicentName, ClientPhone, CurrentStatus, Environment.NewLine, Vehicle.ToString());

        }
    }
}
