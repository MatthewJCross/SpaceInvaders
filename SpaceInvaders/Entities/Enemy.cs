using SpaceInvaders.Engine;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entities
{
    public class Enemy : Entity
    {
        public Sprite SpriteA { get; set; }
        public Sprite SpriteB { get; set; }

        public bool AnimationFrame;

        public int Points { get; }
        public int DieFrames { get; set; }

        public Enemy(int x, int y, int width, int height, Sprite spriteA, Sprite spriteB, int points)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            SpriteA = spriteA;
            SpriteB = spriteB;

            Points = points;
            DieFrames = 0;
        }

        public override void Update(float dt)
        {
            if (DieFrames > 0)
                DieFrames--;
        }

        public override void Render(IRenderer renderer)
        {
            DrawPrimitives.DrawSprite(renderer, AnimationFrame ? SpriteA : SpriteB, (int)X, (int)Y);
        }
    }
}
