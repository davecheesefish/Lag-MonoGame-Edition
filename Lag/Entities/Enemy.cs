using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lag.Entities
{
    class Enemy:Entity
    {
        public bool IsDead { get { return isDead; } }
        private bool isDead = false;

        private Texture2D texture;

        public Enemy(Vector2 position, Vector2 velocity)
            : base(position, velocity)
        {
            radius = 15;
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("enemy");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(radius), 1.0f, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Kill this enemy so it will be removed from the level.
        /// </summary>
        public void Kill()
        {
            isDead = true;
        }
    }
}
