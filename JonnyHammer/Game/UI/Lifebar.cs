using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
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

        public bool desapearing;

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
            desapearing = false;
        }


        public IEnumerator ShowBars()
        {

            lifebar.Opacity = 1;
            lifebarBack.Opacity = 1;
            yield return new WaitUntil(() => !desapearing);
            yield return Desapear();

        }


        IEnumerator Desapear()
        {
            yield return TimeSpan.FromSeconds(3);

            if (desapearing)
                yield break;

            desapearing = true;
            var opacity = lifebar.Opacity;
            var step = 0.2f;

            while (opacity > 0 && desapearing)
            {
                opacity -= step;

                lifebar.Opacity = opacity;
                lifebarBack.Opacity = opacity;

                yield return TimeSpan.FromMilliseconds(50);

            }
            desapearing = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bounds = new Rectangle(
            (int)Entity.Transform.Position.X,
            (int)Entity.Transform.Position.Y - 5,
            renderer.Width, 4);

            var currentLife = bounds.Width * CurrentLife / TotalLife;

            lifeBounds = new Rectangle(
                bounds.X, bounds.Y,
                currentLife, bounds.Height);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            lifebarBack.Draw(spriteBatch, bounds.Scale());
            lifebar.Draw(spriteBatch, lifeBounds.Scale());


        }

    }
}
