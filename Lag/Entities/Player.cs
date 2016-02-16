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
        /// The maximum number of past positions to store. This is the maximum number of frames
        /// behind the player that Buddy will be able to lag.
        /// </summary>
        private const int MAX_OLD_POSITIONS = 240;

        /// <summary>
        /// The radius of the circular player character.
        /// </summary>
        public override float Radius { get { return scale * radius; } }
        
        /// <summary>
        /// List of positions from previous frames. Used for Buddy's position calculations.
        /// </summary>
        private List<Vector2> pastPositions;

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
            pastPositions = new List<Vector2>();
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
            PushPastPosition(position);

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

        /// <summary>
        /// Push an old position onto the old positions list.
        /// </summary>
        /// <param name="position">The position to add.</param>
        private void PushPastPosition(Vector2 position)
        {
            if (pastPositions.Count < MAX_OLD_POSITIONS)
            {
                // If we haven't reached the limit, add another Vector2.
                // Create a new Vector2, otherwise it would just add a reference and the value would change.
                pastPositions.Add(new Vector2(position.X, position.Y));
            }
            else
            {
                // If we have reached the limit, reuse the oldest Vector2.
                // Pop it off the top, change the values and add it to the end again.
                Vector2 pos = pastPositions[0];
                pastPositions.RemoveAt(0);
                pos.X = position.X;
                pos.Y = position.Y;
                pastPositions.Add(pos);
            }
        }

        /// <summary>
        /// Returns the position from [index] frames ago.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 GetPastPosition(int index)
        {
            // Check if we've saved [index] entries.
            if (index < pastPositions.Count)
            {
                // Return the position [index] places from the end of the list.
                return pastPositions[pastPositions.Count - index - 1];
            }
            else
            {
                // If not enough elements, return the oldest element.
                return pastPositions[0];
            }
        }

        public Buddy SpawnBuddy()
        {
            return new Buddy(this, this.position);
        }
    }
}
