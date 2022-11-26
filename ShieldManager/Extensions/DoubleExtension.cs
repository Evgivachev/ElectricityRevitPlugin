namespace ShieldManager.Extensions
{
    public static class DoubleExtension
    {
        public static double FootToMillimeters(this double n)
        {
            return n * 0.3048 * 1000;
        }

        public static double ConvertVolts(this double n)
        {
            return n * 0.3048 * 0.3048;
        }
    }
}
