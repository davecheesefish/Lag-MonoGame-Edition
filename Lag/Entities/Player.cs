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
    class Player
    {
        public Vector2 Position { get { return position; } }
        private Vector2 position;

        private Vector2 velocity;

        /// <summary>
        /// How much speed the player character loses per frame.
        /// </summary>
        private float friction = 1.0f;

        /// <summary>
        /// How much speed the player gains when a direction button is pressed.
        /// </summary>
        private float power = 1.6f;

        /// <summary>
        /// The maximum speed the player character is allowed to reach.
        /// </summary>
        private float maxSpeed = 8;

        private Texture2D texture;

        public Player(Vector2 position)
        {
            this.position = position;
            this.velocity = new Vector2(0.0f);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
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
            Vector2 moveInput = power * GetMovementInput();
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(60.0f, 60.0f), 1.0f, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Returns a unit vector in the direction that the movement controls are being pressed in.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetMovementInput()
        {
            Vector2 inputVector = new Vector2(0.0f);
            InputManager input = InputManager.Instance;

            // Check UP input (up arrow / w).
            if (input.IsKeyDown(Keys.Up) || input.IsKeyDown(Keys.W))
            {
                inputVector.Y -= 1.0f;
            }

            // Check DOWN input (down arrow / s).
            if (input.IsKeyDown(Keys.Down) || input.IsKeyDown(Keys.S))
            {
                inputVector.Y += 1.0f;
            }

            // Check LEFT input (left arrow / a).
            if (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.A))
            {
                inputVector.X -= 1.0f;
            }

            // Check LEFT input (right arrow / d).
            if (input.IsKeyDown(Keys.Right) || input.IsKeyDown(Keys.D))
            {
                inputVector.X += 1.0f;
            }

            return inputVector;
        }
    }
}
