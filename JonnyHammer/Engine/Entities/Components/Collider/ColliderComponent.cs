using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Entities.Components.Collider
{
    public class ColliderComponent : Component
    {
        private SpriteComponent Sprite;
        private Rectangle? customCollision;

        public override void Start()
        {
            Sprite = GetComponent<SpriteComponent>();
            base.Start();
        }

        public Rectangle Collision
        {
            get
            {
                var spriteSource = (Sprite?.Origin ?? Vector2.Zero) * (Scale * Screen.Scale);
                return customCollision ?? new Rectangle((int)(Position.X - spriteSource.X), (int)(Position.Y - spriteSource.Y), Sprite.Width, Sprite.Height);
            }
        }

        public void CustomizeCollision(Rectangle collision)
        {
            customCollision = collision;
        }

        public void ResetCollisionToSpriteBounds()
        {
            customCollision = null;
        }
        public bool CollidesWith(Entity body)
        {
            return Collision.Intersects(Collision);
        }
    }
}
