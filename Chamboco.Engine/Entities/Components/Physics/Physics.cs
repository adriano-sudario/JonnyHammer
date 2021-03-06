﻿using Chamboco.Engine.Managers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Chamboco.Engine.Entities.Components.Physics
{
    public class Physics : Component
    {
        readonly Collider.Collider collider;
        float friction;
        float restitution;

        public Body Body { get; private set; }
        public BodyType BodyType { get; }
        public Vector2 MaxVelocity { get; set; } = new(3, 3);

        public float Mass { get; }
        public bool UseMaxVelocityX { get; } = false;
        public bool UseMaxVelocityY { get; set; }

        public Vector2 Velocity
        {
            get => Body.LinearVelocity;
            set => Body.LinearVelocity = value;
        }

        public static float PixelsPerMeter => 100;

        public IList<Body> Collided { get; } = new List<Body>();


        public Physics(BodyType bodyType, Collider.Collider collider, float mass = 1, float friction = 0, float restitution = 0)
        {
            BodyType = bodyType;
            Mass = mass;
            this.collider = collider;
            this.friction = friction;
            this.restitution = restitution;
        }

        public override void Start()
        {
            Configure();
            Entity.Transform.OnSetScale += Entity_OnSetScale;
            Entity.Transform.SetPhysicsComponent(this);

        }

        public override void Dispose()
        {
            Entity.Transform.OnSetScale -= Entity_OnSetScale;
            base.Dispose();
        }

        float width => collider.Bounds.Width / PixelsPerMeter;
        float height => collider.Bounds.Height / PixelsPerMeter;

        float x => collider.Bounds.X / PixelsPerMeter;
        float y => collider.Bounds.Y / PixelsPerMeter;


        void Entity_OnSetScale()
        {
            var fixture = Body.FixtureList[0];
            var d = fixture.Shape.Density;
            var mass = Body.Mass;

            Body.Remove(fixture);
            var rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            Body.CreatePolygon(rectangleVertices, d);

            Body.Mass = mass;
        }

        void Configure()
        {
            Body = CreateBody();
        }

        private Body CreateBody()
        {
            var body = SceneManager.CurrentScene.World.CreateRectangle(
                width,
                height,
                1f, new Vector2(x + width / 2, y - height / 2));

            //body.FixedRotation = true;
            body.SetRestitution(restitution);
            body.SetFriction(friction);
            body.BodyType = BodyType;
            body.OnCollision += Body_OnCollision;
            body.Tag = Entity;
            body.Mass = Mass;
            return body;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            Collided.Add(other.Body);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            Entity.Transform.MoveTo((Body.Position - new Vector2(width, height) / 2) * PixelsPerMeter, ignorePhysics: true);
            Entity.Transform.Rotation = Body.Rotation;


            if (UseMaxVelocityY && MaxVelocity.Y > 0)
            {
                if (Body.LinearVelocity.Y > MaxVelocity.Y)
                    Body.LinearVelocity = new Vector2(Body.LinearVelocity.X, MaxVelocity.Y);
                if (Body.LinearVelocity.Y < -MaxVelocity.Y)
                    Body.LinearVelocity = new Vector2(Body.LinearVelocity.X, -MaxVelocity.Y);
            }
            if (UseMaxVelocityX && MaxVelocity.X > 0)
            {
                if (Body.LinearVelocity.X > MaxVelocity.X)
                    Body.LinearVelocity = new Vector2(MaxVelocity.X, Body.LinearVelocity.Y);
                if (Body.LinearVelocity.X < -MaxVelocity.X)
                    Body.LinearVelocity = new Vector2(-MaxVelocity.X, Body.LinearVelocity.Y);
            }

            Collided.Clear();
        }

        public void SetVelocity(float? velocityX = null, float? velocityY = null, bool noConvert = false)
        {
            var convertFactor = (noConvert ? 1 : PixelsPerMeter);

            var newX = velocityX.HasValue ? velocityX.Value / convertFactor : Body.LinearVelocity.X;
            var newY = velocityY.HasValue ? velocityY.Value / convertFactor : Body.LinearVelocity.Y;
            Velocity = new Vector2(newX, newY);
        }
        public void ResetVelocity() => SetVelocity(0, 0);

        public void ApplyForce(Vector2 force) => Body.ApplyLinearImpulse(force);

        public void SetVelocity(Vector2 velocity, bool noConvert = false) =>
            Velocity = velocity / (noConvert ? 1 : PixelsPerMeter);

        public void MoveForward(float addPosition) =>
            SetVelocity(addPosition, Body.LinearVelocity.Y, true);

        public void MoveTo(Vector2 position)
        {
            var (newX, newY) = position;
            Body.Position =
                new Vector2(newX / PixelsPerMeter, newY / PixelsPerMeter)
                + new Vector2(width / 2, height / 2);
        }
    }
}
