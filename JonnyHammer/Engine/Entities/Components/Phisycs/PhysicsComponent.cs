using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Entities.Components.Collider;
using Microsoft.Xna.Framework;
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
            Configure();
        }

        void Configure()
        {
            Body = SceneManager.CurrentScene.World.CreateRectangle(
                collider.Bounds.Width,
                collider.Bounds.Height,
                1f, new Vector2(collider.Bounds.X + collider.Bounds.Width / 2, collider.Bounds.Y - collider.Bounds.Height / 2));

            collider.IsTrigger = true;
            collider.AutoCheck = false;

            Body.SetRestitution(0.7f);
            Body.SetFriction(0.5f);
            Body.BodyType = BodyType;
        }


        public override void Update(GameTime gameTime)
        {

            Entity.Position = Body.Position - new Vector2(collider.Bounds.Width, collider.Bounds.Height) / 2;
            Entity.Rotation = Body.Rotation;

            base.Update(gameTime);
        }


    }
}
