﻿using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Engine
{
    public class Component : IComponent
    {
        public Component()
        {

        }

        public Entity Entity { get; private set; }

        public Vector2 Position { get => Entity.Position; set => Entity.Position = value; }
        public Direction.Horizontal FacingDirection { get => Entity.FacingDirection; set => Entity.FacingDirection = value; }
        public float Scale { get => Entity.Scale; set => Entity.Scale = value; }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void SetEntity(Entity entity) => Entity = entity;

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }
        public T GetComponent<T>() where T : IComponent => Entity.GetComponent<T>();
        public T[] GetComponents<T>() where T : IComponent => Entity.GetComponents<T>();

    }
}