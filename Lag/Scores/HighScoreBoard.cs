using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Lag.Scores
{
    class HighScoreBoard
    {
        private List<HighScore> entries;

        /// <summary>
        /// Returns the number of high score entries.
        /// </summary>
        public int Count { get { return entries.Count; } }

        public HighScoreBoard()
        {
            entries = new List<HighScore>();
        }

        /// <summary>
        /// Reloads this score board from the given file path.
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            string scoresJson = reader.ReadToEnd();
            entries = JsonConvert.DeserializeObject<List<HighScore>>(scoresJson);
            reader.Close();
        }

        /// <summary>
        /// Saves this high-score board to the given file path.
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveToFile(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(JsonConvert.SerializeObject(entries));
            writer.Close();
        }

        /// <summary>
        /// Adds a score to the high score board. Make sure to save the board to file afterwards to keep any changes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <returns>A unique ID string for this score.</returns>
        public string AddScore(string name, int score){
            // Generate a unique ID for this score.
            string scoreId = Guid.NewGuid().ToString();

            // Find the first score lower than this one and insert a new score before it.
            for (int i = 0; i <= entries.Count; ++i)
            {
                if (i == entries.Count || entries[i].Score < score)
                {
                    HighScore newScore = new HighScore();
                    newScore.Id = scoreId;
                    newScore.Name = name;
                    newScore.Score = score;
                    entries.Insert(i, newScore);
                    break;
                }
            }

            return scoreId;
        }

        /// <summary>
        /// Returns the number of pages in the given high score board, with the given number of scores per page.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetPageCount(int pageSize)
        {
            return (int)Math.Ceiling((float)entries.Count / (float)pageSize);
        }

        /// <summary>
        /// Returns a list of scores to display on a page of the high score board.
        /// </summary>
        /// <param name="pageNo">The page number to retrieve (zero-indexed).</param>
        /// <param name="pageSize">The number of scores to a page.</param>
        /// <returns></returns>
        public List<HighScore> GetPage(int pageNo, int pageSize)
        {
            int highestIndexRequested = (pageNo * pageSize) + pageSize;
            if (highestIndexRequested > entries.Count - 1)
            {
                // If there aren't enough scores to fill a page, return as many as possible.
                return entries.GetRange(pageNo * pageSize, entries.Count - (pageNo * pageSize));
            }
            else
            {
                // Else return the full page requested.
                return entries.GetRange(pageNo * pageSize, pageSize);
            }
        }
    }
}
