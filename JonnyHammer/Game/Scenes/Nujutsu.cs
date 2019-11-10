using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Scenes;
using JonnyHammer.Game.Characters;
using JonnyHammer.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Game.Scenes
{
    public class Nujutsu : Scene
    {
        private Entity narutao;
        private Texture2D background;

        public Nujutsu()
        {
            background = Loader.LoadTexture("bg");
            Camera.AreaWidth = background.Width * 2;
            Camera.AreaHeight = background.Height;

            narutao = Spawn<BigNarutoFisica>("Narutao", new Vector2(100, 200));
            //narutao = Spawn<BigNaruto>("Narutao", new Vector2(100, 200));

            Spawn<Floor>("Chao 1", new Vector2(0, 350), f => f.Width = 600);
            Spawn<Floor>("Chao 2", new Vector2(700, 350), f => f.Width = 500);
            Spawn<Floor>("Chao 2", new Vector2(1300, 300), f => f.Width = 400);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Camera.Update(narutao);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.ViewMatrix);
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(background.Width, 0), null, Color.White, 0, Vector2.Zero, 1,
                SpriteEffects.FlipHorizontally, 0);

            base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}