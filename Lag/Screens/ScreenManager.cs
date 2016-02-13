using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lag.Screens
{
    /// <summary>
    /// Class responsible for calling Update and Draw on a set of screens and switching between
    /// them.
    /// </summary>
    class ScreenManager
    {
        private Screen activeScreen;
        private ContentManager contentManager;

        public ScreenManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        /// <summary>
        /// Update the active screen.
        /// </summary>
        /// <param name="gameTime">The current gameTime.</param>
        public void Update(GameTime gameTime)
        {
            activeScreen.Update(gameTime);
        }

        /// <summary>
        /// Draw the active screen.
        /// </summary>
        /// <param name="gameTime">The current gameTime.</param>
        /// <param name="spriteBatch">An active SpriteBatch, upon which Begin() has already been called.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            activeScreen.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Transition from the current active screen to the supplied new screen.
        /// </summary>
        /// <param name="screen">The screen to transition to.</param>
        public void GoTo(Screen screen)
        {
            this.activeScreen = screen;
            activeScreen.LoadContent(this.contentManager);
        }
    }
}
