using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lag.Entities
{
    class Player:Entity
    {
        /// <summary>
        /// The radius of the circular player character.
        /// </summary>
        public override float Radius { get { return scale * radius; } }
        
        /// <summary>
        /// Current scale of the player character.
        /// </summary>
        private float scale = 0.4f;

        /// <summary>
        /// Rate at which the character grows, per frame.
        /// </summary>
        private float growthRate = 0.0001f;

        /// <summary>
        /// How much speed the player character loses per frame.
        /// </summary>
        private float friction = 0.5f;

        /// <summary>
        /// How much speed the player gains when a direction button is pressed.
        /// </summary>
        private float power = 0.9f;

        /// <summary>
        /// The maximum speed the player character is allowed to reach.
        /// </summary>
        private float maxSpeed = 6;

        private Texture2D texture;

        public Player(Vector2 position)
            : base(position, new Vector2(0.0f))
        {
            radius = 60.0f;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
        }

        public override void Update(GameTime gameTime)
        {
            ResolveMotion();
            scale += growthRate;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(60.0f, 60.0f), scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Handles player movement in the world.
        /// </summary>
        private void ResolveMotion()
        {
            // Apply friction.
            // Use length squared for vector comparisons - squaring the other value is far faster
            // than square-rooting the velocity.
            float speedSquared = velocity.LengthSquared();
            if (speedSquared > 0) // If the player's moving...
            {
                if (speedSquared > friction * friction) // ...and the speed is more than the friction value...
                {
                    // ...reduce speed by the friction value.
                    velocity -= friction * Vector2.Normalize(velocity);
                }
                else
                {
                    // ...else stop entirely (otherwise the player could start moving back the other way).
                    velocity.X = 0;
                    velocity.Y = 0;
                }
            }

            // Get movement input and apply it to the current velocity.
            Vector2 moveInput = power * InputManager.Instance.GetMovementInput();
            velocity += moveInput;
            if (velocity.LengthSquared() > maxSpeed * maxSpeed)
            {
                // If velocity is too fast, clamp it to the max speed.
                velocity.Normalize();
                velocity *= maxSpeed;
            }

            // Apply position change.
            position += velocity;
        }

        public Buddy SpawnBuddy()
        {
            return new Buddy(this, this.position);
        }
    }
}
