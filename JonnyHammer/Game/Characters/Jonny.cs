﻿using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components.Physics;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Inputs;
using Chamboco.Engine.Managers;
using JonnyHammer.Game.Components;
using JonnyHammer.Game.Tiles;

namespace JonnyHammer.Game.Characters
{
    using Chamboco.Engine.Entities.Components.Collider;
    using Chamboco.Engine.Helpers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections;
    using tainicom.Aether.Physics2D.Dynamics;

    public class Jonny : GameObject
    {
        enum State
        {
            Grounded,
            Jumping,
            Dashing,
            HitStun,
        }

        int totalLife = 100;
        int life;
        bool locked;
        bool invulnerable;


        const float JumpForce = 4f;
        float speed = 2f;
        bool touchGround = true;
        bool canDash = true;
        State state = State.Jumping;

        KeyboardInput keyboard = new ();
        Physics physics;
        AnimationRenderer spriteRenderer;
        Lifebar lifebar;

        public Vector2 RespawnPosition { get; set; }

        public Jonny(Vector2 respawnPosition) : this()
        {
            RespawnPosition = respawnPosition;

        }

        public Jonny()
        {
            life = totalLife;

            spriteRenderer = AddComponent(CreateAnimations());

            lifebar = AddComponent(new Lifebar(totalLife));

            var debugColor = Color.Green;
            debugColor.A = 90;
            var collider = AddComponent(
                new Collider(
                    new Rectangle(0, 0, spriteRenderer.Width, spriteRenderer.Height),
                    autoCheck: true,
                    isDebug: true,
                    debugColor: debugColor));

            physics = AddComponent(new Physics(BodyType.Dynamic, collider, mass: 1));
            physics.MaxVelocity = new Vector2(3, JumpForce);
            physics.UseMaxVelocityY = true;

            var floorTrigger = AddComponent(
                new Collider(
                    new Rectangle(10, spriteRenderer.Height, spriteRenderer.Width - 20, 5),
                    autoCheck: true,
                    isDebug: true,
                    isTrigger: true,
                    debugColor: Color.Yellow));

            floorTrigger.OnTrigger += e =>
            {
                if (state == State.Dashing || !e.Name.Contains("floor") || locked) return;
                state = State.Grounded;
                touchGround = true;
                physics.SetVelocity(velocityY: 0);
            };
        }

        public void TakeDamage(int amount, Vector2 reference)
        {
            if (state == State.HitStun || invulnerable)
                return;

            life -= amount;
            lifebar.UpdateLife(life);
            physics.ResetVelocity();

            StartCoroutine(Blink());
            StartCoroutine(HitStun());

            var sideModifier = Transform.X <= reference.X ? -1 : 1;
            physics.ApplyForce(new Vector2(5f * sideModifier, (state != State.Jumping ? -1f : -2f)));
        }

        IEnumerator HitStun()
        {
            locked = true;
            state = State.HitStun;

            yield return 10;

            state = State.Jumping;
            locked = false;
        }
        IEnumerator Blink()
        {
            invulnerable = true;
            for (var i = 0; i < 7; i++)
            {
                spriteRenderer.IsVisible = !spriteRenderer.IsVisible;
                yield return 5;
            }

            spriteRenderer.IsVisible = true;
            invulnerable = false;
        }

        AnimationRenderer CreateAnimations()
        {
            var spriteSheet = Loader.LoadTexture("main_char_spritesheet");
            var animationFrames = Loader.LoadAsepriteFrames("main_char_sprite_data");

            return new AnimationRenderer(spriteSheet, animationFrames);
        }

        public void Respawn()
        {
            state = State.Jumping;
            locked = false;
            life = totalLife;
            lifebar.UpdateLife(life);
            physics.Body.IgnoreGravity = false;
            physics.ResetVelocity();
            isActive = true;
            Transform.MoveTo(RespawnPosition);
        }

        protected override void Update(GameTime gameTime)
        {
            if (life == 0)
                Respawn();

            if (state == State.Dashing)
            {
                physics.ApplyForce(new Vector2((Transform.FacingDirection == Direction.Horizontal.Left ? -1f : 1f) * 0.8f, 0));
                return;
            }

            HandleInput();

            physics.Body.Rotation = MathHelper.Clamp(physics.Body.Rotation, -.1f, .1f);
        }

        private void HandleInput()
        {
            if (keyboard.HasPressed(Keys.Escape))
                Respawn();

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

            if (keyboard.IsPressing(Keys.Right))
                Run(Direction.Horizontal.Right);
            else if (keyboard.IsPressing(Keys.Left))
                Run(Direction.Horizontal.Left);
            else
            {
                spriteRenderer.Change("Idle");
                if (physics.Velocity.X != 0)
                    physics.SetVelocity(velocityX: physics.Velocity.X * .8f, noConvert: true);
            }

            if (keyboard.HasPressed(Keys.LeftShift))
                Dash();
        }

        void Dash()
        {
            if (state == State.Dashing || !touchGround || !canDash)
                return;

            touchGround = canDash = false;
            var oldState = state;
            state = State.Dashing;
            physics.Body.IgnoreGravity = true;
            physics.SetVelocity(velocityY: 0);
            spriteRenderer.Change("Running");
            Invoke(() =>
            {
                state = oldState;
                physics.Body.IgnoreGravity = false;

            }, TimeSpan.FromSeconds(0.15));
            Invoke(() => canDash = true, TimeSpan.FromMilliseconds(300));
        }

        void Stop() => physics.SetVelocity(velocityX: 0);

        public void Run(Direction.Horizontal direction)
        {
            spriteRenderer.Change("Running");
            Transform.FacingDirection = direction;

            if (Transform.FacingDirection != direction) Stop();

            var m = direction == Direction.Horizontal.Left ? -speed : speed;
            physics.MoveForward(m);
        }

    }
}
