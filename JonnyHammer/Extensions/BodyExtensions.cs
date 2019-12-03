using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer
{
    public static class BodyExtensions
    {
        public static void MoveAndSlide(this Body body, Vector2 addPosition)
        {
            body.SetTransform(body.Position + (addPosition / Physics.PixelsPerMeter), 0);
        }
    }
}
