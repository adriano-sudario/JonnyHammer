using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components;
using Chamboco.Engine.Entities.Components.Collider;
using Chamboco.Engine.Entities.Components.Physics;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Chamboco.Engine.Inputs;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : GameObject
    {
        private float speed = 2f;

        private KeyboardInput keyboard;
        private AnimationRenderer animatedSprite;

        public float HorizontalPosition { get; set; }
        public float MoveAmount { get; set; }

        public int Height => animatedSprite.Height;
        public int Width => animatedSprite.Width;

        public BigNaruto(float moveAmount, Vector2 position)
        {
            MoveAmount = moveAmount;
            keyboard = new KeyboardInput();

            var animations = CreateNarutaoAnimations();
            animations.Color = Color.Red;
            animatedSprite = AddComponent(animations);

            var collider = AddComponent(new Collider(animatedSprite, true, true, Color.Purple));
            //AddComponent(new PhysicsComponent(BodyType.Dynamic, collider, mass: 10, friction: 0.2f)); ;
            AddComponent<SlimPhysics>();

            Transform.Scale = 0.6f;

            collider.OnTrigger += Collider_OnCollide;

            Transform.MoveTo(position);
            HorizontalPosition = Transform.X;
            AddComponent(
                new Tween(TweenMode.Loop,
                    () => HorizontalPosition,
                    v => HorizontalPosition = v,
                HorizontalPosition + MoveAmount,
                    EaseFunction.Linear, 1000));

        }

        private void Collider_OnCollide(GameObject obj)
        {
            if (obj is Jonny jonnny)
            {
                jonnny.TakeDamage(25, Transform.Position);
            }
        }

        AnimationRenderer CreateNarutaoAnimations()
        {
            var spriteSheet = Loader.LoadTexture("narutao");
            var animationFrames = Loader.LoadAsepriteFrames("narutao");

            return new AnimationRenderer(spriteSheet, animationFrames);
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard.Update();
            Transform.MoveHorizontally((int)HorizontalPosition);
        }

        public void Run(Direction.Horizontal direction)
        {
            animatedSprite.Change("Running");
            Transform.MoveAndSlide(new Vector2(
                direction == Direction.Horizontal.Left ? -speed : speed, 0));
        }
    }
}
