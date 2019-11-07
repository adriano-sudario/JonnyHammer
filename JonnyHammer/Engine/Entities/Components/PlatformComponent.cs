
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Entities.Components
{
    public class PlatformComponent : Component
    {
        private MoveComponent move;

        public int Gravity { get; set; } = 3;

        private Vector2 force;

        public override void Start()
        {
            move = Entity.RequireComponent<MoveComponent>();
        }

        public void AddForce(Vector2 direction)
        {
            force = direction;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            move.MoveVertically((int)(Entity.Position.Y + Gravity));

            if (force.Y > 0)
            {
                var ap = new Vector2(0, Gravity * -2);
                move.MoveAndSlide(ap, false);
                force += ap;
            }
        }


    }
}
