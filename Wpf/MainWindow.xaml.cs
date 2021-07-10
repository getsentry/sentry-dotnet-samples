using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Background = new ImageBrush(BitmapSources.BackgroundImage)
            {
                Stretch = Stretch.Uniform,
                TileMode = TileMode.FlipY,
                AlignmentY = AlignmentY.Top,
                ViewportUnits = BrushMappingMode.Absolute,
                Viewport = Rect.Parse("0,0,862,714")
            };

            WindowStyle = WindowStyle.ThreeDBorderWindow;
            Title = "Sentry Toolbox";

            BottomLeftImage.UseLayoutRounding = true;
            BottomLeftImage.Source = BitmapSources.FlatLogoLeft;

            UseLayoutRounding = true;
            var topLogo = BitmapSources.SentryLogoWhiteSmall;
            TopLeftImage.UseLayoutRounding = true;
            TopLeftImage.Source = topLogo;

            MenuBackground.Fill = new SolidColorBrush(Color.FromRgb(52, 44, 62));
        }
    }
}
