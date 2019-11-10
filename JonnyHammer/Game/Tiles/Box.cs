using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Game.Tiles
{
    public class Box : Entity
    {
        private PhysicsComponent physics;

        public override void Load()
        {
            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, 20, 20), false, true, Color.Blue));
            physics = AddComponent(new PhysicsComponent(BodyType.Dynamic, collider));
        }


        public override void Update(GameTime gameTime)
        {
            physics.Body.Mass = 0.2f;
        }

    }
}
