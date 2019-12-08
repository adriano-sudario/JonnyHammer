using JonnyHammer.Engine.Helpers;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public override void Load()
        {
            base.Load();

            Screen.SetVirtualArea(sprite.Width, sprite.Height);
        }
    }
}
