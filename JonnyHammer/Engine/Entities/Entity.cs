﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using JonnyHammer.Engine.Entities.Components;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHamer.Engine.Entities
{
    public class Entity : IDraw, IUpdate
    {
        private IList<IComponent> components = new List<IComponent>();
        private IList<CoroutineTask> coroutines = new List<CoroutineTask>();
        protected bool isActive = true;

        public float Scale { get; set; } = 1;
        public Vector2 Position { get; set; }
        public Direction.Horizontal FacingDirection { get; set; }
        public Entity(Vector2 position,
            Direction.Horizontal facingDirection = Direction.Horizontal.Right,
            float scale = 1f)
        {
            FacingDirection = facingDirection;
            Scale = scale;
            Position = position;

        }

        public virtual void Update(GameTime gameTime)
        {
           if (!isActive)
                return;

           for (var i = 0; i < components.Count; i++)
                components[i].Update(gameTime);

           UpdateCoroutines(gameTime);

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isActive)
                return;

            for (int i = 0; i < components.Count; i++)
                components[i].Draw(spriteBatch);
        }

        public T AddComponent<T>(T component) where T : IComponent
        {
            component.SetEntity(this);
            components.Add(component);
            component.Start();
            return component;
        }

        public T AddComponent<T>() where T : IComponent, new() => AddComponent(new T());

        public T GetComponent<T>() where T : IComponent => components.OfType<T>().FirstOrDefault();
        public T[] GetComponents<T>() where T : IComponent => components.OfType<T>().ToArray();


        void UpdateCoroutines(GameTime gameTime)
        {
            for (var i = 0; i < coroutines.Count; i++)
            {
                var coroutine = coroutines[i];
                if (coroutine.Done)
                {
                    coroutines.Remove(coroutine);
                    continue;
                }

                coroutine.Update(gameTime);
            }
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            var c = new CoroutineTask(coroutine, null);
            coroutines.Add(c);
        }

        public void StartCoroutine(IEnumerator coroutine, TimeSpan wait)
        {
            var c = new CoroutineTask(coroutine, new WaitTime(wait));
            coroutines.Add(c);
        }

        public void StartCoroutine(IEnumerator coroutine, int numberOfFrames)
        {
            var c = new CoroutineTask(coroutine, new WaitFrames(numberOfFrames));
            coroutines.Add(c);
        }

        public void StopCoroutines() => coroutines.Clear();
        public void Invoke(Action action, TimeSpan waitFor) => StartCoroutine(waitAndRun(action, waitFor));
        IEnumerator waitAndRun(Action action, TimeSpan time)
        {
            yield return time;
            action();
        }

    }
}