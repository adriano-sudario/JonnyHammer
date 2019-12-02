using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class Scenery : Entity
    {
        protected SpriteComponent sprite;

        public string TextureName { get; set; }
        public Vector2 Source { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override void Load()
        {
            base.Load();

            sprite = new SpriteComponent(Loader.LoadTexture(TextureName),
                source: new Rectangle((int)Source.X, (int)Source.Y, Width, Height));
            AddComponent(sprite);
        }

    }
}
