using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Inputs;
using JonnyHammer.Engine;
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


            collider.OnCollide += (e) => { Console.WriteLine($"colidiu com {e.Name} {DateTime.UtcNow.Millisecond}"); };


            floorTrigger.OnTrigger += (e) =>
            {
                Console.WriteLine($"pisou no chao");
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
            var animationFrames = Loader.LoadAnimation("narutao");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyboard.Update();

            if (keyboard.IsPressing(Keys.Space) && state != State.Jumping)
            {
                physics.ApplyForce(new Vector2(0, -1f));
                state = State.Jumping;
                Console.WriteLine("Pulou!!!!");
            }

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
            {
                animations.Change("Idle");
                if (physics.Velocity.X != 0) Stop();
            }

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
