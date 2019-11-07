using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Tiles
{
    public class Floor : Entity
    {
        public Floor()
        {
            AddComponent(new ColliderComponent(new Rectangle(0, 0, 1000, 30), false, true));
        }
    }
}