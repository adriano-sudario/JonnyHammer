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
    public class Jonny : Entity
    {
        enum State
        {
            Grounded,
            Jumping,
            Dashing,
            HitStun,
        }

        int totalLife = 100;
        int life = 0;
        bool locked = false;
        bool invulnerable = false;


        const float JumpForce = 2.5f;
        float speed = 2f;
        bool isScaling = false;
        bool canDash = true;
        State state = State.Jumping;

        KeyboardInput keyboard;
        PhysicsComponent physics;
        AnimatedSpriteComponent animations;

        public Vector2 RespawnPosition { get; set; }

        public override void Load()
        {
            life = totalLife;
            keyboard = new KeyboardInput();

            animations = AddComponent(CreateAnimations());

            var debugColor = Color.Green;
            debugColor.A = 90;
            var collider = AddComponent(
                            new ColliderComponent(
                                   new Rectangle(0, 0, animations.Width, animations.Height),
                                   autoCheck: true,
                                   isDebug: true,
                                   debugColor: debugColor));

            var floorTrigger = AddComponent(
                                new ColliderComponent(
                                    new Rectangle(5, animations.Height, animations.Width - 10, 5),
                                    autoCheck: true,
                                    isDebug: true,
                                    isTrigger: true,
                                    debugColor: Color.Yellow));

            collider.OnCollide += e =>
            {
                //Console.WriteLine($"colidiu com {e.Name} {DateTime.UtcNow.Millisecond}");
            };

            floorTrigger.OnTrigger += e =>
            {
                if (state != State.Dashing && e.Name.Contains("floor") && !locked)
                {
                    state = State.Grounded;
                    canDash = true;
                }
            };

            physics = AddComponent(new PhysicsComponent(BodyType.Dynamic, collider, mass: 1));
            physics.MaxVelocity = new Vector2(3, JumpForce);
        }

        public void TakeDamage(int amount, Vector2 reference)
        {
            if (state == State.HitStun || invulnerable)
                return;

            life -= amount;
            StartCoroutine(Blink());
            StartCoroutine(HitStun());

            physics.ResetVelocity();

            var sideModifier = Transform.X <= reference.X ? -1 : 1;
            physics.ApplyForce(new Vector2(3f, 1f) * sideModifier);
        }

        IEnumerator Scale()
        {
            yield return new WaitUntil(() => !isScaling);
            isScaling = true;

            while (Transform.Scale < 3)
            {
                Transform.Scale += 0.01f;
                yield return null; // wait 1 frame
            }

            while (Transform.Scale > 1)
            {
                Transform.Scale -= 0.01f;
                yield return null;
            }

            isScaling = false;
        }

        IEnumerator HitStun()
        {
            var oldState = state;
            locked = true;
            state = State.HitStun;

            yield return 10;

            state = oldState;
            locked = false;
        }
        IEnumerator Blink()
        {
            invulnerable = true;
            for (var i = 0; i < 7; i++)
            {
                animations.IsVisible = !animations.IsVisible;
                yield return 5;
            }

            animations.IsVisible = true;
            invulnerable = false;
        }

        AnimatedSpriteComponent CreateAnimations()
        {
            var spriteSheet = Loader.LoadTexture("main_char_spritesheet");
            var animationFrames = Loader.LoadAsepriteFrames("main_char_sprite_data");

            return new AnimatedSpriteComponent(spriteSheet, animationFrames);
        }

        public void Respawn()
        {
            life = totalLife;
            Transform.MoveTo(RespawnPosition);
            isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (life == 0)
                Respawn();

            if (state == State.Dashing)
            {
                physics.ApplyForce(new Vector2((Transform.FacingDirection == Direction.Horizontal.Left ? -.5f : .5f) * 2.8f, 0)); //-SceneManager.CurrentScene.World.Gravity.Y));
                return;
            }

            HandleInput();
        }

        private void HandleInput()
        {
            if (locked)
                return;

            keyboard.Update();

            if (keyboard.IsPressing(Keys.LeftControl))
                return;

            if (keyboard.IsPressing(Keys.Space) && state == State.Grounded)
            {
                physics.ApplyForce(new Vector2(0, -JumpForce));
                state = State.Jumping;
            }

            if (keyboard.HasPressed(Keys.S))
                StartCoroutine(Scale());

            if (keyboard.HasPressed(Keys.Escape))
                Respawn();


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
            if (state == State.Dashing || !canDash)
                return;

            canDash = false;
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
            Transform.FacingDirection = direction;

            if (Transform.FacingDirection != direction) Stop();

            var m = direction == Direction.Horizontal.Left ? -speed : speed;
            physics.MoveForward(m);
        }
    }
}
