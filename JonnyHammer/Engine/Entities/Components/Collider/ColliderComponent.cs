using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHamer.Engine.Managers;
using JonnyHammer.Engine.Entities.Components.Phisycs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JonnyHammer.Engine.Entities.Components.Collider
{
    public class ColliderComponent : Component
    {
        public bool IsDebug { get; set; }
        public bool AutoCheck { get; set; }

        public bool IsTrigger { get; set; }

        private Rectangle bounds;
        private Texture2D debugTexture;

        public event Action<Entity> OnCollide = delegate { };
        public event Action<Entity> OnTrigger = delegate { };

        PhysicsComponent physics = null;

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

        public ColliderComponent(Rectangle rectangle, bool autoCheck = false, bool isDebug = false, Color? debugColor = null, bool isTrigger = false)
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
        public bool CollideOrTriggersWithAnyEntity(out Entity[] entity, bool stopOnFirst = false)
        {
            if (SceneManager.CurrentScene == null)
            {
                entity = Array.Empty<Entity>();
                return false;
            }

            var entities = SceneManager.CurrentScene.Entities;
            var entityList = new List<Entity>();

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

        public ColliderComponent(SpriteComponent spriteComponent, bool autoCheck = false, bool isDebug = false, Color? debugColor = null, bool isTrigger = false)
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

        public bool CollidesWith(ColliderComponent collider) => CollidesWith(collider.Bounds);

        public bool CollidesWith(Entity entity)
        {
            if (Entity == entity || IsTrigger)
                return false;

            if (physics?.Collided.Count > 0 && physics.Collided.Any(x => x.Tag == entity))
                return true;

            var colliders = entity.GetComponents<ColliderComponent>();
            for (var i = 0; i < colliders.Length; i++)
                if (CollidesWith(colliders[i]))
                    return true;

            return false;
        }
        public bool TriggerWith(Rectangle rectangle) => Bounds.Intersects(rectangle);

        public bool TriggerWith(ColliderComponent collider) => CollidesWith(collider.Bounds);

        public bool TriggerWith(Entity entity)
        {
            if (Entity == entity || !IsTrigger)
                return false;

            var colliders = entity.GetComponents<ColliderComponent>();
            for (var i = 0; i < colliders.Length; i++)
                if (CollidesWith(colliders[i]))
                    return true;

            return false;
        }

        public override void Start()
        {
            physics = Entity.GetComponent<PhysicsComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            if (!AutoCheck)
                return;

            CollideOrTriggersWithAnyEntity(out var _);

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDebug) return;

            //var origin =
            //     new Vector2(
            //        (Entity.Width / 2),
            //         (Entity.Height / 2));

            //var position = new Vector2(Bounds.X, Bounds.Y);

            //spriteBatch.Draw(
            //    debugTexture,
            //    position + origin,
            //    Bounds,
            //    Color.White,
            //    Entity.Transform.Rotation,
            //    origin,
            //    1,
            //    SpriteEffects.None, 0
            //    );

            spriteBatch.Draw(
                debugTexture,
                Bounds.Scale(),
                Color.White);
        }
    }
}