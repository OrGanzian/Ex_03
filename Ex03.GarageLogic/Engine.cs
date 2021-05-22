namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        public void AddEnergy(float i_EnergyToAdd)
        {
            if (CurrEnergy + i_EnergyToAdd > CurrCapacity || CurrEnergy + i_EnergyToAdd < 0)
            {
                throw new ValueOutOfRangeException(0, CurrCapacity);
            }
            else
            {
                CurrEnergy = CurrEnergy + i_EnergyToAdd;
            }
        }

        public float CurrCapacity { get; set; }

        public float CurrEnergy { get; set; }

        public enum eTypeOfEngine
        {
            Fuel,
            Battery,
        }
    }
}
