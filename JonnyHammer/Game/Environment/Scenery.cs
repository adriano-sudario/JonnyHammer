using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class Scenery : GameObject
    {
        protected SpriteRenderer sprite;

        public string TextureName { get; }
        public Vector2 Source { get; }
        public int Width { get; }
        public int Height { get; }

        public Scenery(string textureName, Vector2 source, int width, int height)
        {
            TextureName = textureName;
            Source = source;
            Width = width;
            Height = height;

            sprite = new SpriteRenderer(Loader.LoadTexture(TextureName),
                source: new Rectangle((int)Source.X, (int)Source.Y, Width, Height));
            AddComponent(sprite);
        }

    }
}
