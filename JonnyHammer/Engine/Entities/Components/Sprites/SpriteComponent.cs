using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities.Sprites
{
    public class SpriteComponent : Component, IScalable
    {
        private Texture2D spriteStrip;
        public bool IsVisible { get; set; }

        public virtual Rectangle Source { get; set; }
        public virtual int SpriteWidth => spriteStrip.Width;
        public virtual int SpriteHeight => spriteStrip.Height;
        public float SpriteScale { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Vector2 SpritePosition { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public float LayerDepth { get; set; }

        public int Width => (int)(SpriteWidth * (Scale * Screen.Scale));
        public int Height => (int)(SpriteHeight * (Scale * Screen.Scale));

        public float Scale { get => SpriteScale; set => SpriteScale = value; }

        public SpriteComponent(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2 origin = default, float rotation = 0)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new Rectangle(0, 0, SpriteWidth, SpriteHeight) : source;
            Opacity = opacity;
            Origin = origin;
            Rotation = rotation;
            IsVisible = true;
        }


        public override void Start()
        {
            SpriteScale = Entity.Scale;
            SpritePosition = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (!IsVisible) return;

            var effect = FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(spriteStrip, SpritePosition, Source, Color * Opacity, Rotation, Origin, SpriteScale * Screen.Scale, effect, LayerDepth);
        }
    }
}
