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
        float force = 0.3f;
        bool isScaling = false;

        KeyboardInput keyboard;

        MoveComponent move;
        PhysicsComponent physics;
        AnimatedSpriteComponent animations;

        public BigNarutoFisica() { }
        public override void Load()
        {
            keyboard = new KeyboardInput();

            animations = AddComponent(CreateNarutaoAnimations());
            var collider = AddComponent(new ColliderComponent(new Rectangle(0, 0, animations.Width, animations.Height), true));
            move = AddComponent<MoveComponent>();
            physics = AddComponent(new PhysicsComponent(BodyType.Dynamic, collider));

            collider.OnCollide += (e) => { Console.WriteLine($"colidiu com {e.Name} {DateTime.UtcNow.Millisecond}"); };

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

            physics.Body.Mass = 1;
            if (keyboard.IsPressing(Keys.Space))
            {
                physics.Body.ApplyLinearImpulse(new Vector2(0, -1f));

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
                animations.Change("Idle");


        }

        public void Run(Direction.Horizontal direction)
        {
            animations.Change("Running");

            if (FacingDirection != direction)
                physics.Body.LinearVelocity = new Vector2(0, physics.Body.LinearVelocity.Y);

            FacingDirection = direction;
            var m = new Vector2(direction == Direction.Horizontal.Left ? -force : force, 0);
            physics.Body.ApplyLinearImpulse(m * physics.Body.Mass);

        }


    }
}
