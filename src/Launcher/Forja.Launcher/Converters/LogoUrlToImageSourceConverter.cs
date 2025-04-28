using Avalonia.Platform;
using Avalonia.Media.Imaging;

namespace Forja.Launcher.Converters;

public class LogoUrlToImageSourceConverter : IValueConverter
{
    private static readonly string DefaultImageUri = "avares://Forja.Launcher/Assets/Forja-icon-logo.svg";

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Game game && !string.IsNullOrEmpty(game.LogoUrl))
        {
            return new Bitmap(game.LogoUrl);
        }
        return new Bitmap(DefaultImageUri);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}