using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class Scenery : Entity
    {
        private int width;
        private int height;

        protected SpriteComponent sprite;
        
        public string TextureName { get; set; }
        public Vector2 Source { get; set; }
        public override int Width { get => width; set => width = value; }
        public override int Height { get => height; set => height = value; }

        public override void Load()
        {
            base.Load();

            sprite = new SpriteComponent(Loader.LoadTexture(TextureName), 
                source: new Rectangle((int)Source.X, (int)Source.Y, Width, Height));
            AddComponent(sprite);
        }
    }
}
