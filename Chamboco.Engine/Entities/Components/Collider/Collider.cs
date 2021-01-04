using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamboco.Engine.Entities.Components.Collider
{
    public class Collider : Component
    {
        public bool IsDebug { get; set; }
        public bool AutoCheck { get; set; }

        public bool IsTrigger { get; set; }

        private Rectangle bounds;
        private Texture2D debugTexture;

        public event Action<GameObject> OnCollide = delegate { };
        public event Action<GameObject> OnTrigger = delegate { };

        Physics.Physics physics;

        public Rectangle Bounds
        {
            get
            {
                var position = (new Vector2(Entity.Transform.X, Entity.Transform.Y))
                             + new Vector2(bounds.X, bounds.Y) * Entity.Transform.Scale;

                return new Rectangle(
                            (int)position.X,
                            (int)position.Y,
                            (int)(bounds.Width * Entity.Transform.Scale),
                            (int)(bounds.Height * Entity.Transform.Scale)
                          );
            }
            private set => bounds = value;
        }

        public Collider(Rectangle rectangle, bool autoCheck = false, bool isDebug = false, Color? debugColor = null, bool isTrigger = false)
        {
            IsDebug = isDebug && System.Diagnostics.Debugger.IsAttached;
            Bounds = rectangle;
            debugTexture = GetDebugTexture(debugColor ?? Color.Red);
            AutoCheck = autoCheck;
            IsTrigger = isTrigger;
        }

        public static Texture2D GetDebugTexture(Color color)
        {
            var texture = new Texture2D(Core.Instance.GraphicsDevice, 1, 1);
            if (color.A > 150) color.A = 150;
            texture.SetData(new[] { color });
            return texture;
        }

        public bool CollideOrTriggersWithAnyEntity(bool stopOnFirst = false) => CollideOrTriggersWithAnyEntity(out var _, stopOnFirst);
        public bool CollideOrTriggersWithAnyEntity(out GameObject[] entity, bool stopOnFirst = false)
        {
            if (SceneManager.CurrentScene == null)
            {
                entity = Array.Empty<GameObject>();
                return false;
            }

            var entities = SceneManager.CurrentScene.Entities;
            var entityList = new List<GameObject>();

            for (var i = 0; i < entities.Count; i++)
                if (!IsTrigger && CollidesWith(entities[i]) || (IsTrigger && TriggerWith(entities[i])))
                {
                    if (stopOnFirst)
                    {
                        entity = new[] { entities[i] };
                        return true;
                    }
                    else
                        entityList.Add(entities[i]);
                }

            entity = entityList.ToArray();

            for (var i = 0; i < entity.Length; i++)
            {

                if (IsTrigger)
                    OnTrigger(entity[i]);
                else
                    OnCollide(entity[i]);
            }

            return entityList.Any();
        }

        public Collider(SpriteRenderer spriteComponent, bool autoCheck = false, bool isDebug = false, Color? debugColor = null, bool isTrigger = false)
        : this(
            new Rectangle(
                (int)spriteComponent.Entity.Transform.X,
                (int)spriteComponent.Entity.Transform.Y,
                spriteComponent.SpriteWidth,
                spriteComponent.SpriteHeight),
            autoCheck, isDebug, debugColor, isTrigger
        )
        { }

        public bool CollidesWith(Rectangle rectangle) => Bounds.Intersects(rectangle);

        public bool CollidesWith(Collider collider) => CollidesWith(collider.Bounds);

        public bool CollidesWith(GameObject entity)
        {
            if (Entity == entity || IsTrigger)
                return false;

            if (physics?.Collided.Count > 0 && physics.Collided.Any(x => x.Tag == entity))
                return true;

            var colliders = entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Length; i++)
                if (CollidesWith(colliders[i]))
                    return true;

            return false;
        }
        public bool TriggerWith(Rectangle rectangle) => Bounds.Intersects(rectangle);

        public bool TriggerWith(Collider collider) => CollidesWith(collider.Bounds);

        public bool TriggerWith(GameObject entity)
        {
            if (Entity == entity || !IsTrigger)
                return false;

            var colliders = entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Length; i++)
                if (CollidesWith(colliders[i]))
                    return true;

            return false;
        }

        public override void Start()
        {
            physics = Entity.GetComponent<Physics.Physics>();
        }

        public override void Update(GameTime gameTime)
        {
            if (!AutoCheck)
                return;

            CollideOrTriggersWithAnyEntity(out _);

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDebug) return;

            spriteBatch.Draw(
                debugTexture,
                Bounds,
                Color.White);
        }
    }
}
