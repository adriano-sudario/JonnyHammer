using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Tiles
{
    public class Floor: Entity
    {
        public Floor(Vector2 position) : base(position)
        {
            AddComponent(new ColliderComponent(new Rectangle(100, 100, 100, 100), true));
        }
    }
}