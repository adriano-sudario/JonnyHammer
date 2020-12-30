using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Scenes;
using JonnyHammer.Game.Characters;
using JonnyHammer.Game.Tiles;
using JonnyHammer.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Game.Scenes
{
    public class Nujutsu : Scene
    {
        private GameObject narutao;
        private GameObject narutitos;
        private Texture2D background;
        //Floor testFloor;

        public Nujutsu()
        {
            background = Loader.LoadTexture("bg");
            Camera2D.SetBounds(background.Width * 2, background.Height);

            narutao = Spawn<Jonny>("Narutao", new Vector2(150, 200));
            narutitos = Spawn<BigNaruto>("NarutoRed", new Vector2(200, 100), x => x.MoveAmount = 100);
            Spawn<Block>("floor 1", new Vector2(0, 350), f => f.Width = 600);
            Spawn<Block>("floor 2", new Vector2(700, 350), f => f.Width = 500);
            Spawn<Block>("floor 2", new Vector2(1300, 300), f => f.Width = 400);
            
            Spawn<Box>("Box 1", new Vector2(800, 300));
            Spawn<Box>("Box 2", new Vector2(800, 250));
            Spawn<Box>("Box 3", new Vector2(800, 200));
            Spawn<Box>("Box 4", new Vector2(600, 300));
            Spawn<Box>("Box 5", new Vector2(600, 200));

            foreach (var go in Entities)
                if (go is Box or Block)
                    go.GetComponent<Collider>().IsDebug = true;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Camera2D.Follow(narutao);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera2D.GetViewTransformationMatrix());
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.Draw(background, new Vector2(background.Width, 0), null, Color.White, 0, Vector2.Zero, 1,
                SpriteEffects.FlipHorizontally, 0);

            base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

}