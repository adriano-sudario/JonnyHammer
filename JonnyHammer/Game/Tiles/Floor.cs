using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Tiles
{
    public class Floor : Entity
    {
        public Floor()
        {
        }

        public override void Start()
        {
            base.Start();

            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, 1000, 30), false, true));
            AddComponent(new PhysicsComponent(BodyType.Static, collider));

        }
    }
}