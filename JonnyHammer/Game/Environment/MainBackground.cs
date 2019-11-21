using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public override void Load()
        {
            base.Load();

            Camera.AreaWidth = sprite.Width;
            Camera.AreaHeight = sprite.Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
