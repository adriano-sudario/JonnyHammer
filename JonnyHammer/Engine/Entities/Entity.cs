using JonnyHammer.Engine;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace JonnyHamer.Engine.Entities
{
    public class Entity : IDraw, IUpdate
    {

        private IList<IComponent> components = new List<IComponent>();

        protected bool isActive = true;

        private bool runStart = false;

        public float Scale { get; set; } = 1;

        public Direction.Horizontal FacingDirection { get; set; }

        public Vector2 Position { get; set; }


        public Entity(Vector2 position,
            Direction.Horizontal facingDirection = Direction.Horizontal.Right,
            float scale = 1f)
        {
            FacingDirection = facingDirection;
            Scale = scale;
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


            for (int i = 0; i < components.Count; i++)
                components[i].Update(gameTime);

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

        public T AddComponent<T>() where T : IComponent, new()
        {
            var component = new T();
            component.SetEntity(this);
            components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : IComponent => components.OfType<T>().FirstOrDefault();
        public T[] GetComponents<T>() where T : IComponent => components.OfType<T>().ToArray();

    }
}
