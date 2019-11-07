using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine
{
    public class MoveComponent : Component
    {
        SpriteComponent Sprite;
        ColliderComponent Collider;
        public Vector2 PreviousPosition { get; private set; }

        public override void Start()
        {
            Sprite = GetComponent<SpriteComponent>();
            Collider = GetComponent<ColliderComponent>();
            MoveTo(Entity.Position);
        }

        public void MoveTo(Vector2 position, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            PreviousPosition = Entity.Position;

            if (Entity.Position.X != position.X && setFacingDirection)
            {
                float horizontalDifference = position.X - Entity.Position.X;
                FacingDirection = horizontalDifference < 0 ? Direction.Horizontal.Left : Direction.Horizontal.Right;
            }

            if (keepOnScreenBounds)
            {
                position.X = MathHelper.Clamp(position.X, 0, Camera.AreaWidth - Sprite.Width);
                position.Y = MathHelper.Clamp(position.Y, 0, Camera.AreaHeight - Sprite.Height);
            }


            Entity.Position = position;

            if (Collider?.CollidesWithAnyEntity() == true)
            {
                Entity.Position = PreviousPosition;
                return;
            }
        }

        public void MoveTo(int x, int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(x, y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveHorizontally(int x, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(x, Entity.Position.Y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveVertically(int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(Entity.Position.X, y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveAndSlide(int x, int y, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            MoveTo(new Vector2(Entity.Position.X + x, Entity.Position.Y + y), setFacingDirection, keepOnScreenBounds);
        }

        public void MoveAndSlide(Vector2 position, bool setFacingDirection = true, bool keepOnScreenBounds = true)
        {
            if (position != Vector2.Zero)
                MoveTo(Entity.Position + position, setFacingDirection, keepOnScreenBounds);
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
