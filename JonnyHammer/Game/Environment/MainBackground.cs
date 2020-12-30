using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public MainBackground(string textureName) : base(textureName, Vector2.Zero, 0, 0)
        {
            Camera2D.SetBounds(sprite.Width, sprite.Height);
        }
        
    }
}
