using SpaceInvaders.Engine;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Graphics
{
    public class WriteableBitmapRenderer : IRenderer
    {
        private readonly WriteableBitmap _bitmap;
        private readonly uint[] _pixels;

        public Image Display { get; }

        public int Width { get; }
        public int Height { get; }

        public WriteableBitmapRenderer(int width, int height)
        {
            Width = width;
            Height = height;

            _pixels = new uint[width * height];

            _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            Display = new Image
            {
                Source = _bitmap,
                Stretch = Stretch.Uniform
            };

            RenderOptions.SetBitmapScalingMode(Display, BitmapScalingMode.NearestNeighbor);
        }

        public void Clear()
        {
            Array.Clear(_pixels);
        }

        public void Present()
        {
            _bitmap.WritePixels(new System.Windows.Int32Rect(0, 0, Width, Height), _pixels, Width * 4, 0);
        }

        public void SetPixel(int x, int y, uint colour)
        {
            if (x < 0 || x >= Width)
                return;

            if (y < 0 || y >= Height)
                return;

            _pixels[(y * Width) + x] = colour;
        }

        public void DrawSprite(Sprite sprite, int x, int y)
        {
            for (int sy = 0; sy < sprite.Height; sy++)
            {
                for (int sx = 0; sx < sprite.Width; sx++)
                {
                    uint colour = sprite.GetPixel(sx, sy);

                    if ((colour >> 24) == 0)
                        continue;

                    int px = x + sx;
                    int py = y + sy;

                    if (px < 0 || py < 0 || px >= Width || py >= Height)
                        continue;

                    SetPixel(px, py, colour);
                }
            }
        }
    }
}
