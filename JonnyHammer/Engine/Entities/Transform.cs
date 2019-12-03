﻿using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using System;

namespace JonnyHammer.Engine.Entities
{
    public class Transform
    {
        Physics physicsComponent;
        Vector2 position;

        public Vector2 PreviousPosition { get; private set; }
        public Direction.Horizontal FacingDirection { get; set; } = Direction.Horizontal.Right;

        float _scale = 1;
        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnSetScale();
            }
        }

        public float Rotation { get; set; }
        public Vector2 Origin { get; private set; }
        public Vector2 Position => position;
        public float X => position.X;
        public float Y => position.Y;



        public void FixPosition(Vector2 position) => this.position = position;

        public event Action OnSetScale = delegate { };

        public void SetPhysicsComponent(Physics physicsComponent)
        {
            this.physicsComponent = physicsComponent;
        }

        public void MoveTo(Vector2 position, bool setFacingDirection = true, bool ignorePhysics = false)
        {
            PreviousPosition = this.position;

            if (X != position.X && setFacingDirection)
            {
                float horizontalDifference = position.X - X;

                if (Math.Abs(horizontalDifference) > .1)
                    FacingDirection = horizontalDifference < 0 ? Direction.Horizontal.Left : Direction.Horizontal.Right;
            }

            this.position = position;
            if (physicsComponent == null || ignorePhysics)
                this.position = position;
            else
                physicsComponent.MoveTo(position);
        }

        public void KeepOnCameraBounds(Entity entity)
        {
            int width = 1, height = 1;

            if (entity.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                width = renderer.Width;
                height = renderer.Height;
            }


            position.X = MathHelper.Clamp(position.X, 0, Camera.AreaWidth - width);
            position.Y = MathHelper.Clamp(position.Y, 0, Camera.AreaHeight - height);
        }

        public void MoveTo(float x, float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(x, y), setFacingDirection);

        public void MoveHorizontally(float x, bool setFacingDirection = true) =>
            MoveTo(new Vector2(x, Y), setFacingDirection);

        public void MoveVertically(float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X, y), setFacingDirection);

        public void MoveAndSlide(float x, float y, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X + x, position.Y + y), setFacingDirection);

        public void MoveAndSlideHorizontally(float amount, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X + amount, Y), setFacingDirection);

        public void MoveAndSlideVertically(float amount, bool setFacingDirection = true) =>
            MoveTo(new Vector2(X, position.Y + amount), setFacingDirection);

        public void MoveAndSlide(Vector2 position, bool setFacingDirection = true) =>
            MoveTo(this.position + position, setFacingDirection);

        //public void SetOrigin(float origin, Entity entity, bool keepInPlace = true)
        //{
        //    float totalScale = (Scale);
        //    Vector2 updatedOrigin = origin == 0 ? Vector2.Zero : new Vector2((entity.Width * origin) / totalScale, (entity.Height * origin) / totalScale);

        //    if (keepInPlace)
        //        MoveAndSlide((updatedOrigin * totalScale) - (Origin * totalScale), false);

        //    Origin = updatedOrigin;
        //}

        //public void SetOrigin(Vector2 origin, Entity entity, bool keepInPlace = true)
        //{
        //    float totalScale = (Scale);
        //    Origin = new Vector2((entity.Width * origin.X) / totalScale, (entity.Height * origin.Y) / totalScale) * -1;

        //    if (keepInPlace)
        //        MoveAndSlide(Origin * totalScale);
        //}

        public void Rotate(float degrees) => Rotation = MathHelper.ToRadians(degrees);
        public float GetRotationInDegrees() => MathHelper.ToDegrees(Rotation);

    }
}
