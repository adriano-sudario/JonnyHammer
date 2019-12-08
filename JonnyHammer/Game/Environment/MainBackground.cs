using JonnyHammer.Engine;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public override void Load()
        {
            base.Load();

            Core.Instance.ResolutionIndependence.SetVirtualArea(sprite.Width, sprite.Height);
        }
    }
}
