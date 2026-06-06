using System.Diagnostics;
using System.Windows.Media;

namespace SpaceInvaders.Engine
{
    public sealed class GameLoop
    {
        private readonly Action<float> _update;
        private readonly Action _render;

        private readonly Stopwatch _watch = new();
        private long _last;

        public GameLoop(Action<float> update, Action render)
        {
            _update = update;
            _render = render;

            _watch.Start();
            _last = _watch.ElapsedTicks;

            CompositionTarget.Rendering += OnRender;
        }

        private void OnRender(object? sender, EventArgs e)
        {
            long now = _watch.ElapsedTicks;
            float dt = (float)(now - _last) / Stopwatch.Frequency;
            _last = now;
            _update(dt);
            _render();
        }
    }
}
