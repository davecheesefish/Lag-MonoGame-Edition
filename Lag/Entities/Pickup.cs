using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lag.Entities
{
    class Pickup:Entity
    {
        public bool IsDead { get { return isDead; } }
        private bool isDead = false;

        Texture2D texture;
        
        public Pickup(Vector2 position, Vector2 velocity)
            : base(position, velocity)
        {
            radius = 13.0f;
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("pickup");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(15, 13), 1.0f, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Kill this pickup so it will be removed from the level.
        /// </summary>
        public void Kill()
        {
            isDead = true;
        }
    }
}
