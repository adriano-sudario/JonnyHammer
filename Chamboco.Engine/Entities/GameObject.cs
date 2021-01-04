using Chamboco.Engine.Entities.Components;
using Chamboco.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chamboco.Engine.Entities
{
    public class GameObject : IDisposable
    {
        IList<Component> components = new List<Component>();
        protected bool isActive = true;
        public Transform Transform { get; private set; } = new();
        CoroutineManager coroutineManager = new();

        bool didLoad;
        public string Name { get; set; }

        public GameObject(string name) => Name = name;

        protected GameObject() { }

        protected void LoadComponents()
        {
            for (var i = 0; i < components.Count; i++)
                components[i].Start();
            didLoad = true;
        }
        protected virtual void Update(GameTime gameTime) { }

        public void UpdateObject(GameTime gameTime)
        {
            BaseUpdate(gameTime);
            Update(gameTime);
        }

        void BaseUpdate(GameTime gameTime)
        {
            if (!isActive)
                return;

            if (!didLoad)
                LoadComponents();
            for (var i = 0; i < components.Count; i++)
                components[i].Update(gameTime);

            coroutineManager.UpdateCoroutines(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isActive)
                return;

            for (var i = 0; i < components.Count; i++)
                components[i].Draw(spriteBatch);
        }

        public T AddComponent<T>(T component) where T : Component
        {
            component.SetEntity(this);
            components.Add(component);
            return component;
        }

        public T AddComponent<T>() where T : Component, new() => AddComponent(new T());
        public T RequireComponent<T>() where T : Component, new() => GetComponent<T>() ?? AddComponent<T>();
        public T GetComponent<T>() where T : Component => components.OfType<T>().SingleOrDefault();
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = components.OfType<T>().SingleOrDefault();
            return component != null;
        }

        public T[] GetComponents<T>() where T : Component => components.OfType<T>().ToArray();

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
