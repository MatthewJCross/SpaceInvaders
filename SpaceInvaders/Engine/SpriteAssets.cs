using SpaceInvaders.Graphics;

namespace SpaceInvaders.Engine
{
    public static class SpriteAssets
    {
        public static readonly Sprite Invader1a = CreateInvader1a();
        public static readonly Sprite Invader1b = CreateInvader1b();
        public static readonly Sprite Invader2a = CreateInvader2a();
        public static readonly Sprite Invader2b = CreateInvader2b();
        public static readonly Sprite Invader3a = CreateInvader3a();
        public static readonly Sprite Invader3b = CreateInvader3b();
        public static readonly Sprite InvaderShot = CreateInvaderShot();
        public static readonly Sprite Player = CreatePlayer();
        public static readonly Sprite[] PlayerExplosion =
        {
            CreatePlayerExplosion1(),
            CreatePlayerExplosion2()
        }; 
        public static readonly Sprite Ufo = CreateUfo();
        public static readonly Sprite Bullet = CreateBullet();

        private static Sprite CreateInvader1a()
        {
            string[] data =
            {
                ".....XX.....",
                "....XXXX....",
                "...XXXXXX...",
                "..XX.XX.XX..",
                "..XXXXXXXX..",
                "....X..X....",
                "...X.XX.X...",
                "..X.X..X.X.."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvader1b()
        {
            string[] data =
            {
                ".....XX.....",
                "....XXXX....",
                "...XXXXXX...",
                "..XX.XX.XX..",
                "..XXXXXXXX..",
                "..X..XX..X..",
                ".X........X.",
                "..X......X.."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvader2a()
        {
            string[] data =
            {
                "..X......X..",
                "X..X....X..X",
                "X.XXXXXXXX.X",
                "XXX.XXXX.XXX",
                "XXXXXXXXXXXX",
                ".XXXXXXXXXX.",
                "..X......X..",
                ".X........X."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvader2b()
        {
            string[] data =
            {
                "..X......X..",
                "...X....X...",
                "..XXXXXXXX..",
                "XXX.XXXX.XXX",
                "XXXXXXXXXXXX",
                "X.XXXXXXXX.X",
                "X.X......X.X",
                "...XX..XX..."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvader3a()
        {
            string[] data =
            {
                "....XXXX....",
                ".XXXXXXXXXX.",
                "XXXXXXXXXXXX",
                "XXX..XX..XXX",
                "XXXXXXXXXXXX",
                "...XX..XX...",
                "..XX.XX.XX..",
                "XX........XX"
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvader3b()
        {
            string[] data =
            {
                "....XXXX....",
                ".XXXXXXXXXX.",
                "XXXXXXXXXXXX",
                "XXX..XX..XXX",
                "XXXXXXXXXXXX",
                "..XXX..XXX..",
                ".XX......XX.",
                "..XX....XX.."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreateInvaderShot()
        {
            string[] data =
            {
                ".X..X.X..X.",
                "..X.X.X.X..",
                "...X...X...",
                "XX.......XX",
                "...X...X...",
                "..X.X.X.X..",
                ".X..X.X..X."
            };

            return CreateFromStrings(data, Colours.White);
        }

        private static Sprite CreatePlayer()
        {
            string[] data =
            {
                ".......XX.......",
                "......XXXX......",
                "......XXXX......",
                ".XXXXXXXXXXXXXX.",
                ".XXXXXXXXXXXXXX.",
                "XXXXXXXXXXXXXXXX",
                "XXXXXXXXXXXXXXXX"
            };

            return CreateFromStrings(data, Colours.Green);
        }

        private static Sprite CreatePlayerExplosion1()
        {
            string[] data =
            {
                "......X.........",
                "...........X....",
                "...X..X.X.X.....",
                ".......XX.XX....",
                ".X...X.XX.X.X...",
                "..XXXXXXXXXXXX..",
                "XXXXXXXXXXXXXXXX"
            };

            return CreateFromStrings(data, Colours.Green);
        }

        private static Sprite CreatePlayerExplosion2()
        {
            string[] data =
            {
                ".........X......",
                "....X...........",
                ".....X.X.X..X...",
                "....XX.XX.......",
                "...X.X.XX.X...X.",
                "..XXXXXXXXXXXX..",
                "XXXXXXXXXXXXXXXX"
            };

            return CreateFromStrings(data, Colours.Green);
        }

        private static Sprite CreateUfo()
        {
            string[] data =
            {
                ".........XXXXXX.........",
                ".......XXXXXXXXXX.......",
                "...XXXXXXXXXXXXXXXXXX...",
                ".XXX..XXX..XX..XXX..XXX.",
                "XXXX..XXX..XX..XXX..XXXX",
                ".XXXXXXXXXXXXXXXXXXXXXX.",
                "...XXXX...XXXX...XXXX...",
                "....XX.....XX.....XX...."
            };

            return CreateFromStrings(data, Colours.Red);
        }

        private static Sprite CreateBullet()
        {
            string[] data =
            {
                ".X",
                "X.",
                ".X",
                "X.",
                ".X",
                "X.",
                ".X",
                "X."
            };

            return CreateFromStrings(data, Colours.Yellow);
        }

        private static Sprite CreateFromStrings(string[] rows, uint colour)
        {
            int width = rows[0].Length;
            int height = rows.Length;

            uint[] pixels = new uint[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = rows[y][x] == 'X' ? colour : 0;
                }
            }

            return new Sprite(width, height, pixels);
        }
    }
}
