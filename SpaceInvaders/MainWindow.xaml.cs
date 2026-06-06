using System.Windows;
using SpaceInvaders.Audio;
using SpaceInvaders.Engine;
using SpaceInvaders.Game;
using SpaceInvaders.Graphics;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameWorld _world;
        private readonly GameLoop _loop;

        public MainWindow()
        {
            InitializeComponent();

            var audio = new AudioEngine();

            audio.Load(AudioKeys.Shoot, "Assets/shoot.wav");
            audio.Load(AudioKeys.Explosion, "Assets/explosion.wav");
            audio.Load(AudioKeys.Ufo, "Assets/ufo.wav");
            audio.Load(AudioKeys.InvaderKilled, "Assets/invaderkilled.wav");
            audio.Load(AudioKeys.March1, "Assets/march1.wav");
            audio.Load(AudioKeys.March2, "Assets/march2.wav");
            audio.Load(AudioKeys.March3, "Assets/march3.wav");
            audio.Load(AudioKeys.March4, "Assets/march4.wav");
            audio.Load(AudioKeys.ExtraLife, "Assets/extralife.wav");

            var renderer = new WriteableBitmapRenderer(320, 240);
            GameHost.Children.Add(renderer.Display);

            _world = new GameWorld(renderer, audio);
            _world.OnHud = UpdateHud;

            _loop = new GameLoop(_world.Update, _world.Render);

            KeyDown += _world.Input.KeyDown;
            KeyUp += _world.Input.KeyUp;
        }

        private void UpdateHud(int score, int highScore, int wave, int lives)
        {
            ScoreText.Text = $"{score:D6}";
            HighScoreText.Text = $"{highScore:D6}";
            LivesText.Text = $"{lives}";
            WavesText.Text = $"{wave}";

            if (_world.IsGameOver)
            {
                GameOverPanel.Visibility = _world.ShowGameOverText ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                GameOverPanel.Visibility = Visibility.Collapsed; 
            }
        }
    }
}