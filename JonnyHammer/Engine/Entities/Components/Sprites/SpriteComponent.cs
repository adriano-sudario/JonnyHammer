using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities.Sprites
{
    public class SpriteComponent : Component
    {
        private Texture2D spriteStrip;
        public bool IsVisible { get; set; }

        public virtual Rectangle Source { get; set; }
        public virtual int SpriteWidth => spriteStrip.Width;
        public virtual int SpriteHeight => spriteStrip.Height;
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public float LayerDepth { get; set; }

        public int Width => (int)(SpriteWidth * (Entity.Transform.Scale * Screen.Scale));
        public int Height => (int)(SpriteHeight * (Entity.Transform.Scale * Screen.Scale));


        public SpriteComponent(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2 origin = default, float rotation = 0)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new Rectangle(0, 0, SpriteWidth, SpriteHeight) : source;
            Opacity = opacity;
            Origin = origin;
            Rotation = rotation;
            IsVisible = true;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var effect = FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(spriteStrip, Entity.Transform.Position, Source, Color * Opacity, Rotation, Origin, Entity.Transform.Scale * Screen.Scale, effect, LayerDepth);
        }
    }
}
