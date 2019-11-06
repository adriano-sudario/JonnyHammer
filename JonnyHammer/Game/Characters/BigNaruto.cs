using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : Entity
    {
        public AnimatedSprite Animation => Sprite as AnimatedSprite;

        private float speed = 3f;

        public BigNaruto(Vector2 position) : base(position, GetNarutaoAnimation())
        {

        }

        public static AnimatedSprite GetNarutaoAnimation()
        {
            var spriteSheet = Loader.LoadTexture("narutao");
            var animationFrames = Loader.LoadAnimation("narutao");

            return new AnimatedSprite(spriteSheet, animationFrames);
        }

        public void Run(Direction.Horizontal direction)
        {
            Animation.Change("Running");
            MoveAndSlide(new Vector2(
                direction == Direction.Horizontal.Left ? -speed : speed, 0));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
