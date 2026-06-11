using System.Windows.Input;
using InputManager = SpaceInvaders.Engine.InputManager;
using SpaceInvaders.Audio;
using SpaceInvaders.Engine;
using SpaceInvaders.Entities;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Game
{
    public enum GameState
    {
        Playing,
        GameOver
    }

    public class GameWorld
    {
        private readonly Random _rng = new();
        private readonly IRenderer _r;
        public InputManager Input { get; } = new();

        public Action<int, int, int, int>? OnHud;
        
        private Player _player;
        private List<Enemy> _enemies = new();
        private readonly List<Bullet> _playerBullets = new();
        private readonly List<Bullet> _enemyBullets = new();

        private float _enemyDir = 1;
        private float _animationTimer;
        private bool _animationFrame; 
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public int Lives { get; private set; } = GameConstants.StartLives;
        private int _nextExtraLifeScore = GameConstants.ExtraLifeScore;
        public int Wave { get; private set; } = 1;

        private float _ufoTimer;
        private Enemy? _ufo;
        private bool _ufoActive;
        private SoundEffectInstance _ufoLoop;
        private static readonly int[] UfoScores = new[] { 100, 150, 200, 300 };

        private float _enemyShootAccumulator;

        private List<Shield> _shields = new();

        private GameState _state = GameState.Playing;
        public bool IsGameOver => _state == GameState.GameOver;
        private float _gameOverFlashTimer;
        public bool ShowGameOverText => ((int)(_gameOverFlashTimer * 2)) % 2 == 0;

        private readonly AudioEngine _audio;
        private int _marchStep;
        private float _enemyMoveTimer;

        private float _deathPauseTimer;

        public GameWorld(IRenderer renderer, AudioEngine audio)
        {
            _audio = audio;
            
            _r = renderer;
            HighScore = HighScoreManager.Load();

            _player = new Player(160, 220, 16, 8, Input);

            CreateWave();
            CreateShields();
        }

        private void Restart()
        {
            Score = 0;
            Lives = GameConstants.StartLives;
            Wave = 1;
            _gameOverFlashTimer = 0;

            _state = GameState.Playing;

            _playerBullets.Clear();
            _enemyBullets.Clear();
            _enemies.Clear();

            CreateWave();
            CreateShields();
        }

        private void CheckExtraLife()
        {
            if (Score >= _nextExtraLifeScore)
            {
                if (Lives < GameConstants.MaxLives)
                {
                    Lives++;
                    _audio.Play(AudioKeys.ExtraLife);
                }
                _nextExtraLifeScore += GameConstants.ExtraLifeScore;
            }
        }

        private void CreateWave()
        {
            _enemies.Clear();

            int rows = 5;
            int cols = 11;
            int startX = 30;
            int startY = 20;
            int spacingX = 20;
            int spacingY = 16;
            Sprite sprA, sprB;
            int points = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (y < 1)
                    {
                        sprA = SpriteAssets.Invader1a;
                        sprB = SpriteAssets.Invader1b;
                        points = 30;
                    }
                    else if (y < 3)
                    {
                        sprA = SpriteAssets.Invader2a;
                        sprB = SpriteAssets.Invader2b;
                        points = 20;
                    }
                    else 
                    {
                        sprA = SpriteAssets.Invader3a;
                        sprB = SpriteAssets.Invader3b;
                        points = 10;
                    }
                    _enemies.Add(new Enemy(startX + x * spacingX, startY + y * spacingY, 12, 8, sprA, sprB, points));
                }
            }

            // Wave difficulty scaling
            _enemyDir = 1 + (Wave * 0.1f);
        }

        private void CheckWaveComplete()
        {
            if (_enemies.Count == 0)
            {
                Wave++;
                CreateWave();
                CreateShields();
            }
        }

        public void Update(float dt)
        {
            if (_deathPauseTimer > 0)
            {
                _deathPauseTimer -= dt;
                return;
            }

            if (_state == GameState.GameOver)
            {
                _gameOverFlashTimer += dt;

                if (Input.IsDown(Key.Enter))
                    Restart();

                return;
            }

            UpdatePlayer(dt);
            UpdatePlayerBullets(dt);

            UpdateEnemies(dt);
            UpdateEnemyShooting(dt);
            UpdateEnemyBullets(dt);

            UpdateUfo(dt);

            HandleCollisions();

            CheckLoseCondition();
            
            CheckWaveComplete();

            CheckExtraLife();
                
            _audio.Update();
        }

        private void UpdatePlayer(float dt)
        {
            _player.Update(dt);
            if (_player.WantsToFire && !HasActivePlayerBullet())
            {
                _playerBullets.Add(new Bullet(_player.X + _player.Width / 2, _player.Y, 2, 6, -300));
                _audio.Play(AudioKeys.Shoot);
            }

            if (_player.DeathAnimationFinished)
            {
                _player.Live();
                Lives--;
                if (Lives <= 0)
                {
                    _state = GameState.GameOver;
                    if (_ufoLoop is not null)
                        _ufoLoop.Stop();
                }

                ClearEnemyBullets();
                StartDeathPause();
            }
        }

        private void UpdatePlayerBullets(float dt)
        {
            _playerBullets.RemoveAll(b => !b.Alive);

            foreach (var b in _playerBullets)
                b.Update(dt);
        }

        private bool HasActivePlayerBullet()
        {
            return _playerBullets.Any(b => b.Alive);
        }

        private void UpdateEnemies(float dt)
        {
            _enemyMoveTimer += dt;
            int alive = _enemies.Count;
            float ratio = (float)alive / GameConstants.MaxEnemies;
            float moveInterval = 0.05f + ((0.6f - 0.05f) * ratio); 

            bool bounce = false;

            if (_enemyMoveTimer >= moveInterval)
            {
                _enemyMoveTimer = 0;
                _animationFrame = !_animationFrame;

                foreach (var e in _enemies)
                {
                    e.Update(dt);
                    e.X += _enemyDir * 4;
                    e.AnimationFrame = _animationFrame;

                    if (e.X < 0 || e.X > 300)
                        bounce = true;
                }

                PlayMarchSound();

                if (bounce)
                {
                    _enemyDir *= -1;
                    foreach (var e in _enemies)
                        e.Y += 10;
                }
            }
        }

        private void UpdateEnemyBullets(float dt)
        {
            _enemyBullets.RemoveAll(e => !e.Alive);

            foreach (var b in _enemyBullets)
                b.Update(dt);
        }

        private void HandleCollisions()
        {
            HandlePlayerBulletCollisions();
            HandleEnemyBulletCollisions();
            HandleShieldCollisions();
        }

        private void HandlePlayerBulletCollisions()
        {
            _enemies.RemoveAll(e => !e.Alive && e.DieFrames == 0);

            foreach (var bullet in _playerBullets)
            {
                if (!bullet.Alive)
                    continue;

                foreach (var e in _enemies)
                {
                    if (!e.Alive)
                        continue;

                    if (Collision.Intersects(bullet, e))
                    {
                        e.SpriteA = SpriteAssets.InvaderShot;
                        e.SpriteB = SpriteAssets.InvaderShot;
                        _audio.Play(AudioKeys.InvaderKilled);
                        bullet.Alive = false;
                        e.Alive = false;
                        e.DieFrames = 1; 
                        AddScore(e.Points);
                        break;
                    }
                }

                if (_ufoActive && _ufo != null && bullet.Alive )
                {
                    if (Collision.Intersects(bullet, _ufo))
                    {
                        bullet.Alive = false;
                        _ufoActive = false;
                        _ufo = null;
                        _ufoLoop.Stop();
                        AddScore(GetUfoScore());
                    }
                }
            }

            _playerBullets.RemoveAll(b => !b.Alive);
        }

        private int GetUfoScore()
        {
            int index = _rng.Next(UfoScores.Length);
            return UfoScores[index];
        }

        private void HandleEnemyBulletCollisions()
        {
            foreach (var bullet in _enemyBullets)
            {
                if (!bullet.Alive)
                    continue;

                if (Collision.Intersects(bullet, _player))
                {
                    bullet.Alive = false;
                    if (!_player.IsDying)
                    {
                        _player.Die();
                        _audio.Play(AudioKeys.Explosion);
                    }

                    if (Lives <= 0)
                    {
                        _state = GameState.GameOver;
                        _ufoLoop.Stop();
                    }
                }
            }
        }

        private void HandleShieldCollisions()
        {
            foreach (var bullet in _playerBullets)
            {
                if (!bullet.Alive)
                    continue;

                foreach (var shield in _shields)
                {
                    if (shield.Hit(bullet.CollisionRect))
                    {
                        bullet.Alive = false;
                    }
                }
            }

            foreach (var bullet in _enemyBullets)
            {
                foreach (var shield in _shields)
                {
                    int localX = (int)(bullet.X - shield.X);
                    int localY = (int)(bullet.Y - shield.Y);

                    if (localX >= 0 && localX < Shield.Width && localY >= 0 && localY < Shield.Height && shield.Pixels[localX, localY])
                    { 
                        DamageShield(shield, localX, localY);
                        bullet.Alive = false;
                        break;
                    }
                }
            }
        }

        private void CheckLoseCondition()
        {
            foreach (var e in _enemies)
            {
                if (e.Y + e.Height >= _player.Y)
                {
                    Lives--;
                    if (Lives <= 0)
                    {
                        _ufoLoop.Stop();
                        _state = GameState.GameOver;
                    }
                    else
                        ResetAfterHit();

                    break;
                }
            }
        }

        private void ResetAfterHit()
        {
            _playerBullets.Clear();
            _enemyBullets.Clear();

            CreateWave();

            _player.X = 160;
            _player.Y = 220;
        }
        
        private void AddScore(int v)
        {
            Score += v;

            if (Score > HighScore)
            {
                HighScore = Score;
                HighScoreManager.Save(HighScore);
            }
        }

        public void Render()
        {
            _r.Clear();

            if (_state == GameState.Playing)
                Draw();

            OnHud?.Invoke(Score, HighScore, Wave, Lives);

            _r.Present();
        }

        private void Draw()
        {
            DrawEnemies();
            DrawPlayerBullets();
            DrawEnemyBullets();
            DrawPlayerBullets();
            DrawPlayer();
            DrawShields();
            DrawUfo();
        }

        private void DrawEnemies()
        {
            foreach (var e in _enemies)
                e.Render(_r);
        }

        void DrawPlayerBullets()
        {
            foreach (var b in _playerBullets)
                b.Render(_r);
        }

        private void DrawEnemyBullets()
        {
            foreach (var b in _enemyBullets)
                b.Render(_r);
        }

        private void DrawPlayer()
        {
            _player.Render(_r);
        }

        private void DrawShields()
        {
            foreach (var shield in _shields)
            {
                for (int y = 0; y < Shield.Height; y++)
                {
                    for (int x = 0; x < Shield.Width; x++)
                    {
                        if (shield.Pixels[x, y])
                        {
                            _r.SetPixel(shield.X + x, shield.Y + y, Colours.Cyan);
                        }
                    }
                }
            }
        }

        private void DrawUfo()
        {
            if (_ufoActive && _ufo != null)
                  _ufo.Render(_r);
        }

        private void UpdateUfo(float dt)
        {
            _ufoTimer += dt;

            if (_ufoActive)
            {
                if (_ufo != null)
                {
                    _ufo.X += 60 * dt;

                    if (_ufo.X > 340)
                    {
                        _ufoActive = false;
                        _ufo = null;
                        _ufoTimer = 0;
                        _ufoLoop.Stop();
                    }
                }

                return;
            }

            // spawn check
            if (_ufoTimer >= GameConstants.UfoIntervalSeconds)
            {
                SpawnUfo();
                _ufoTimer = 0;
            }
        }

        private void SpawnUfo()
        {
            _ufoActive = true;
            _ufo = new Enemy(-20, 15, 14, 8, SpriteAssets.Ufo, SpriteAssets.Ufo, 150);
            _ufoLoop  = _audio.Play(AudioKeys.Ufo, loop: true);
        }

        private void UpdateEnemyShooting(float dt)
        {
            _enemyShootAccumulator += dt;

            float chance = GameConstants.BaseEnemyShootChance + (Wave * GameConstants.EnemyShootChancePerWave);

            _enemyShootAccumulator += dt;

            if (_enemyShootAccumulator > 0.1f)
            {
                _enemyShootAccumulator = 0;

                foreach (var e in _enemies)
                {
                    if (Random.Shared.NextDouble() < chance)
                    {
                        _enemyBullets.Add(new Bullet(e.X, e.Y + 8, 2, 6, 120 + (Wave * 10)));
                        break;
                    }
                }
            }
        }

        private void CreateShields()
        {
            _shields.Clear();

            for (int i = 0; i < 4; i++)
            {
                _shields.Add(new Shield(40 + i * 70, 180));
            }
        }

        private void DamageShield(Shield shield, int hitX, int hitY)
        {
            const int radius = 2;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    int sx = hitX + x;
                    int sy = hitY + y;

                    if (sx < 0 || sx >= Shield.Width)
                        continue;

                    if (sy < 0 || sy >= Shield.Height)
                        continue;

                    shield.Pixels[sx, sy] = false;
                }
            }
        }

        private void PlayMarchSound()
        {
            switch (_marchStep)
            {
                case 0:
                    _audio.Play(AudioKeys.March1);
                    break;

                case 1:
                    _audio.Play(AudioKeys.March2);
                    break;

                case 2:
                    _audio.Play(AudioKeys.March3);
                    break;

                case 3:
                    _audio.Play(AudioKeys.March4);
                    break;
            }

            _marchStep = (_marchStep + 1) & 3;
        }

        private void StartDeathPause()
        {
            _deathPauseTimer = 1.0f;   // 1 second freeze
        }

        private void ClearEnemyBullets()
        {
            foreach (var bullet in _enemyBullets)
                bullet.Alive = false;

            _enemyBullets.Clear();
        }
    }
}
