using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class Scenery : GameObject
    {
        protected SpriteRenderer sprite;

        public string TextureName { get; set; }
        public Vector2 Source { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        protected override void Load()
        {
            base.Load();

            sprite = new SpriteRenderer(Loader.LoadTexture(TextureName),
                source: new Rectangle((int)Source.X, (int)Source.Y, Width, Height));
            AddComponent(sprite);
        }

    }
}
