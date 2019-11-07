using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Tiles
{
    public class Floor: Entity
    {
        public Floor() 
        {
            AddComponent(new ColliderComponent(new Rectangle(0, 0, 100, 100), false, true));
        }
    }
}