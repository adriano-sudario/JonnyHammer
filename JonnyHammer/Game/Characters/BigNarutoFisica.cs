using Caieta.Components.Utils;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Inputs;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities.Components;
using JonnyHammer.Engine.Entities.Components.Collider;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Game.Characters
{
    public class BigNarutoFisica : Entity
    {
        enum State
        {
            Grounded,
            Jumping,
            Dashing,
        }

        float speed = 3f;
        bool isScaling = false;
        State state = State.Jumping;

        KeyboardInput keyboard;

        PhysicsComponent physics;
        AnimatedSpriteComponent animations;



        public BigNarutoFisica() { }


        public override void Load()
        {
            keyboard = new KeyboardInput();

            animations = AddComponent(CreateNarutaoAnimations());

            var debugColor = Color.Green;
            debugColor.A = 90;
            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, animations.Width, animations.Height), autoCheck: true, isDebug: true, debugColor));

            var floorTrigger = AddComponent(new ColliderComponent(new Rectangle(5, animations.Height, animations.Width - 10, 10), autoCheck: true, isDebug: true, Color.Yellow));
            floorTrigger.IsTrigger = true;
            //AddComponent(new TweenComponent(TweenMode.Loop, TweenProperty.Scale, 2, EaseFunction.BigBackInOut, 1000));

            collider.OnCollide += (e) => { Console.WriteLine($"colidiu com {e.Name} {DateTime.UtcNow.Millisecond}"); };


            floorTrigger.OnTrigger += (e) =>
            {
                Console.WriteLine($"pisou no chao");

                if (state != State.Dashing)
                    state = State.Grounded;
            };


            physics = AddComponent(new PhysicsComponent(BodyType.Dynamic, collider, mass: 1));
        }

        IEnumerator ScaleNaruto()
        {
            yield return new WaitUntil(() => !isScaling);
            isScaling = true;

            while (Scale < 3)
            {
                Scale += 0.01f;
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
            var animationFrames = Loader.LoadAsepriteFrames("narutao");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (state == State.Dashing)
            {
                physics.ApplyForce(new Vector2((FacingDirection == Direction.Horizontal.Left ? -1 : 1) * 2.8f, 0)); //-SceneManager.CurrentScene.World.Gravity.Y));
                return;
            }

            keyboard.Update();

            if (keyboard.IsPressing(Keys.Space) && state != State.Jumping)
            {
                physics.ApplyForce(new Vector2(0, -1f));
                state = State.Jumping;
                Console.WriteLine("Pulou!!!!");
            }

            if (keyboard.HasPressed(Keys.S))
                StartCoroutine(ScaleNaruto());

            if (keyboard.HasPressed(Keys.A))
                StartCoroutine(BlinkNaruto());


            if (keyboard.IsPressing(Keys.Right))
                Run(Direction.Horizontal.Right);
            else if (keyboard.IsPressing(Keys.Left))
                Run(Direction.Horizontal.Left);
            else
            {
                animations.Change("Idle");
                if (physics.Velocity.X != 0)
                    physics.SetVelocity(x: physics.Velocity.X * .8f, noconvert: true);
            }

            if (keyboard.HasPressed(Keys.LeftShift))
                Dash();

        }


        void Dash()
        {
            if (state == State.Dashing)
                return;

            var oldState = state;
            state = State.Dashing;
            physics.Body.IgnoreGravity = true;
            physics.SetVelocity(y: 0);
            animations.Change("Running");
            Invoke(() =>
            {
                state = oldState;
                physics.Body.IgnoreGravity = false;

            }, TimeSpan.FromSeconds(0.15));
        }

        void Stop() => physics.SetVelocity(x: 0);

        public void Run(Direction.Horizontal direction)
        {
            animations.Change("Running");
            FacingDirection = direction;

            if (FacingDirection != direction) Stop();

            var m = direction == Direction.Horizontal.Left ? -speed : speed;
            physics.MoveForward(m);
        }


    }
}
