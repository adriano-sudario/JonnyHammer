using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Tiles
{
    public class Floor : Entity
    {
        public override void Load()
        {

            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, 1000, 30), false, true));
            AddComponent(new PhysicsComponent(BodyType.Static, collider));

            base.Load();
        }
    }
}