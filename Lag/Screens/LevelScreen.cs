﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        /// <summary>
        /// The font for drawing all HUD elements.
        /// </summary>
        private SpriteFont hudFont;

        /// <summary>
        /// The background texture for HUD elements.
        /// </summary>
        private Texture2D hudTexture;

        /// <summary>
        /// Buddy will lag this number of frames behind the player's movements.
        /// </summary>
        private int lag = 0;

        /// <summary>
        /// The current score for the level.
        /// </summary>
        private int score = 0;

        /// <summary>
        /// The remaining time limit for play.
        /// </summary>
        private TimeSpan timeLimit = TimeSpan.FromMinutes(3.0);

        private Player player;
        private Buddy buddy;

        private List<Enemy> enemies;
        private Queue<Enemy> deadEnemies;
        private double enemySpawnInterval = 2.0;
        private double enemySpawnTimer = 0.0;
        private SoundEffect enemyHitSound;

        private List<Pickup> pickups;
        private Queue<Pickup> deadPickups;
        private double pickupSpawnInterval = 4.13;
        private double pickupSpawnTimer = 0.5;
        private SoundEffect pickupCollectSound;

        private SoundEffect bgMusic;

        private const int MAP_WIDTH = 800;
        private const int MAP_HEIGHT = 600;

        public LevelScreen(ScreenManager manager)
            : base(manager)
        {
            player = new Player(new Vector2(MAP_WIDTH / 2.0f, MAP_HEIGHT / 2.0f));
            buddy = player.SpawnBuddy();

            enemies = new List<Enemy>();
            deadEnemies = new Queue<Enemy>();

            pickups = new List<Pickup>();
            deadPickups = new Queue<Pickup>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            hudFont = contentManager.Load<SpriteFont>(@"fonts\hudfont");
            hudTexture = contentManager.Load<Texture2D>("hud");

            enemyHitSound = contentManager.Load<SoundEffect>(@"sound\enemy-hit");
            pickupCollectSound = contentManager.Load<SoundEffect>(@"sound\pickup");

            bgMusic = contentManager.Load<SoundEffect>(@"sound\music\main");
            bgMusic.Play();

            player.LoadContent(contentManager);
            buddy.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            // Decrement the play time remaining.
            timeLimit = timeLimit.Subtract(gameTime.ElapsedGameTime);
            if (timeLimit <= TimeSpan.Zero)
            {
                // If time is up, log the score...
                string scoreId = Scores.HighScores.Normal.AddScore(DateTime.Today.ToShortDateString(), score);
                Scores.HighScores.Normal.SaveToFile(Scores.HighScores.NormalFilePath);

                // ...and go to the high scores screen.
                GoToScreen(new HighScoreScreen(manager, Scores.HighScores.Normal, scoreId));
            }

            // Decrement enemy spawn timer and spawn if the interval has expired.
            enemySpawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (enemySpawnTimer <= 0.0)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnInterval; // Reset the timer.
            }

            // Update enemies.
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);

                // Check for collision with player.
                if (CheckCollision(enemy, player))
                {
                    lag += 10;
                    enemyHitSound.Play();
                    enemy.Kill();
                }

                // Check for collision with buddy.
                if (CheckCollision(enemy, buddy))
                {
                    lag += 20;
                    enemyHitSound.Play();
                    enemy.Kill();
                }

                // If the enemy is dead or outside the map, put on removal queue.
                if (enemy.IsDead || enemy.Position.X > MAP_WIDTH + 30)
                {
                    deadEnemies.Enqueue(enemy);
                }
            }

            // Remove any dead enemies from the enemies list.
            RemoveDeadEnemies();


            // Decrement pickup spawn timer and spawn if the interval has expired.
            pickupSpawnTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (pickupSpawnTimer <= 0.0)
            {
                SpawnPickup();
                pickupSpawnTimer = pickupSpawnInterval; // Reset the timer.
            }

            // Update pickups.
            foreach (Pickup pickup in pickups)
            {
                pickup.Update(gameTime);

                // Check for collision with player.
                if (CheckCollision(pickup, player))
                {
                    // If player collects pickup, add 10 points and reduce lag by 1
                    score += 10;
                    if (lag > 0)
                    {
                        lag -= 1;
                    }
                    pickupCollectSound.Play();
                    pickup.Kill();
                }

                // Check for collision with buddy.
                if (CheckCollision(pickup, buddy))
                {
                    // If Buddy collects pickup, add 20 points and reduce lag by 10
                    score += 20;
                    if (lag >= 10)
                    {
                        lag -= 10;
                    }
                    else
                    {
                        lag = 0;
                    }
                    pickupCollectSound.Play();
                    pickup.Kill();
                }

                // If the enemy is dead or outside the map, put on removal queue.
                if (pickup.IsDead || pickup.Position.X > MAP_WIDTH + 30)
                {
                    deadPickups.Enqueue(pickup);
                }
            }

            // Remove any dead pickups from the enemies list.
            RemoveDeadPickups();
            

            player.Update(gameTime);
            buddy.Update(gameTime, lag);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawHud(spriteBatch);
            
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            foreach (Pickup pickup in pickups)
            {
                pickup.Draw(gameTime, spriteBatch);
            }

            player.Draw(gameTime, spriteBatch);
            buddy.Draw(spriteBatch);
        }

        /// <summary>
        /// Draws all HUD elements.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use when drawing.</param>
        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hudTexture, new Vector2(5, 5), new Rectangle(0, 0, 115, 76), Color.White);
            spriteBatch.DrawString(hudFont, score.ToString(), new Vector2(11, 33), Color.White);

            spriteBatch.Draw(hudTexture, new Vector2(680, 5), new Rectangle(115, 0, 115, 76), Color.White);
            spriteBatch.DrawString(hudFont, lag.ToString(), new Vector2(686, 33), Color.White);

            string timeString = timeLimit.Minutes.ToString() + " " + FormatTimeComponent(timeLimit.Seconds);
            float timeWidth = hudFont.MeasureString(timeString).X;
            spriteBatch.DrawString(hudFont, timeString, new Vector2(400 - (timeWidth / 2.0f), 15), Color.Black);
        }

        /// <summary>
        /// Adds a leading zero to timer components that are less than 10 and returns it as a
        /// string.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private string FormatTimeComponent(int component)
        {
            string timeString = component.ToString();
            if (component < 10)
            {
                timeString = "0" + timeString;
            }

            return timeString;
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
        /// Spawns a new pickup on the left side of the level at a random y position.
        /// </summary>
        public void SpawnPickup()
        {
            // Generate a random y position.
            float randHeight = (float)LagGame.Rand.NextDouble() * (float)MAP_HEIGHT;
            // Spawn the pickup at that height and off of the left-hand edge.
            Pickup newPickup = new Pickup(new Vector2(-30.0f, randHeight), new Vector2(3.0f, 0.0f));
            newPickup.LoadContent(contentManager);
            pickups.Add(newPickup);
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

        /// <summary>
        /// Removes all dead pickups from the level.
        /// </summary>
        public void RemoveDeadPickups()
        {
            while (deadPickups.Count > 0)
            {
                pickups.Remove(deadPickups.Dequeue());
            }
        }

        /// <summary>
        /// Checks for collision between two entities.
        /// </summary>
        /// <param name="ent1">The first entity.</param>
        /// <param name="ent2">The second entity.</param>
        /// <returns>True if the entities are in collision, false otherwise.</returns>
        public bool CheckCollision(Entity ent1, Entity ent2)
        {
            // Get the sum of the entities' collision radii.
            float radiusSum = ent1.Radius + ent2.Radius;

            // If the radius sum squared is more than the square distance between the entities, they
            // are colliding.
            return (radiusSum * radiusSum) >= (ent2.Position - ent1.Position).LengthSquared();
        }
    }
}
