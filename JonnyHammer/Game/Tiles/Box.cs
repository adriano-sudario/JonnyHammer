using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Game.Tiles
{
    public class Box : GameObject
    {
        public Box()
        {
            var collider = AddComponent(new Collider(new Rectangle(0, 0, 20, 20), false, true, Color.Blue));
            AddComponent(new Physics(BodyType.Dynamic, collider));
        }
    }
}
