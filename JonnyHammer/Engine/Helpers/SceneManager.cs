using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JonnyHamer.Engine.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using JonnyHammer.Engine.Scenes;
using JonnyHamer.Engine.Manipulators;

namespace JonnyHamer.Engine.Helpers
{
    public class SceneManager
    {
        private static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        public static Scene CurrentScene { get; set; }

        public static T GetScene<T>(string sceneId) where T : Scene => (T)scenes[sceneId];

        public static T GetScene<T>() where T : Scene => (T)scenes.Where(s => s.Value.GetType() == typeof(T)).First().Value;

        public static T GetScene<T>(Scene scene) where T : Scene => (T)scenes.Where(s => s.Value == scene).First().Value;

        public static T GetCurrentScene<T>() where T : Scene => (T)CurrentScene;

        public static Dictionary<string, Scene> GetScenes() => scenes;

        public static void AddScene(string sceneId, Scene scene, bool setToCurrent = false)
        {
            scenes.Add(sceneId, scene);

            if (scenes.Count == 1 || setToCurrent)
                ChangeScene(sceneId);
        }

        public static void RemoveScene(string sceneId)
        {
            if (CurrentScene == scenes[sceneId])
                return;

            scenes.Remove(sceneId);
        }

        public static void ChangeScene(string sceneId, Action onSceneChanged = null)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            CurrentScene = scenes[sceneId];
            onSceneChanged?.Invoke();
        }

        public static void Update(GameTime gameTime)
        {
            Camera.Update();
            SoundTrack.Update(gameTime);
            CurrentScene?.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene?.Draw(spriteBatch);
        }
    }
}
