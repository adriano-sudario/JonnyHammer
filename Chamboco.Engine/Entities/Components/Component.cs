using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chamboco.Engine.Entities.Components
{
    public class Component : IDisposable
    {
        public GameObject Entity { get; private set; }
        public bool IsActive { get; set; } = true;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void SetEntity(GameObject entity) => Entity = entity;

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }

        public Lazy<T> GetComponent<T>() where T : Component => new (() => Entity.GetComponent<T>());

        public virtual void Dispose() { }
    }
}
