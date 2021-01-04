using JonnyHamer.Engine.Entities;
using Chamboco.Engine.Entities.Components.Collider;
using Chamboco.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Tiles
{
    public class Block : GameObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Block(string name, int width, int height): base(name)
        {
            Width = width;
            Height = height;
            
            var debugCollor = Color.Red;
            debugCollor.A = 50;

            var collider = AddComponent(new Collider(new Rectangle(0, 0, Width, Height), false, true, debugCollor));
            AddComponent(new Physics(BodyType.Static, collider));
        }
        
    }
}