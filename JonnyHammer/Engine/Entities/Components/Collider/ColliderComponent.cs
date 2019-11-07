using System;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Engine.Entities.Components.Collider
{
    public class ColliderComponent : Component
    {
        public bool IsDebug { get; set; }
        public bool AutoCheck { get; set; }
        
        public bool IsTrigger { get; set; }

        private Rectangle bounds;
        private Texture2D debugTexture;

        public Action<Entity> OnCollide { get; set; } = delegate {  };
        
        public Rectangle Bounds
        {
            get
            {
                var position = new Vector2(Entity.Position.X, Entity.Position.Y) + new Vector2(bounds.X, bounds.Y);
                return new Rectangle((int) position.X, (int) position.Y, (int) (bounds.Width * Entity.Scale),
                    (int) (bounds.Height * Entity.Scale));
            }
            private set => bounds = value;
        }

        public ColliderComponent(Rectangle rectangle, bool autoCheck = false, bool isDebug = false)
        {
            IsDebug = isDebug && System.Diagnostics.Debugger.IsAttached;
            Bounds = rectangle;
            debugTexture = Core.GetDebugTexture(Color.Red);
            AutoCheck = autoCheck;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDebug) return;

            spriteBatch.Draw(debugTexture, Bounds, Color.White);
        }

        public ColliderComponent(SpriteComponent spriteComponent)
            : this(
                new Rectangle(
                    (int) spriteComponent.Entity.Position.X,
                    (int) spriteComponent.Entity.Position.Y,
                    spriteComponent.Width,
                    spriteComponent.Height)
            )
        {
        }


        public bool CollidesWith(Rectangle rectangle) => Bounds.Intersects(rectangle);

        public bool CollidesWith(ColliderComponent collider) => CollidesWith(collider.Bounds);

        public bool CollidesWith(Entity entity)
        {
            if (Entity == entity)
                return false;
            
            var colliders = entity.GetComponents<ColliderComponent>();

            for (var i = 0; i < colliders.Length; i++)
                if (CollidesWith(colliders[i]))
                    return true;

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!AutoCheck)
                return;

            var entities = SceneManager.CurrentScene.Entities;

            for (int i = 0; i < entities.Count; i++)
                if (CollidesWith(entities[i]))
                    OnCollide(entities[i]);

            base.Update(gameTime);
        }
    }
}