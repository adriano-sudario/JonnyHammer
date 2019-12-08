using JonnyHamer.Engine.Entities.Sprites;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Entities.Components
{
    public class SlimPhysics : Component
    {
        private Collider.Collider collider;
        private SpriteRenderer renderer;
        private bool applyGravity = true;
        private float gravityForce = 0;
        private Vector2 velocity;
        private Vector2 previousPosition;

        public Vector2 GravityForce => new Vector2(0, Gravity);
        public float Gravity { get; set; } = 8f;
        public bool KeepOnCameraBounds { get; set; }

        public override void Start()
        {
            collider = GetComponent<Collider.Collider>();
            renderer = GetComponent<SpriteRenderer>();

            collider.IsTrigger = true;
            collider.OnTrigger += (collidedEntity) =>
            {
                if (!collidedEntity.Name.StartsWith("floor"))
                    return;

                var fixedPosition = new Vector2(Entity.Transform.X, Entity.Transform.Y);

                var collidedEntityRenderer = collidedEntity.GetComponent<SpriteRenderer>();
                bool collidedOnBottom = previousPosition.Y < Entity.Transform.Y && Entity.Transform.Y + renderer?.Height > collidedEntity.Transform.Y;
                bool collidedOnTop = previousPosition.Y > Entity.Transform.Y && Entity.Transform.Y < collidedEntity.Transform.Y + collidedEntityRenderer?.Height;

                if (collidedOnBottom)
                    fixedPosition.Y = collidedEntity.Transform.Y - (renderer?.Height ?? 1) - 1;
                else if (collidedOnTop)
                    fixedPosition.Y = collidedEntity.Transform.Y;

                applyGravity = collidedOnBottom || collidedOnTop;

                Entity.Transform.FixPosition(fixedPosition);

                DisableGravity();
            };
        }

        public void AddForce(Vector2 velocity)
        {
            this.velocity = new Vector2(velocity.X, -velocity.Y);

            if (velocity.Y > 0)
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
