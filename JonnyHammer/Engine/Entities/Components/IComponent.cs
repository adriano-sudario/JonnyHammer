using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Interfaces;
using System;
using System.Collections;

namespace JonnyHammer.Engine
{
    public interface IComponent : IDraw, IUpdate, IStartable, IDisposable
    {
        Entity Entity { get; }
        void SetEntity(Entity entity);

        public T GetComponent<T>() where T : IComponent => Entity.GetComponent<T>();
        public T[] GetComponents<T>() where T : IComponent => Entity.GetComponents<T>();
        void StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(IEnumerator coroutine);
        void StopCoroutines();
        void Invoke(Action action, TimeSpan waitFor);
    }
}
