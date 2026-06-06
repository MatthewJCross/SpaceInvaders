using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entities
{
    public abstract class Entity
    {
        public float X;
        public float Y;

        public int Width;
        public int Height;

        public bool Alive = true;

        public abstract void Update(float dt);
        public abstract void Render(IRenderer renderer);
}
}
