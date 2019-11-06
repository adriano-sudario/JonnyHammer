using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JonnyHamer.Engine.Entities
{
    public class Entity : IDraw, IUpdate
    {

        private IList<IComponent> components = new List<IComponent>();
        private IList<IScalable> scalables = new List<IScalable>();
        private IList<CoroutineTask> coroutines = new List<CoroutineTask>();

        protected bool isActive = true;

        private bool runStart = false;

        private float scale = 1;
        public float Scale
        {
            get => scale; set
            {
                scale = value;

                foreach (var scalable in scalables)
                    scalable.Scale = scale;
            }
        }

        public Direction.Horizontal FacingDirection { get; set; }

        public Vector2 Position { get; set; }


        public Entity(Vector2 position,
            Direction.Horizontal facingDirection = Direction.Horizontal.Right,
            float scale = 1f)
        {
            FacingDirection = facingDirection;
            this.scale = scale;
            Position = position;

        }

        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
                components[i].Start();
        }


        public virtual void Update(GameTime gameTime)
        {
            if (!runStart)
            {
                Start();
                runStart = false;
            }

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
            CheckScalable(component);
            return component;
        }

        public T AddComponent<T>() where T : IComponent, new() => AddComponent(new T());

        void CheckScalable(IComponent component)
        {
            if (component is IScalable s)
                scalables.Add(s);
        }


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
