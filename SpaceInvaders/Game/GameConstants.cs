namespace SpaceInvaders.Game
{
    public static class GameConstants
    {
        public const int ScreenW = 320;
        public const int ScreenH = 240;

        public const float PlayerSpeed = 20f;

        public const float BaseEnemySpeed = 10f;
        public const float EnemySpeedPerWave = 5f;

        public const float BulletSpeed = 300f;

        public const float BaseEnemyShootChance = 0.002f;
        public const float EnemyShootChancePerWave = 0.0015f;

        public const float UfoIntervalSeconds = 10f;

        public const int MaxEnemies = 55;
        public const float KillSpeedMultiplier = 2.8f;

        public const int StartLives = 3;
        public const int MaxLives = 5;
        public const int ExtraLifeScore = 1500;
    }
}
