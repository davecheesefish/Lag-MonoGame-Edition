using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lag.Scores;

namespace Lag.Screens
{
    class HighScoreScreen:Screen
    {
        /// <summary>
        /// The y position of the first scoreboard item.
        /// </summary>
        private float boardYPos = 200.0f;

        /// <summary>
        /// Vertical separation of scoreboard items.
        /// </summary>
        private float boardYSep = 30.0f;

        private Texture2D bgTexture;
        private SpriteFont highScoreFont;

        private Scores.HighScoreBoard scoreBoard;
        private List<Scores.HighScore> scoreEntries;

        private string highlightedScoreId;

        public HighScoreScreen(ScreenManager manager, HighScoreBoard scoreBoard, string highlightedScoreId = "")
            : base(manager)
        {
            this.scoreBoard = scoreBoard;
            this.scoreEntries = scoreBoard.GetPage(0, 5);
            this.highlightedScoreId = highlightedScoreId;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            bgTexture = contentManager.Load<Texture2D>("menubackground");
            highScoreFont = contentManager.Load<SpriteFont>(@"fonts\menufont");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                manager.GoTo(new MenuScreen(manager));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the background.
            spriteBatch.Draw(bgTexture, new Vector2(197.0f, 0.0f), Color.White);

            // Draw the "High Scores" title.
            spriteBatch.DrawString(highScoreFont, "High Scores", new Vector2(225.0f, 155.0f), Color.Black);

            // If there are no scores to display, draw a message saying so.
            if (scoreEntries.Count == 0)
            {
                spriteBatch.DrawString(highScoreFont, "No scores yet!", new Vector2(225.0f, 200.0f), Color.White);
            }

            // Draw the high-score entries.
            Color scoreColor;
            HighScore entry;
            for (int i = 0; i < scoreEntries.Count; ++i)
            {
                entry = scoreEntries[i];
                scoreColor = (entry.Id.Equals(highlightedScoreId)) ? Color.White : Color.Black;
                spriteBatch.DrawString(highScoreFont, entry.Name, new Vector2(225.0f, boardYPos + i * boardYSep), scoreColor);
                spriteBatch.DrawString(highScoreFont, entry.Score.ToString(), new Vector2(510.0f, boardYPos + i * boardYSep), scoreColor);
            }
        }
    }
}
