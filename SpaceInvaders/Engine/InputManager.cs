using System.Windows.Input;

namespace SpaceInvaders.Engine
{
    public class InputManager
    {
        private readonly HashSet<Key> _keys = new();

        public bool IsDown(Key key)
        {
            return _keys.Contains(key);
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            _keys.Add(e.Key);
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            _keys.Remove(e.Key);
        }
    }
}
