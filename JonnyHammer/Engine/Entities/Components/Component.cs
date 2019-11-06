using JonnyHamer.Engine.Entities;
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
        public int Width => Entity.Width;
        public float Scale { get => Entity.Scale; set => Entity.Scale = value; }
        public int Height => Entity.Height;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void SetEntity(Entity entity) => Entity = entity;

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }
    }
}
