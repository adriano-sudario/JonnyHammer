using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine
{
    public class MoveComponent : Component
    {
        private SpriteComponent Sprite;

        public Vector2 PreviousPosition { get; private set; }

        public override void Start()
        {
            Sprite = GetComponent<SpriteComponent>();
            MoveTo(Position);
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
                position.X = MathHelper.Clamp(position.X, 0, Camera.AreaWidth - Sprite.Width);
                position.Y = MathHelper.Clamp(position.Y, 0, Camera.AreaHeight - Sprite.Height);
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
            float totalScale = (Entity.Scale * Screen.Scale);
            Vector2 updatedOrigin = origin == 0 ? Vector2.Zero : new Vector2((Sprite.Width * origin) / totalScale, (Sprite.Height * origin) / totalScale);

            if (keepInPlace)
                MoveAndSlide((updatedOrigin * totalScale) - (Sprite.Origin * totalScale), false);

            Sprite.Origin = updatedOrigin;
        }

        public void SetOrigin(Vector2 origin, bool keepInPlace = true)
        {
            if (Sprite == null)
                return;

            float totalScale = (Entity.Scale * Screen.Scale);
            Sprite.Origin = new Vector2((Sprite.Width * origin.X) / totalScale, (Sprite.Height * origin.Y) / totalScale) * -1;

            if (keepInPlace)
                MoveAndSlide(Sprite.Origin * totalScale);
        }
    }
}
