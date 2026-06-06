using SpaceInvaders.Engine;
using SpaceInvaders.Graphics;
using System.Windows;
using System.Windows.Input;
using InputManager = SpaceInvaders.Engine.InputManager;

namespace SpaceInvaders.Entities
{
    public class Player : Entity
    {
        public Sprite Sprite { get; }
        private readonly InputManager _input;
        public float Speed = 100;
        public bool WantsToFire { get; private set; }

        public bool IsDying { get; private set; }

        private float _deathTimer;
        private int _deathFrame;

        public bool DeathAnimationFinished => IsDying && _deathFrame >= SpriteAssets.PlayerExplosion.Length;
        
        public Player(float x, float y, int width, int height, InputManager input)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _input = input;
            Sprite = SpriteAssets.Player;
        }

        public override void Update(float dt)
        {
            WantsToFire = false;

            if (_input.IsDown(Key.Left))
                X -= Speed * dt;

            if (_input.IsDown(Key.Right))
                X += Speed * dt;

            X = Math.Clamp(X, 0, 304);

            if (_input.IsDown(Key.Space))
            {
                WantsToFire = true;
                _input.KeyUp(null, new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(App.Current.MainWindow), 0, Key.Space));
            }

            if (IsDying)
            {
                _deathTimer += dt;

                if (_deathTimer >= 0.12f)
                {
                    _deathTimer = 0;
                    _deathFrame++;
                }

                return;
            }
        }

        public override void Render(IRenderer renderer)
        {
            if (IsDying)
            {
                int frame = Math.Min(_deathFrame, SpriteAssets.PlayerExplosion.Length - 1);
                DrawPrimitives.DrawSprite(renderer, SpriteAssets.PlayerExplosion[frame], (int)X, (int)Y);
                return;
            }

            DrawPrimitives.DrawSprite(renderer, Sprite, (int)X, (int)Y);
        }

        public void Die()
        {
            IsDying = true;
            _deathTimer = 0;
            _deathFrame = 0;
        }

        public void Live()
        {
            IsDying = false;
        }
    }
}
