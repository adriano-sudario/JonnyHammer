using System;
using System.Collections;
using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components;
using Chamboco.Engine.Entities.Components.Collider;
using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JonnyHammer.Game.Components
{
    public class Lifebar : Component
    {
        SolidColorTexture lifebar;
        SolidColorTexture lifebarBack;
        SpriteRenderer renderer;
        Transform transform = new ();

        public int TotalLife { get; }
        public int CurrentLife { get; private set; }

        bool desapearing;

        int lifeBarWidth;
        int lifeBarHeight = 4;

        public Lifebar(int totalLife)
        {
            CurrentLife = TotalLife = totalLife;
            lifebar = new SolidColorTexture(Color.ForestGreen);
            lifebarBack = new SolidColorTexture(Color.Red);
        }

        public override void Start()
        {
            Entity.StartCoroutine(Desapear());
            renderer = Entity.GetComponent<SpriteRenderer>();
            transform.Position = new(0, -5);
            transform.Parent = Entity.Transform;
        }

        public void UpdateLife(int currentLife)
        {
            CurrentLife = currentLife;
            Entity.StartCoroutine(ShowBars());
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
            lifeBarWidth = renderer.Width * CurrentLife / TotalLife;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            lifebarBack.Draw(spriteBatch, new ((int)transform.X, (int)transform.Y, renderer.Width, lifeBarHeight));
            lifebar.Draw(spriteBatch, new ((int)transform.X, (int)transform.Y, lifeBarWidth, lifeBarHeight));
        }

    }
}
