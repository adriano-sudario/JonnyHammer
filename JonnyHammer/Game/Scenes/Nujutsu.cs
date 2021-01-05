﻿using Chamboco.Engine.Entities;
using System;
using Chamboco.Engine.Entities.Components.Collider;
using Chamboco.Engine.Helpers;
using Chamboco.Engine.Scenes;
using JonnyHammer.Game.Characters;
using JonnyHammer.Game.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Game.Scenes
{
    public class Nujutsu : Scene
    {
        GameObject narutao;
        Texture2D background;

        private Random random = new();

        public override void Start()
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
            Spawn(new Box(), new (800, 270),"Box 2");
            Spawn(new Box(), new (800, 240),"Box 3");
            Spawn(new Box(), new (750, 370),"Box 4");
            Spawn(new Box(), new (750, 340),"Box 5");

            foreach (var go in Entities)
                if (go is Box or Block)
                    go.GetComponent<Collider>().IsDebug = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Camera2D.Follow(narutao);

            RainBlocks();
        }

        private void RainBlocks()
        {
            if (random.Next(30) != 5) return;
            var pos = random.Next(0, background.Width * 2);
            var box = new Box();
            box.GetComponent<Collider>().IsDebug = true;
            Spawn(box, new (pos, -30),"Box 1");
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
