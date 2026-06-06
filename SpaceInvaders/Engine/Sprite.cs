namespace SpaceInvaders.Engine
{
    public class Sprite
    {
        public int Width { get; }
        public int Height { get; }

        private readonly uint[] _pixels;

        public Sprite(int width, int height, uint[] pixels)
        {
            Width = width;
            Height = height;
            _pixels = pixels;
        }

        public uint GetPixel(int x, int y)
        {
            return _pixels[y * Width + x];
        }
    }
}
