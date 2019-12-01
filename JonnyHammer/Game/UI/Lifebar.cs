using JonnyHamer.Engine.Entities.Sprites;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace JonnyHammer.Game.UI
{
    public class Lifebar : Component
    {
        SolidColorTexture lifebar;
        SolidColorTexture lifebarBack;
        SpriteComponent renderer;
        private Rectangle bounds;
        private Rectangle lifeBounds;

        public int TotalLife { get; }
        public int CurrentLife { get; private set; }

        public Lifebar(int totalLife) => CurrentLife = TotalLife = totalLife;

        public override void Start()
        {
            lifebar = new SolidColorTexture(Color.ForestGreen);
            lifebarBack = new SolidColorTexture(Color.Red);
            renderer = GetComponent<SpriteComponent>();

            StartCoroutine(Desapear());

        }

        public void UpdateLife(int currentLife)
        {

            CurrentLife = currentLife;
            StartCoroutine(ShowBars());
        }


        public IEnumerator ShowBars()
        {
            var color = lifebar.Color;
            var back = lifebarBack.Color;

            color.A = 255;
            back.A = 255;

            lifebar.Color = color;
            lifebarBack.Color = back;


            yield return TimeSpan.FromSeconds(2);

            yield return Desapear();

        }

        IEnumerator Desapear()
        {
            var color = lifebar.Color;
            var back = lifebarBack.Color;
            byte step = 15;

            while (lifebar.Color.A > 0)
            {
                color.A -= step;
                back.A -= step;

                lifebar.Color = color;
                lifebarBack.Color = back;

                yield return TimeSpan.FromMilliseconds(100);
            }
        }

        public override void Update(GameTime gameTime)
        {
            bounds = new Rectangle(
            (int)Entity.Transform.Position.X,
            (int)Entity.Transform.Position.Y - 7,
            renderer.Width, 4);

            var currentLife = bounds.Width * CurrentLife / TotalLife;

            lifeBounds = new Rectangle(
                bounds.X, bounds.Y,
                currentLife, bounds.Height);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            lifebarBack.Draw(spriteBatch, bounds);
            lifebar.Draw(spriteBatch, lifeBounds);


            base.Draw(spriteBatch);
        }

    }
}
