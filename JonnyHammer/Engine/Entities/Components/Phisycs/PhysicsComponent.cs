using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Entities.Components.Collider;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Engine.Entities.Components.Phisycs
{
    public class PhysicsComponent : Component
    {
        readonly ColliderComponent collider;

        public Body Body { get; private set; }
        public BodyType BodyType { get; private set; }


        public PhysicsComponent(BodyType bodyType, ColliderComponent collider)
        {
            BodyType = bodyType;
            this.collider = collider;

        }

        public override void Start()
        {
            Entity.OnSetScale += Entity_OnSetScale;
            Configure();
        }

        public override void Dispose()
        {
            Entity.OnSetScale -= Entity_OnSetScale;
            Entity.Destroy();
            base.Dispose();
        }

        void Entity_OnSetScale()
        {
            var fixture = Body.FixtureList[0];
            var d = fixture.Shape.Density;
            Body.Remove(fixture);
            var rectangleVertices = PolygonTools.CreateRectangle(collider.Bounds.Width / 2, collider.Bounds.Height / 2);
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
                collider.Bounds.Width,
                collider.Bounds.Height,
                1f, new Vector2(collider.Bounds.X + collider.Bounds.Width / 2, collider.Bounds.Y - collider.Bounds.Height / 2));

            body.SetRestitution(0.7f);
            body.SetFriction(1.5f);
            body.BodyType = BodyType;
            body.Tag = Entity.Name;
            body.OnCollision += Body_OnCollision;

            return body;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {

            Entity.Position = Body.Position - new Vector2(collider.Bounds.Width, collider.Bounds.Height) / 2;
            Entity.Rotation = Body.Rotation;

            base.Update(gameTime);
        }


    }
}
