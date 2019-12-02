﻿using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities.Sprites
{
    public class SpriteComponent : Component
    {
        public Texture2D spriteStrip;

        public bool IsVisible { get; set; }
        public virtual Rectangle Source { get; set; }
        public virtual int SpriteWidth => spriteStrip.Width;
        public virtual int SpriteHeight => spriteStrip.Height;
        public float Opacity { get; set; }
        public Vector2? Origin { get; set; }
        public Vector2 RotateOrigin { get; set; }
        public Color Color { get; set; } = Color.White;
        public float LayerDepth { get; set; }
        public int Width => (int)(SpriteWidth * Entity.Transform.Scale);
        public int Height => (int)(SpriteHeight * Entity.Transform.Scale);

        public SpriteComponent(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2? origin = null)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new Rectangle(0, 0, SpriteWidth, SpriteHeight) : source;
            Opacity = opacity;
            Origin = origin;
            IsVisible = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var effect = FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var origin = Origin ?? new Vector2(Width / 2f, Height / 2f);

            spriteBatch.Draw(
                spriteStrip,
                (Entity.Transform.Position + (origin * Entity.Transform.Scale)) * Screen.Scale,
                Source,
                Color * Opacity,
                Entity.Transform.Rotation,
                origin,
                Screen.Scale * Entity.Transform.Scale,
                effect, LayerDepth);
        }
    }
}
