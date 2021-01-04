using Chamboco.Engine.Entities.Components;
using Chamboco.Engine.Entities.Components.Physics;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using System;

namespace Chamboco.Engine.Entities
{
    public class Transform : Transform2D.Transform2D
    {
        Physics physicsComponent;

        public Direction.Horizontal FacingDirection { get; set; } = Direction.Horizontal.Right;

        public override Vector2 Scale
        {
            get => base.Scale;
            set
            {
                base.Scale = value;
                OnSetScale();
            }
        }

        public float X => Position.X;
        public float Y => Position.Y;

        public void FixPosition(Vector2 position) => Position = position;

        public event Action OnSetScale = delegate { };

        public void SetPhysicsComponent(Physics physicsComponent) =>
            this.physicsComponent = physicsComponent;

        public void MoveTo(Vector2 position, bool setFacingDirection = true, bool ignorePhysics = false)
        {
            if (Math.Abs(X - position.X) > 0.01 && setFacingDirection)
            {
                var horizontalDifference = position.X - X;

                if (Math.Abs(horizontalDifference) > .1)
                    FacingDirection = horizontalDifference < 0 ? Direction.Horizontal.Left : Direction.Horizontal.Right;
            }

            Position = position;
            if (physicsComponent == null || ignorePhysics)
                Position = position;
            else
                physicsComponent.MoveTo(position);
        }

        public void KeepOnCameraBounds(GameObject entity)
        {
            int width = 1, height = 1;

            if (entity.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                width = renderer.Width;
                height = renderer.Height;
            }

            Position = new (
                MathHelper.Clamp(Position.X, 0, Screen.VirtualWidth - width),
                MathHelper.Clamp(Position.Y, 0, Screen.VirtualHeight - height));
        }

        public void MoveTo(float x, float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(x, y), setFacingDirection);

        public void MoveHorizontally(float x, bool setFacingDirection = true) =>
            MoveTo(new Vector2(x, Y), setFacingDirection);

        public void MoveVertically(float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X, y), setFacingDirection);

        public void MoveAndSlide(float x, float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X + x, Position.Y + y), setFacingDirection);

        public void MoveAndSlideHorizontally(float amount, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X + amount, Y), setFacingDirection);

        public void MoveAndSlideVertically(float amount, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X, Position.Y + amount), setFacingDirection);

        public void MoveAndSlide(Vector2 position, bool setFacingDirection = true) =>
            MoveTo(this.Position + position, setFacingDirection);

        public void Rotate(float degrees) => Rotation = MathHelper.ToRadians(degrees);
        public float GetRotationInDegrees() => MathHelper.ToDegrees(Rotation);

    }
}
