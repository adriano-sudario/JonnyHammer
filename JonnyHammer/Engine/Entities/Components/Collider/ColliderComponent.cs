using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Engine.Entities.Components.Collider
{
    public class ColliderComponent : Component, IScalable
    {
        public float Scale { get; set; }
        public bool IsDebug { get; set; }
        
        private Rectangle bounds;
        private Texture2D debugTexture;

        public Rectangle Bounds
        {
            get 
        {
                var position = new Vector2(Entity.Position.X, Entity.Position.Y) + new Vector2(bounds.X, bounds.Y);
                return new Rectangle((int) position.X, (int) position.Y, (int) (bounds.Width * Scale), (int) (bounds.Height *  Scale));
        }
            private set => bounds = value;
        }

        public ColliderComponent(Rectangle rectangle, bool isDebug = false)
        {
            IsDebug = isDebug;
            Bounds = rectangle;
            debugTexture = Core.GetDebugTexture(Color.Red);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDebug) return;
            
            spriteBatch.Draw(debugTexture, Bounds, Color.White);
        }

        public ColliderComponent(SpriteComponent spriteComponent) 
            : this(new Rectangle((int)spriteComponent.Position.X, (int) spriteComponent.Position.Y, spriteComponent.Width, spriteComponent.Height)) { }
       
        
        
       public bool CollidesWith(Rectangle rectangle) => Bounds.Intersects(rectangle);
       
       public bool CollidesWith(ColliderComponent collider) => CollidesWith(collider.Bounds);

       public bool CollidesWith(Entity entity)
       {
           var colliders = entity.GetComponents<ColliderComponent>();
           
           for (var i = 0; i < colliders.Length; i++)
               if (CollidesWith(colliders[i]))
                   return true;

           return false;
       }
    }
}