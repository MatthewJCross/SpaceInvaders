using System.IO;

namespace SpaceInvaders.Game
{
    public static class HighScoreManager
    {
        private const string FileName = "Highscore.dat";

        public static int Load()
        {
            if (!File.Exists(FileName))
                return 0;

            int.TryParse(File.ReadAllText(FileName), out int v);
            return v;
        }

        public static void Save(int score)
        {
            File.WriteAllText(FileName, score.ToString());
        }
    }
}
