using JonnyHamer.Engine.Managers;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using JonnyHammer.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JonnyHamer.Engine.Entities
{
    public class Entity : IDraw, IUpdate, IDisposable
    {
        private IList<IComponent> components = new List<IComponent>();
        protected bool isActive = true;
        bool didStart = false;
        public Transform Transform { get; private set; } = new Transform();
        CoroutineManager coroutineManager = new CoroutineManager();

        public string Name { get; set; }
        public virtual int Width { get; set; } = 1;
        public virtual int Height { get; set; } = 1;

        public Entity() { }

        public virtual void Load() { }

        void BaseLoad()
        {
            for (var i = 0; i < components.Count; i++)
                components[i].Start();
        }

        public virtual void Update(GameTime gameTime) { }

        void IUpdate.Update(GameTime gameTime) => FullUpdate(gameTime);
        public void FullUpdate(GameTime gameTime)
        {
            BaseUpdate(gameTime);
            Update(gameTime);
        }

        private void BaseUpdate(GameTime gameTime)
        {
            if (!isActive)
                return;

            if (!didStart)
            {
                Load();
                BaseLoad();
                didStart = true;
                return;
            }

            for (var i = 0; i < components.Count; i++)
                components[i].Update(gameTime);

            coroutineManager.UpdateCoroutines(gameTime);
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
            return component;
        }

        public T AddComponent<T>() where T : IComponent, new() => AddComponent(new T());
        public T RequireComponent<T>() where T : IComponent, new() => GetComponent<T>() ?? AddComponent<T>();
        public T GetComponent<T>() where T : IComponent => components.OfType<T>().SingleOrDefault();
        public T[] GetComponents<T>() where T : IComponent => components.OfType<T>().ToArray();



        public void StartCoroutine(IEnumerator coroutine) =>
            coroutineManager.StartCoroutine(coroutine);
        public void StartCoroutine(IEnumerator coroutine, TimeSpan wait) =>
            coroutineManager.StartCoroutine(coroutine, wait);
        public void StartCoroutine(IEnumerator coroutine, int numberOfFrames) =>
            coroutineManager.StartCoroutine(coroutine, numberOfFrames);
        public void StopCoroutines() =>
            coroutineManager.StopCoroutines();
        public void StopCoroutine(IEnumerator coroutine) =>
            coroutineManager.StopCoroutine(coroutine);

        public void Invoke(Action action, TimeSpan waitFor) => StartCoroutine(waitAndRun(action, waitFor));
        IEnumerator waitAndRun(Action action, TimeSpan time)
        {
            yield return time;
            action();
        }

        public void Destroy() => SceneManager.CurrentScene.Destroy(this);
        public void Destroy(TimeSpan waitFor) => Invoke(Destroy, waitFor);

        public void Dispose()
        {
            for (int i = 0; i < components.Count; i++)
                components[i].Dispose();
        }
    }
}