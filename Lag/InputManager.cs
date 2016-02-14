using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lag
{
    class InputManager
    {
        /// <summary>
        /// Singleton instance of InputManager.
        /// </summary>
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }
        private static InputManager instance;

        /// <summary>
        /// The current state of the keyboard.
        /// </summary>
        private KeyboardState keyboardState;

        /// <summary>
        /// The keyboard state as it was when the InputManager was last updated. Can be used to
        /// detect key presses/releases when compared with the current state.
        /// </summary>
        private KeyboardState oldKeyboardState;

        /// <summary>
        /// Initialises the internally stored input states.
        /// </summary>
        public void Initialize()
        {
            keyboardState = Keyboard.GetState();
            oldKeyboardState = keyboardState; // Initialise the "old" keyboard state to be identical at first.
        }

        /// <summary>
        /// Updates the internally stored input states. This should be called in each update loop
        /// before attempting to check the status of any inputs.
        /// </summary>
        public void Update()
        {
            // Save the old keyboard state, then update the current one.
            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Get whether the given key is is being pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is being pressed, false otherwise.</returns>
        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether the given key has been released since the last update.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns a unit vector in the direction that the movement controls are being pressed in.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMovementInput()
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
