using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JonnyHammer.Game.Characters
{
    public class BigNaruto : Entity
    {
        public AnimatedSprite Animation => (Sprite as AnimatedSprite);
        private float speed = 3f;
        public BigNaruto(Vector2 position) : base(position, GetNarutaoAnimation())
        {

        }

        public static AnimatedSprite GetNarutaoAnimation()
        {
            Texture2D spriteSheet = Loader.LoadTexture("narutao");
            Dictionary<string, Frame[]> animationFrames = new Dictionary<string, Frame[]>();
            int width = 58, height = 63;

            animationFrames.Add("Idle", new Frame[]
            {
                new Frame()
                {
                    Name = "idle_1",
                    Source = new Rectangle(width * 4, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_2",
                    Source = new Rectangle(width * 5, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_3",
                    Source = new Rectangle(width * 6, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_4",
                    Source = new Rectangle(width * 7, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_5",
                    Source = new Rectangle(width * 8, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_6",
                    Source = new Rectangle(width * 9, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "idle_7",
                    Source = new Rectangle(width * 10, 0, width, height),
                    Duration = 100
                }
            });

            animationFrames.Add("Run", new Frame[]
            {
                new Frame()
                {
                    Name = "run_1",
                    Source = new Rectangle(0, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "run_2",
                    Source = new Rectangle(width * 1, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "run_3",
                    Source = new Rectangle(width * 2, 0, width, height),
                    Duration = 100
                },
                new Frame()
                {
                    Name = "run_4",
                    Source = new Rectangle(width * 3, 0, width, height),
                    Duration = 100
                }
            });

            return new AnimatedSprite(spriteSheet, animationFrames);
        }

        public void Run(Direction.Horizontal direction)
        {
            Animation.Change("Run");
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
