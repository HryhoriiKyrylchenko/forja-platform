namespace Forja.Launcher.Converters;

public class RepairButtonStateConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is GameViewModel game)
        {
            return game is { IsInstalled: true, IsRunning: false, IsUpdated: true };
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
