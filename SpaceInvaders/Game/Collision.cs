using SpaceInvaders.Entities;

namespace SpaceInvaders.Game
{
    public static class Collision
    {
        public static bool Intersects(Entity a, Entity b)
        {
            return a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y;
        }

        public static bool IntersectsRect(float ax, float ay, int aw, int ah, float bx, float by, int bw, int bh)
        {
            return ax < bx + bw && ax + aw > bx && ay < by + bh && ay + ah > by;
        }
    }
}
