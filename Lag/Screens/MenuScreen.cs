using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lag.Screens
{
    class MenuScreen:Screen
    {
        /// <summary>
        /// The menu option that is currently selected.
        /// </summary>
        private int activeMenuItem = 0;

        /// <summary>
        /// Menu option strings.
        /// </summary>
        private string[] menuItems = { "Play game" };

        /// <summary>
        /// Text colour for the active menu item.
        /// </summary>
        private Color activeMenuColor = Color.White;

        /// <summary>
        /// Text colour for inactive menu items.
        /// </summary>
        private Color inactiveMenuColor = Color.Black;

        /// <summary>
        /// The y position of the first menu item.
        /// </summary>
        private float menuYPos = 260.0f;

        /// <summary>
        /// Vertical separation of menu items.
        /// </summary>
        private float menuYSep = 30.0f;

        private Texture2D titleTexture;
        private Texture2D menuBgTexture;
        private SpriteFont menuItemFont;

        public MenuScreen(ScreenManager manager)
            : base(manager)
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            titleTexture = contentManager.Load<Texture2D>("title");
            menuBgTexture = contentManager.Load<Texture2D>("menubackground");
            menuItemFont = contentManager.Load<SpriteFont>(@"fonts\menufont");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyReleased(Keys.S) || InputManager.Instance.KeyReleased(Keys.Down))
            {
                // If down was pressed, select the next menu item.
                SelectNextMenuItem();
            }
            
            if (InputManager.Instance.KeyReleased(Keys.W) || InputManager.Instance.KeyReleased(Keys.Up))
            {
                // If up is pressed, select the previous menu item.
                SelectPreviousMenuItem();
            }
            
            if (InputManager.Instance.KeyReleased(Keys.Enter))
            {
                // If enter is pressed, perform the chosen menu action.
                ActivateMenuItem(activeMenuItem);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the menu background.
            spriteBatch.Draw(menuBgTexture, new Vector2(197.0f, 0.0f), Color.White);

            // Draw the title.
            spriteBatch.Draw(titleTexture, new Vector2(292.0f, 163.0f), Color.White);

            Color itemColor;
            for (int i = 0; i < menuItems.Length; ++i)
            {
                // Choose appropriate active/inactive menu colour.
                itemColor = (i == activeMenuItem) ? activeMenuColor : inactiveMenuColor;
                
                // Draw menu item.
                spriteBatch.DrawString(menuItemFont, menuItems[i], new Vector2(400.0f - (menuItemFont.MeasureString(menuItems[i]).X / 2.0f), menuYPos + i * menuYSep), itemColor);
            }
        }

        /// <summary>
        /// Selects the next item on the menu. If the last item is already selected, selection
        /// wraps back around to the first item.
        /// </summary>
        private void SelectNextMenuItem()
        {
            ++activeMenuItem;
            if (activeMenuItem == menuItems.Length)
            {
                // Wrap around to the first menu item if the selection goes past the last item.
                activeMenuItem = 0;
            }
        }

        /// <summary>
        /// Selects the previous item on the menu. If the first item is already selected, selection
        /// wraps back around to the last item.
        /// </summary>
        private void SelectPreviousMenuItem()
        {
            --activeMenuItem;
            if (activeMenuItem < 0)
            {
                // Wrap around to the last menu item if the selection goes before the first item.
                activeMenuItem = menuItems.Length - 1;
            }
        }

        /// <summary>
        /// Performs the action associated with a menu item.
        /// </summary>
        /// <param name="optionId">The ID of the menu item.</param>
        private void ActivateMenuItem(int optionId)
        {
            switch (optionId)
            {
                case 0: // Play game
                    manager.GoTo(new LevelScreen(manager));
                    break;
            }
        }
    }
}
