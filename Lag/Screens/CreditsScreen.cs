using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lag.Screens
{
    class CreditsScreen:Screen
    {
        private Texture2D texture;

        public CreditsScreen(ScreenManager manager)
            : base(manager)
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("credits");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyReleased(Keys.Enter))
            {
                // If enter is pressed, go back to the main menu.
                manager.GoTo(new MenuScreen(manager));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the credits texture.
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
        }
    }
}
