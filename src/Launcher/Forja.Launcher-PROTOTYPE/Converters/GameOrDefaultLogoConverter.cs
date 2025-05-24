namespace Forja.Launcher.Converters;

public class GameOrDefaultLogoConverter : IMultiValueConverter
{
    public static readonly GameOrDefaultLogoConverter Instance = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var logoBitmap = values[0] as Bitmap;
        var defaultLogo = values[1] as Bitmap;

        return logoBitmap ?? defaultLogo;
    }
}