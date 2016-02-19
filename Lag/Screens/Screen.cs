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
    /// Class representing a program state, e.g. the main menu, game screen or options screen.
    /// </summary>
    abstract class Screen
    {
        protected ScreenManager manager;

        public Screen(ScreenManager manager)
        {
            this.manager = manager;
        }

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        protected void GoToScreen(Screen screen)
        {
            manager.GoTo(screen);
        }
    }
}
