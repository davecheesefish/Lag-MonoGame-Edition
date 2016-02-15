using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lag.Entities
{
    /// <summary>
    /// Buddy orbits around the player character and gives a bonus when collecting pickups.
    /// </summary>
    class Buddy:Entity
    {
        /// <summary>
        /// The player this Buddy is orbiting.
        /// </summary>
        private Player player;

        /// <summary>
        /// Timer used for orbit position calculations.
        /// </summary>
        private float orbitTimer = 0;

        private Texture2D texture;
        
        public Buddy(Player player, Vector2 position)
            : base(position, Vector2.Zero)
        {
            this.player = player;
            radius = 7.5f;
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("buddy");
        }

        public override void Update(GameTime gameTime)
        {
            // Increment the orbit timer.
            float distanceFromPlayer = player.Radius + 30;
            
            orbitTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Position Buddy so it is not within the player character.
            position = player.Position + new Vector2(distanceFromPlayer * (float)Math.Cos(orbitTimer * 2.0f), distanceFromPlayer * (float)Math.Sin(orbitTimer * 2.0f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(7.5f, 7.5f), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
