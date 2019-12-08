using JonnyHammer.Engine.Helpers;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public override void Load()
        {
            base.Load();

            Camera2D.SetBounds(sprite.Width, sprite.Height);
        }
    }
}
