using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Lag.Entities
{
    /// <summary>
    /// Represents a moving entity in the game world.
    /// </summary>
    abstract class Entity
    {
        /// <summary>
        /// The collision radius of this entity.
        /// </summary>
        public virtual float Radius { get { return Radius; } }
        protected float radius = 0;

        public Vector2 Position { get { return position; } }
        protected Vector2 position;

        protected Vector2 velocity;

        public Entity(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public virtual void Update(GameTime gameTime)
        {
            position += velocity;
        }
    }
}
