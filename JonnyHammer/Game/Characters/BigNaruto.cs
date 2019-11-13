using Caieta.Components.Utils;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Inputs;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities.Components;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : Entity
    {
        float speed = 3f;
        bool isScaling = false;

        KeyboardInput keyboard;

        MoveComponent move;
        private SlimPhysicsComponent platform;
        AnimatedSpriteComponent animations;

        public float Teste { get; set; }

        public BigNaruto()
        {
            keyboard = new KeyboardInput();

            animations = AddComponent(CreateNarutaoAnimations());
            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, animations.Width, animations.Height), true));
            move = AddComponent<MoveComponent>();
            platform = AddComponent<SlimPhysicsComponent>();

            collider.OnCollide += (e) => { Console.WriteLine($"colidiu com {e.Name} {DateTime.UtcNow.Millisecond}"); };

        }

        public override void Load()
        {
            base.Load();

            Teste = Position.X;
            //AddComponent(new TweenComponent(TweenMode.Loop, TweenProperty.X, Position.X + 200, EaseFunction.Linear, 1000));
            AddComponent(new TweenComponent(TweenMode.Loop, this, "Teste", Teste + 200, EaseFunction.Linear, 1000));
        }

        IEnumerator ScaleNaruto()
        {
            yield return new WaitUntil(() => !isScaling);
            isScaling = true;

            while (Scale < 3)
            {
                Scale += 0.01f;
                move.MoveAndSlide(0, -1);
                yield return null; // wait 1 frame
            }

            while (Scale > 1)
            {
                Scale -= 0.01f;
                yield return null;
            }

            isScaling = false;
        }

        IEnumerator BlinkNaruto()
        {

            for (var i = 0; i < 30; i++)
            {
                animations.IsVisible = !animations.IsVisible;
                yield return 5; // wait 5 frames
            }

            animations.IsVisible = true;
        }
        AnimatedSpriteComponent CreateNarutaoAnimations()
        {
            var spriteSheet = Loader.LoadTexture("narutao");
            var animationFrames = Loader.LoadAnimation("narutao");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();

            if (keyboard.IsPressing(Keys.Space))
                platform.AddForce(new Vector2(0, 4));

            if (keyboard.HasPressed(Keys.S))
            {
                StartCoroutine(ScaleNaruto());
            }

            if (keyboard.HasPressed(Keys.A))
                StartCoroutine(BlinkNaruto());

            if (keyboard.IsPressing(Keys.Right))
                Run(Direction.Horizontal.Right);
            else if (keyboard.IsPressing(Keys.Left))
                Run(Direction.Horizontal.Left);
            else
                animations.Change("Running");

            move.MoveHorizontally((int)Teste);

            base.Update(gameTime);
        }

        public void Run(Direction.Horizontal direction)
        {
            animations.Change("Running");
            move.MoveAndSlide(new Vector2(
                direction == Direction.Horizontal.Left ? -speed : speed, 0));
        }


    }
}
