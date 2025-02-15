namespace Print.View;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

[ValueConversion(typeof(Enum), typeof(IEnumerable<Tuple<object, object>>))]
public class EnumToCollectionConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        return GetAllValuesAndDescriptions(value.GetType());
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    private static IEnumerable<Tuple<object, object>> GetAllValuesAndDescriptions(Type t)
    {
        if (!t.IsEnum)
            throw new ArgumentException($"{nameof(t)} must be an enum type");

        return Enum.GetValues(t).Cast<Enum>().Select((e) => Tuple.Create((object)e, (object)e.Description())).ToList();
    }
}
