using SpaceInvaders.Engine;

namespace SpaceInvaders.Graphics
{
    public interface IRenderer
    {
        int Width { get; }
        int Height { get; }

        void SetPixel(int x, int y, uint colour);
        void Clear();
        void Present();
        void DrawSprite(Sprite sprite, int x, int y);
    }
}
