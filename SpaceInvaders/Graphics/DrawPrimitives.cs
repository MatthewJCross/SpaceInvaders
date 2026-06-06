using SpaceInvaders.Engine;

namespace SpaceInvaders.Graphics
{
    public static class DrawPrimitives
    {
        public static void Line(IRenderer renderer, int x0, int y0, int x1, int y1, uint colour)
        {
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;

            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;

            int err = dx + dy;

            while (true)
            {
                renderer.SetPixel(x0, y0, colour);

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = 2 * err;

                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public static void Rect(IRenderer renderer, int x, int y, int width, int height, uint colour)
        {
            Line(renderer, x, y, x + width - 1, y, colour);
            Line(renderer, x, y + height - 1, x + width - 1, y + height - 1, colour);
            Line(renderer, x, y, x, y + height - 1, colour);
            Line(renderer, x + width - 1, y, x + width - 1, y + height - 1, colour);
        }

        public static void FillRect(IRenderer renderer, int x, int y, int width, int height, uint colour)
        {
            for (int py = 0; py < height; py++)
            {
                for (int px = 0; px < width; px++)
                {
                    renderer.SetPixel(x + px, y + py, colour);
                }
            }
        }

        public static void Circle(IRenderer renderer, int centreX, int centreY, int radius, uint colour)
        {
            int x = radius;
            int y = 0;
            int err = 0;

            while (x >= y)
            {
                renderer.SetPixel(centreX + x, centreY + y, colour);
                renderer.SetPixel(centreX + y, centreY + x, colour);
                renderer.SetPixel(centreX - y, centreY + x, colour);
                renderer.SetPixel(centreX - x, centreY + y, colour);
                renderer.SetPixel(centreX - x, centreY - y, colour);
                renderer.SetPixel(centreX - y, centreY - x, colour);
                renderer.SetPixel(centreX + y, centreY - x, colour);
                renderer.SetPixel(centreX + x, centreY - y, colour);

                y++;

                if (err <= 0)
                {
                    err += (2 * y) + 1;
                }

                if (err > 0)
                {
                    x--;
                    err -= (2 * x) + 1;
                }
            }
        }

        public static void FillCircle(IRenderer renderer, int centreX, int centreY, int radius, uint colour)
        {
            for (int y = -radius; y <= radius; y++)
            {
                int span = (int)Math.Sqrt(radius * radius - y * y);

                for (int x = -span; x <= span; x++)
                {
                    renderer.SetPixel(centreX + x, centreY + y, colour);
                }
            }
        }

        public static void Triangle(IRenderer renderer, int x1, int y1, int x2, int y2, int x3, int y3, uint colour)
        {
            Line(renderer, x1, y1, x2, y2, colour);
            Line(renderer, x2, y2, x3, y3, colour);
            Line(renderer, x3, y3, x1, y1, colour);
        }

        public static void Cross(IRenderer renderer, int x, int y, int size, uint colour)
        {
            Line(renderer, x - size, y, x + size, y, colour);
            Line(renderer, x, y - size, x, y + size, colour);
        }

        public static void DrawSprite(IRenderer renderer, Sprite sprite, int x, int y)
        {
            if (sprite == null) 
                return;
            
            for (int sy = 0; sy < sprite.Height; sy++)
            {
                for (int sx = 0; sx < sprite.Width; sx++)
                {
                    uint colour = sprite.GetPixel(sx, sy);

                    // transparent
                    if ((colour >> 24) == 0)
                        continue;

                    renderer.SetPixel(x + sx, y + sy, colour);
                }
            }
        }
    }
}
