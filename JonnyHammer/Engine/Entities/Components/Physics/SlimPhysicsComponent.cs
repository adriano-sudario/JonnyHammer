
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Entities.Components
{
    public class SlimPhysicsComponent : Component
    {
        public Vector2 GravityForce => new Vector2(0, Gravity);
        public float Gravity { get; set; } = 8f;

        Vector2 velocity;
        MoveComponent move;

        public override void Start()
        {
            move = Entity.RequireComponent<MoveComponent>();
        }

        public void AddForce(Vector2 velocity)
        {
            this.velocity = new Vector2(velocity.X, -velocity.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity += (GravityForce * time);

            if (!move.MoveAndSlide(velocity, false))
                velocity = Vector2.Zero;
        }


    }
}
