using Microsoft.Xna.Framework.Input;

namespace Chamboco.Engine.Inputs
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

        public bool IsPressing(Keys key) => currentKeyboardState.IsKeyDown(key);

        public bool HasPressed(Keys key) => previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);

        public bool HasReleased(Keys key) => previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
    }
}
