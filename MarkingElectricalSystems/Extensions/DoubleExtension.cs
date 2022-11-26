namespace MarkingElectricalSystems.Extensions;

public static class DoubleExtension
{
    public static double ToMetersFromFoots(this double d)
    {
        return d * 0.3048;
    }
}