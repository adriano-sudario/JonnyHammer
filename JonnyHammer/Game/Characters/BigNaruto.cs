using Caieta.Components.Utils;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Inputs;
using JonnyHammer.Engine.Entities.Components;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : Entity
    {
        private float speed = 2f;

        private KeyboardInput keyboard;
        private AnimatedSpriteComponent animatedSprite;

        public float HorizontalPosition { get; set; }
        public float MoveAmount { get; set; }

        public override int Height { get => animatedSprite.Height; }
        public override int Width { get => animatedSprite.Width; }

        public BigNaruto()
        {
            keyboard = new KeyboardInput();

            var animations = CreateNarutaoAnimations();
            animations.Color = Color.Red;
            animatedSprite = AddComponent(animations);

            var collider = AddComponent(new ColliderComponent(animatedSprite, true, true, Color.Purple));
            //AddComponent(new PhysicsComponent(BodyType.Dynamic, collider, mass: 10, friction: 0.2f)); ;
            AddComponent<SlimPhysicsComponent>();

            Transform.Scale = 0.6f;

            collider.OnTrigger += Collider_OnCollide;
        }

        private void Collider_OnCollide(Entity obj)
        {
            if (obj is Jonny jonnny)
            {
                jonnny.TakeDamage(25, Transform.Position);
            }
        }

        public override void Load()
        {
            base.Load();

            HorizontalPosition = Transform.X;
            AddComponent(new TweenComponent(TweenMode.Loop, this, nameof(HorizontalPosition),
                HorizontalPosition + MoveAmount, EaseFunction.Linear, 1000));
        }

        AnimatedSpriteComponent CreateNarutaoAnimations()
        {
            var spriteSheet = Loader.LoadTexture("narutao");
            var animationFrames = Loader.LoadAsepriteFrames("narutao");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();
            Transform.MoveHorizontally((int)HorizontalPosition);
            base.Update(gameTime);
        }

        public void Run(Direction.Horizontal direction)
        {
            animatedSprite.Change("Running");
            Transform.MoveAndSlide(new Vector2(
                direction == Direction.Horizontal.Left ? -speed : speed, 0));
        }
    }
}
