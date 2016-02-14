using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Lag.Entities;

namespace Lag.Screens
{
    /// <summary>
    /// Class representing the main gameplay screen.
    /// </summary>
    class LevelScreen:Screen
    {
        /// <summary>
        /// Saved reference to the content manager for loading and retrieving entity content.
        /// </summary>
        private ContentManager contentManager;

        private Player player;
        private List<Enemy> enemies;
        private Queue<Enemy> deadEnemies;

        double enemySpawnInterval = 2.0;
        double enemySpawnTimer = 0.0;

        private const int MAP_WIDTH = 800;
        private const int MAP_HEIGHT = 600;

        public LevelScreen()
            : base()
        {
            player = new Player(new Vector2(MAP_WIDTH / 2.0f, MAP_HEIGHT / 2.0f));
            enemies = new List<Enemy>();
            deadEnemies = new Queue<Enemy>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            player.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            // Decrement enemy spawn timer and spawn if the interval has expired.
            enemySpawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (enemySpawnTimer <= 0.0)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnInterval; // Reset the timer.
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);

                // If the enemy is dead or outside the map, put on removal queue.
                if (enemy.IsDead || enemy.Position.X > MAP_WIDTH + 30)
                {
                    deadEnemies.Enqueue(enemy);
                }
            }

            // Remove any dead enemies from the enemies list.
            RemoveDeadEnemies();

            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            player.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Spawns a new enemy on the left side of the level at a random y position.
        /// </summary>
        public void SpawnEnemy()
        {
            // Generate a random y position.
            float randHeight = (float)LagGame.Rand.NextDouble() * (float)MAP_HEIGHT;
            // Spawn the enemy at that height and off of the left-hand edge.
            Enemy newEnemy = new Enemy(new Vector2(-30.0f, randHeight), new Vector2(3.0f, 0.0f));
            newEnemy.LoadContent(contentManager);
            enemies.Add(newEnemy);
        }

        /// <summary>
        /// Removes all dead enemies from the level.
        /// </summary>
        public void RemoveDeadEnemies()
        {
            while (deadEnemies.Count > 0)
            {
                enemies.Remove(deadEnemies.Dequeue());
            }
        }
    }
}
