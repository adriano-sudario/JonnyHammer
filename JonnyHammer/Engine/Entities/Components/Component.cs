using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace JonnyHammer.Engine
{
    public class Component : IComponent
    {
        public GameObject Entity { get; private set; }
        public bool IsActive { get; set; }

        public Direction.Horizontal FacingDirection { get => Entity.Transform.FacingDirection; set => Entity.Transform.FacingDirection = value; }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void SetEntity(GameObject entity) => Entity = entity;

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }

        public T AddComponent<T>(T component) where T : IComponent => Entity.AddComponent(component);
        public T AddComponent<T>() where T : IComponent, new() => Entity.AddComponent<T>();
        public Lazy<T> GetComponent<T>() where T : IComponent => new (() => Entity.GetComponent<T>());
        public void StartCoroutine(IEnumerator coroutine) => Entity.StartCoroutine(coroutine);
        public void StopCoroutine(IEnumerator coroutine) => Entity.StopCoroutine(coroutine);
        public void StopCoroutines() => Entity.StopCoroutines();
        public void Invoke(Action action, TimeSpan waitFor) => Entity.Invoke(action, waitFor);

        public virtual void Dispose() { }
    }
}
