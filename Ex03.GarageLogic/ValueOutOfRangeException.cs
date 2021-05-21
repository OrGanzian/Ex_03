using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException(float i_MaxValue, float i_MinValue)
            : base(string.Format("Invalid input number range. Should be between {0} and {1}", i_MaxValue, i_MinValue))
        {
            MaxValue = i_MaxValue;
            MinValue = i_MinValue;
        }
        public float MaxValue { get; }

        public float MinValue { get; }
    }
}
