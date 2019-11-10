using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Entities.Components.Collider;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace JonnyHammer.Engine.Entities.Components.Phisycs
{
    public class PhysicsComponent : Component
    {
        readonly ColliderComponent collider;
        private readonly float mass;

        public Body Body { get; private set; }
        public BodyType BodyType { get; private set; }
        public Vector2 MaxVelocity { get; set; } = new Vector2(3, 3);

        public Vector2 Velocity
        {
            get => Body.LinearVelocity;
            set => Body.LinearVelocity = value;
        }

        public const float PixelsPerMeter = 100;

        public IList<Body> Collided { get; } = new List<Body>();


        public PhysicsComponent(BodyType bodyType, ColliderComponent collider, float mass = 1)
        {
            BodyType = bodyType;
            this.collider = collider;
            this.mass = mass;
        }

        public override void Start()
        {
            Configure();
            Entity.OnSetScale += Entity_OnSetScale;
        }

        public override void Dispose()
        {
            Entity.OnSetScale -= Entity_OnSetScale;
            Entity.Destroy();
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
            Body.Remove(fixture);
            var rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            Body.CreatePolygon(rectangleVertices, d);
        }

        void Configure()
        {
            Body = CreateBody();

            //collider.IsTrigger = true;
            //collider.AutoCheck = false;
        }

        private Body CreateBody()
        {
            var body = SceneManager.CurrentScene.World.CreateRectangle(
                width,
                height,
                1f, new Vector2(x + width / 2, y - height / 2));

            //body.SetRestitution(0.5f);
            //body.SetFriction(0.3f);
            body.BodyType = BodyType;
            body.Tag = Entity.Name;
            body.OnCollision += Body_OnCollision;
            body.Tag = Entity;
            body.Mass = mass;
            return body;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            Collided.Add(other.Body);
            return true;
        }

        public override void Update(GameTime gameTime)
        {

            Entity.Position = (Body.Position - new Vector2(width, height) / 2) * PixelsPerMeter;
            Entity.Rotation = Body.Rotation;


            if (MaxVelocity.Y > 0)
            {
                if (Body.LinearVelocity.Y > MaxVelocity.Y)
                    Body.LinearVelocity = new Vector2(Body.LinearVelocity.X, MaxVelocity.Y);

                if (Body.LinearVelocity.Y < -MaxVelocity.Y)
                    Body.LinearVelocity = new Vector2(Body.LinearVelocity.X, -MaxVelocity.Y);
            }

            if (MaxVelocity.X > 0)
            {
                if (Body.LinearVelocity.X > MaxVelocity.X)
                    Body.LinearVelocity = new Vector2(MaxVelocity.X, Body.LinearVelocity.Y);
                if (Body.LinearVelocity.X < -MaxVelocity.X)
                    Body.LinearVelocity = new Vector2(-MaxVelocity.X, Body.LinearVelocity.Y);
            }

            Collided.Clear();
        }

        public void SetVelocity(float? x = null, float? y = null, bool noconvert = false)
        {
            var convertFactor = (noconvert ? 1 : PixelsPerMeter);

            var newX = x.HasValue ? x.Value / convertFactor : Body.LinearVelocity.X;
            var newY = y.HasValue ? y.Value / convertFactor : Body.LinearVelocity.Y;
            Velocity = new Vector2(newX, newY);
        }

        public void ApplyForce(Vector2 force) => Body.ApplyLinearImpulse(force);

        public void SetVelocity(Vector2 velocity, bool noconvert = false) =>
            Velocity = velocity / (noconvert ? 1 : PixelsPerMeter);

        public void MoveForward(float addPosition) =>
            SetVelocity(addPosition, Body.LinearVelocity.Y, true);
    }
}
