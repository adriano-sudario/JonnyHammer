using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Game.Tiles
{
    public class Box : GameObject
    {
        private Physics physics;

        public override void Load()
        {
            var collider = AddComponent(new Collider(new Rectangle(0, 0, 20, 20), false, true, Color.Blue));
            physics = AddComponent(new Physics(BodyType.Dynamic, collider));
        }


    }
}
