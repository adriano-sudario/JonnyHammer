using Chamboco.Engine.Entities.Components.Sprites;
using Microsoft.Xna.Framework;

namespace Chamboco.Engine.Entities.Components.Physics
{
    public class SlimPhysics : Component
    {
        Collider.Collider collider;
        SpriteRenderer renderer;
        bool applyGravity = true;
        float gravityForce;
        Vector2 velocity;
        Vector2 previousPosition;
        public float Gravity { get; } = 8f;
        public bool KeepOnCameraBounds { get; set; }

        public override void Start()
        {
            collider = Entity.GetComponent<Collider.Collider>();
            renderer = Entity.GetComponent<SpriteRenderer>();

            collider.IsTrigger = true;
            collider.OnTrigger += (collidedEntity) =>
            {
                if (!collidedEntity.Name.StartsWith("floor"))
                    return;

                var fixedPosition = new Vector2(Entity.Transform.X, Entity.Transform.Y);

                var collidedEntityRenderer = collidedEntity.GetComponent<SpriteRenderer>();
                var collidedOnBottom = previousPosition.Y < Entity.Transform.Y && Entity.Transform.Y + renderer?.Height > collidedEntity.Transform.Y;
                var collidedOnTop = previousPosition.Y > Entity.Transform.Y && Entity.Transform.Y < collidedEntity.Transform.Y + collidedEntityRenderer?.Height;

                if (collidedOnBottom)
                    fixedPosition.Y = collidedEntity.Transform.Y - (renderer?.Height ?? 1) - 1;
                else if (collidedOnTop)
                    fixedPosition.Y = collidedEntity.Transform.Y;

                applyGravity = collidedOnBottom || collidedOnTop;

                Entity.Transform.FixPosition(fixedPosition);

                DisableGravity();
            };
        }

        public void AddForce(Vector2 newVelocity)
        {
            velocity = new Vector2(newVelocity.X, -newVelocity.Y);

            if (newVelocity.Y > 0)
                ApplyGravity();
        }

        private void ApplyGravity()
        {
            applyGravity = true;
            gravityForce = 0;
        }

        private void DisableGravity()
        {
            applyGravity = false;
            gravityForce = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            previousPosition = Entity.Transform.Position;

            if (applyGravity)
            {
                gravityForce += Gravity * time;
                Entity.Transform.MoveAndSlideVertically(gravityForce);
            }

            Entity.Transform.MoveAndSlide(velocity);

            if (KeepOnCameraBounds)
                Entity.Transform.KeepOnCameraBounds(Entity);
        }
    }
}
