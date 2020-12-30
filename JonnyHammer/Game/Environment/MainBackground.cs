using JonnyHammer.Engine.Helpers;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public MainBackground(string textureName)
        {
            Camera2D.SetBounds(sprite.Width, sprite.Height);
            TextureName = textureName;
        }
    }
}
