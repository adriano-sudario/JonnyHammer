using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;

namespace JonnyHammer.Engine.Scenes
{
    public abstract class Scene
    {
        IList<Entity> entities = new List<Entity>();
        public IReadOnlyList<Entity> Entities => entities.ToArray();
        public World World { get; }

        public Scene()
        {
            World = new World();
            World.Gravity = new Vector2(0, 10);

            if (SceneManager.CurrentScene == null)
                SceneManager.CurrentScene = this;
        }

        public void Destroy(Entity entity)
        {
            entity.Dispose();
            entities.Remove(entity);
        }

        public T Spawn<T>(string name = "no name", Vector2? position = null) where T : Entity, new()
        {
            var entity = new T { Position = position ?? Vector2.Zero, FacingDirection = Direction.Horizontal.Right, Name = name };
            entities.Add(entity);
            return entity;
        }

        public virtual void Update(GameTime gameTime)
        {
            WorldStep(gameTime);
            for (int i = 0; i < entities.Count; i++)
                entities[i].FullUpdate(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch);
        }

        public void WorldStep(GameTime gameTime) =>
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, 1 / 30f));
    }

}
