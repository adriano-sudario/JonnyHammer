using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Tiles
{
    public class Block : Entity
    {
        private int width;
        private int height = 30;
        public override int Width { get => width; set => width = value; }
        public override int Height { get => height; set => height = value; }

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