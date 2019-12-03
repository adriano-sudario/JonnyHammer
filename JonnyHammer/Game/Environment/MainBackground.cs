using JonnyHamer.Engine.Manipulators;

namespace JonnyHammer.Game.Environment
{
    public class MainBackground : Scenery
    {
        public override void Load()
        {
            base.Load();

            Camera.AreaWidth = sprite.Width;
            Camera.AreaHeight = sprite.Height;
        }
    }
}
