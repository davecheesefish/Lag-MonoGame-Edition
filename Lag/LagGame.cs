using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lag.Screens;

namespace Lag
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LagGame : Game
    {
        /// <summary>
        /// Shared random number generator instance.
        /// </summary>
        public static Random Rand { get { return rand; } }
        private static Random rand;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D backgroundTex;

        ScreenManager screenManager;

        public LagGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rand = new Random(541);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            screenManager = new ScreenManager(Content);
            screenManager.GoTo(new LevelScreen(screenManager));

            InputManager.Instance.Initialize();

            IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0 / 60.0); // 60 FPS

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTex = Content.Load<Texture2D>("background");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
            
            if (InputManager.Instance.KeyReleased(Keys.Escape))
            {
                Exit();
            }

            // Update the active screen
            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White);
            screenManager.Draw(gameTime, spriteBatch); // Draw the active screen.
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
