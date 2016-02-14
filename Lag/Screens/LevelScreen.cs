using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lag.Entities;

namespace Lag.Screens
{
    /// <summary>
    /// Class representing the main gameplay screen.
    /// </summary>
    class LevelScreen:Screen
    {
        /// <summary>
        /// Saved reference to the content manager for loading and retrieving entity content.
        /// </summary>
        private ContentManager contentManager;

        private Player player;

        public LevelScreen()
            : base()
        {
            player = new Player(new Vector2(400, 300));
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            player.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(gameTime, spriteBatch);
        }
    }
}
