using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : Entity
    {
        float speed = 3f;
        MoveComponent move;

        public AnimatedSpriteComponent Animation { get; private set; }

        public BigNaruto(Vector2 position) : base(position)
        {
            move = AddComponent<MoveComponent>();
            Animation = AddComponent(CreateNarutaoAnimations());
        }

        static AnimatedSpriteComponent CreateNarutaoAnimations()
        {
            var spriteSheet = Loader.LoadTexture("narutao");
            var animationFrames = Loader.LoadAnimation("narutao");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public void Run(Direction.Horizontal direction)
        {
            Animation.Change("Running");
            move.MoveAndSlide(new Vector2(
                direction == Direction.Horizontal.Left ? -speed : speed, 0));
        }


    }
}
