using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf
{
    internal static class BitmapSources
    {
        public static BitmapSource BackgroundImage { get; } = Properties.Resources.SentryPattern.ToBitmapImage().ReplaceTransparency(Color.FromRgb(255, 255, 255));
        public static BitmapSource FlatLogoLeft { get; } = Properties.Resources.FlagLogoLeft.ToBitmapImage();
        public static BitmapSource SentryLogoWhiteSmall { get; } = Properties.Resources.SentryLogoWhiteSmall.ToBitmapImage();
    }
}
