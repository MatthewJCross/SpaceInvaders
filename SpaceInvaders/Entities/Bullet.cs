using SpaceInvaders.Engine;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entities
{
    public class Bullet : Entity
    {
        public Sprite Sprite { get; }
        public int Vy;

        public Bullet(float x, float y, int width, int height, int vy)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Vy = vy;
            Sprite = SpriteAssets.Bullet;
        }

        public override void Update(float dt)
        {
            Y += Vy * dt;

            if (Y < -10 || Y > 260)
                Alive = false;
        }

        public override void Render(IRenderer renderer)
        {
            DrawPrimitives.DrawSprite(renderer, Sprite, (int)X, (int)Y);
         }
    }
}
