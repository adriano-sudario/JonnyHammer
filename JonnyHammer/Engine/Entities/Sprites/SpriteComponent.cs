using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities.Sprites
{
    public class SpriteComponent : Component
    {
        private Texture2D spriteStrip;

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

        public SpriteComponent(Texture2D spriteStrip, Rectangle source = default, float opacity = 1f, Vector2 origin = default, float rotation = 0)
        {
            this.spriteStrip = spriteStrip;
            Source = source == default ? new Rectangle(0, 0, SpriteWidth, SpriteHeight) : source;
            Opacity = opacity;
            Origin = origin;
            Rotation = rotation;
        }


        public override void Start()
        {
            SpriteScale = Scale;
            SpritePosition = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var effect = FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(spriteStrip, SpritePosition, Source, Color * Opacity, Rotation, Origin, SpriteScale * Screen.Scale, effect, LayerDepth);
        }
    }
}
