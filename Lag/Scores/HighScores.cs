using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lag.Scores
{
    static class HighScores
    {
        /// <summary>
        /// The high-score board for a normal game.
        /// </summary>
        public static HighScoreBoard Normal;
        public static string NormalFilePath = @"./Scores/normal.json";

        public static void Initialize()
        {
            Normal = new HighScoreBoard();
            Normal.LoadFromFile(NormalFilePath);
        }
    }
}
