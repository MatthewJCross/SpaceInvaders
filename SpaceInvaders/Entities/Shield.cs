namespace SpaceInvaders.Entities
{
    public class Shield
    {
        public int X;
        public int Y;

        public const int Width = 24;
        public const int Height = 12;

        public bool[,] Pixels = new bool[Width, Height];

        public Shield(int x, int y)
        {
            X = x;
            Y = y;

            for (int py = 0; py < Shield.Height; py++)
            {
                for (int px = 0; px < Shield.Width; px++)
                {
                    Pixels[px, py] = true;
                }
            }

            for (int py = 8; py < Shield.Height; py++)
            {
                for (int px = 9; px < 15; px++)
                {
                    Pixels[px, py] = false;
                }
            }

            for (int py = 0; py < 4; py++)
            {
                for (int px = 0; px < 4 - py; px++)
                {
                    Pixels[px, py] = false;
                }
            }

            for (int py = 0; py < 4; py++)
            {
                for (int px = Width - (4 - py); px < Width; px++)
                {
                    Pixels[px, py] = false;
                }
            }
        }
    }
}
