using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Manipulators;
using Microsoft.Xna.Framework;
using System;

namespace JonnyHammer.Game.Environment
{
    public class Cloud : Entity
    {
        private SpriteComponent sprite;

        public Action OnDisappear { get; set; }
        public Vector2 Spawn { get; set; }
        public float Speed { get; set; }
        public override int Height { get => sprite.Height; }
        public override int Width { get => sprite.Width; }

        public override void Load()
        {
            base.Load();

            sprite = new SpriteComponent(Loader.LoadTexture("cloud"));
            AddComponent(sprite);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Transform.MoveAndSlideHorizontally(Speed, false);

            if ((Speed < 0 && Transform.X + Width < 0) || (Speed > 0 && Transform.X > Camera.AreaWidth))
                OnDisappear?.Invoke();
        }
    }
}
