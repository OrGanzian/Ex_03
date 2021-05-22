using System;

namespace Ex03.GarageLogic
{
    public class MissingVehicleException : Exception
    {
        internal string m_LicenseNumber;

        public MissingVehicleException(string i_LicenseNumber) : base(string.Format("[No such vehicle in garage]{0}", Environment.NewLine))
        {
            m_LicenseNumber = i_LicenseNumber;
        }

        public string LicenseNumber
        {
            get
            {
                return m_LicenseNumber;
            }
        }
    }
}