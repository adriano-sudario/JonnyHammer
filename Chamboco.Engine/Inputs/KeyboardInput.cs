using Microsoft.Xna.Framework.Input;

namespace JonnyHamer.Engine.Inputs
{
    public class KeyboardInput
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public bool IsPressing(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool HasPressed(Keys key)
        {
            return previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);
        }

        public bool HasReleased(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }
    }
}
