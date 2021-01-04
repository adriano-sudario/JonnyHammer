using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Environment
{
    public class Cloud : GameObject
    {
        private SpriteRenderer sprite;

        public Vector2 Spawn { get; set; }
        public float Speed { get; set; }
        public int Height { get => sprite.Height; }
        public int Width { get => sprite.Width; }
        public Vector2 Cloudrespawn { get; set; }

        public Cloud(in float tileSpeed, in Vector2 cloudRespawn)
        {
            Speed = tileSpeed;
            Cloudrespawn = cloudRespawn;
            sprite = new SpriteRenderer(Loader.LoadTexture("cloud"));
            AddComponent(sprite);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Transform.MoveAndSlideHorizontally(Speed, false);

            if ((Speed < 0 && Transform.X + Width < 0) || (Speed > 0 && Transform.X > Screen.VirtualWidth))
                Transform.MoveTo(Cloudrespawn);
        }
    }
}
