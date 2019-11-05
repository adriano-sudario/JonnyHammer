using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Inputs;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Scenes;
using JonnyHammer.Game.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JonnyHammer.Game.Scenes
{
    public class Nujutsu : Scene
    {
        private BigNaruto narutao;
        private KeyboardInput keyboard;
        private Texture2D background;

        public Nujutsu()
        {
            background = Loader.LoadTexture("bg");
            Camera.AreaWidth = background.Width * 2;
            Camera.AreaHeight = background.Height;
            keyboard = new KeyboardInput();
            narutao = new BigNaruto(new Vector2(100, 300));
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();

            if (keyboard.IsPressing(Keys.Right))
                narutao.Run(Engine.Helpers.Direction.Horizontal.Right);
            else if (keyboard.IsPressing(Keys.Left))
                narutao.Run(Engine.Helpers.Direction.Horizontal.Left);
            else
                narutao.Animation.Change("Idle");

            narutao.Update(gameTime);

            Camera.Update(narutao);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.ViewMatrix);
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(background.Width, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            narutao.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
