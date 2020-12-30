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
        private Texture2D background;
        //Floor testFloor;

        public Nujutsu()
        {
            background = Loader.LoadTexture("bg");
            Camera2D.SetBounds(background.Width * 2, background.Height);
            narutao  = new Jonny();
            var narutitos  = new BigNaruto(100,new (200, 100));
            
            Spawn(narutao, new (150, 200), "Narutao");
            Spawn(narutitos , name: "NarutoRed");
            
            Spawn(new Block("floor 1", 600, 30) ,new (0, 350));
            Spawn(new Block("floor 2", 500, 30) ,new (700,350));
            Spawn(new Block("floor 2",  400, 30),new (1300,300));
            
            Spawn(new Box(), new (800, 300),"Box 1");
            Spawn(new Box(), new (800, 250),"Box 2");
            Spawn(new Box(), new (800, 200),"Box 3");
            Spawn(new Box(), new (700, 300),"Box 4");
            Spawn(new Box(), new (700, 200),"Box 5");

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