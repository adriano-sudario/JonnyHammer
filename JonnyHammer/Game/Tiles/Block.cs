using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Tiles
{
    public class Block : Entity
    {

        public override void Load()
        {
            var debugCollor = Color.Red;
            debugCollor.A = 50;
            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, Width, Height), false, true, debugCollor));
            AddComponent(new PhysicsComponent(BodyType.Static, collider));

            base.Load();
        }
    }
}