using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
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

        private Rectangle? customCollision;
        private IList<IComponent> components = new List<IComponent>();

        protected bool isActive = true;


        private bool runStart = false;
        private SpriteComponent Sprite;

        public float Scale { get; set; } = 1;

        public Direction.Horizontal FacingDirection { get; set; }

        public Vector2 Position { get; set; }

        //public Rectangle Collision
        //{
        //    get
        //    {
        //        Vector2 spriteSource = (Sprite?.Origin ?? Vector2.Zero) * (Scale * Screen.Scale);
        //        return customCollision ?? new Rectangle((int)(Position.X - spriteSource.X), (int)(Position.Y - spriteSource.Y), Width, Height);
        //    }
        //}

        //public bool CollidesWith(Entity body)
        //{
        //    return Collision.Intersects(body.Collision);
        //}

        public int Width => (int)((Sprite?.SpriteWidth ?? 0) * (Scale * Screen.Scale));
        public int Height => (int)((Sprite?.SpriteHeight ?? 0) * (Scale * Screen.Scale));

        public bool IsVisible { get; set; }

        public Entity(Vector2 position, Direction.Horizontal facingDirection = Direction.Horizontal.Right,
            float scale = 1f, Rectangle? customCollision = null)
        {
            IsVisible = true;
            FacingDirection = facingDirection;
            Scale = scale;
            Position = position;
            this.customCollision = customCollision;

        }

        public void Start()
        {
            Sprite = GetComponent<SpriteComponent>();

            for (int i = 0; i < components.Count; i++)
                components[i].Start();
        }


        public void CustomizeCollision(Rectangle collision)
        {
            customCollision = collision;
        }

        public void ResetCollisionToSpriteBounds()
        {
            customCollision = null;
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
            if (!isActive || !IsVisible)
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
