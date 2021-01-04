using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chamboco.Engine.Entities.Components.Sprites
{
    public class SpriteRenderer : Component
    {
        Texture2D spriteStrip;

        protected virtual Rectangle Source { get; set; }
        public bool IsVisible { get; set; }
        public virtual int SpriteWidth => spriteStrip.Width;
        public virtual int SpriteHeight => spriteStrip.Height;
        public float Opacity { get; set; }
        public Vector2 Origin { get; }
        public Color Color { get; set; } = Color.White;
        public float LayerDepth { get; set; }
        public int Width => (int)(SpriteWidth * Entity.Transform.Scale.X);
        public int Height => (int)(SpriteHeight * Entity.Transform.Scale.Y);

        public SpriteRenderer(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2 origin = default)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new (0, 0, SpriteWidth, SpriteHeight) : source;
            Opacity = opacity;
            Origin = origin;
            IsVisible = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var effect = Entity.Transform.FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.DrawEntity(
                spriteStrip,
                Entity.Transform,
                Source,
                Color * Opacity,
                Origin,
                effect,
                LayerDepth);

        }
    }
}
