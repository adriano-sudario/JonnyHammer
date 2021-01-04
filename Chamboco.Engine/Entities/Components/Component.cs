using JonnyHamer.Engine.Entities;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace Chamboco.Engine
{
    public class Component : IDisposable
    {
        public GameObject Entity { get; private set; }
        public bool IsActive { get; set; }

        public Direction.Horizontal FacingDirection { get => Entity.Transform.FacingDirection; set => Entity.Transform.FacingDirection = value; }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void SetEntity(GameObject entity) => Entity = entity;

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }

        public Lazy<T> GetComponent<T>() where T : Component => new (() => Entity.GetComponent<T>());

        public virtual void Dispose() { }
    }
}
