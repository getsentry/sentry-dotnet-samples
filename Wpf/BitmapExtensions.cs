using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf
{
    internal static class BitmapExtensions
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static BitmapSource ReplaceTransparency(this BitmapSource bitmap, System.Windows.Media.Color color)
        {
            var rect = new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            var visual = new DrawingVisual();
            var context = visual.RenderOpen();
            context.DrawRectangle(new SolidColorBrush(color), null, rect);
            context.DrawImage(bitmap, rect);
            context.Close();

            var render = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight,
                96, 96, PixelFormats.Pbgra32);
            render.Render(visual);
            return render;
        }
    }
}
