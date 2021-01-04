using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chamboco.Engine.Inputs
{
    public class GamePadInput
    {
        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;
        private PlayerIndex index;

        public GamePadThumbSticks Thumbsticks => currentGamePadState.ThumbSticks;
        public GamePadTriggers Triggers => currentGamePadState.Triggers;
        public GamePadDPad DPad => currentGamePadState.DPad;

        public GamePadInput(PlayerIndex index) => this.index = index;

        public bool IsPressing(Buttons button) => currentGamePadState.IsButtonDown(button);

        public bool HasPressed(Buttons button) => previousGamePadState.IsButtonUp(button) && currentGamePadState.IsButtonDown(button);

        public bool HasReleased(Buttons button) => previousGamePadState.IsButtonDown(button) && currentGamePadState.IsButtonUp(button);

        public void Vibrate(float intensity = 1f) => GamePad.SetVibration(index, intensity, intensity);
    }
}
