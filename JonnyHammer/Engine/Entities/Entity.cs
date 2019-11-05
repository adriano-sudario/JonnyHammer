using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities
{
    public class Entity : IDraw, IUpdate
    {
        private Rectangle? customCollision;

        protected bool isActive = true;

        private Vector2 position;
        private float scale;

        public Sprite Sprite { get; private set; }
        public float Scale
        {
            get => Sprite?.Scale ?? scale;
            set
            {
                scale = value;
                if (Sprite != null)
                    Sprite.Scale = scale;
            }
        }
        public Direction.Horizontal FacingDirection { get; set; }
        public Vector2 PreviousPosition { get; private set; }
        public Vector2 Position
        {
            get => Sprite?.Position ?? position;
            private set
            {
                position = value;
                if (Sprite != null)
                    Sprite.Position = position;
            }
        }
        public Rectangle Collision
        {
            get
            {
                Vector2 spriteSource = (Sprite?.Origin ?? Vector2.Zero) * (Scale * Screen.Scale);
                return customCollision ?? new Rectangle((int)(Position.X - spriteSource.X), (int)(Position.Y - spriteSource.Y), Width, Height);
            }
        }
        public int Width => (int)((Sprite?.Width ?? 0) * (Scale * Screen.Scale));
        public int Height => (int)((Sprite?.Height ?? 0) * (Scale * Screen.Scale));
        public bool IsVisible { get; set; }

        public Entity(Vector2 position, Sprite sprite = null, Direction.Horizontal facingDirection = Direction.Horizontal.Right,
            float scale = 1f, Rectangle? customCollision = null)
        {
            IsVisible = true;
            Sprite = sprite;
            FacingDirection = facingDirection;
            Scale = scale;
            this.customCollision = customCollision;
            MoveTo(position);
        }

        public void ReplaceSprite(Sprite sprite, Rectangle? customCollision = null)
        {
            Sprite = sprite;
            this.customCollision = customCollision;
        }

        public void MoveTo(Vector2 position, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            PreviousPosition = Position;

            if (Position.X != position.X && setFacingDirection)
            {
                float horizontalDifference = position.X - Position.X;
                FacingDirection = horizontalDifference < 0 ? Direction.Horizontal.Left : Direction.Horizontal.Right;
            }

            if (keepOnScreenBounds)
            {
                position.X = MathHelper.Clamp(position.X, 0, Camera.AreaWidth - Width);
                position.Y = MathHelper.Clamp(position.Y, 0, Camera.AreaHeight - Height);
            }
            
            Position = position;
        }

        public void MoveTo(int x, int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(x, y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveHorizontally(int x, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(x, Position.Y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveVertically(int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(Position.X, y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveAndSlide(int x, int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(Position.X + x, Position.Y + y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveAndSlide(Vector2 position, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            if (position != Vector2.Zero)
                MoveTo(Position + position, setFacingDirection, keepOnScreenBounds);
        }

        public void SetOrigin(float origin, bool keepInPlace = true)
        {
            float totalScale = (Scale * Screen.Scale);
            Vector2 updatedOrigin = origin == 0 ? Vector2.Zero : new Vector2((Width * origin) / totalScale, (Height * origin) / totalScale);

            if (keepInPlace)
                MoveAndSlide((updatedOrigin * totalScale) - (Sprite.Origin * totalScale), false);

            Sprite.Origin = updatedOrigin;
        }

        public void SetOrigin(Vector2 origin, bool keepInPlace = true)
        {
            if (Sprite == null)
                return;

            float totalScale = (Scale * Screen.Scale);
            Sprite.Origin = new Vector2((Width * origin.X) / totalScale, (Height * origin.Y) / totalScale) * -1;

            if (keepInPlace)
                MoveAndSlide(Sprite.Origin * totalScale);
        }

        public void CustomizeCollision(Rectangle collision)
        {
            customCollision = collision;
        }

        public void ResetCollisionToSpriteBounds()
        {
            customCollision = null;
        }

        public bool CollidesWith(Entity body)
        {
            return Collision.Intersects(body.Collision);
        }

        protected void UpdateAnimation(GameTime gameTime)
        {
            if (Sprite != null && Sprite.GetType() == typeof(AnimatedSprite))
                (Sprite as AnimatedSprite).Update(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!isActive)
                return;

            UpdateAnimation(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isActive || !IsVisible)
                return;

            Sprite?.Draw(spriteBatch, effect: FacingDirection == Direction.Horizontal.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
