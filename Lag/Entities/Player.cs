using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lag.Entities
{
    class Player
    {
        public Vector2 Position { get { return position; } }
        private Vector2 position;

        private Texture2D texture;

        public Player(Vector2 position)
        {
            this.position = position;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
        }

        internal void Update(GameTime gameTime)
        {
            
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(60.0f, 60.0f), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
