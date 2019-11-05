using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities.Sprites
{
    public class Sprite
    {
        private Texture2D spriteStrip;
        
        public virtual Rectangle Source { get; set; }
        public virtual int Width => spriteStrip.Width;
        public virtual int Height => spriteStrip.Height;
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;

        public Sprite(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2 origin = default, float rotation = 0)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new Rectangle(0, 0, Width, Height) : source;
            Opacity = opacity;
            Origin = origin;
            Rotation = rotation;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0)
        {
            spriteBatch.Draw(spriteStrip, Position, Source, Color * Opacity, Rotation, Origin, Scale * Screen.Scale, effect, layerDepth);
        }
    }
}
