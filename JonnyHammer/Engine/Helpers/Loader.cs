using JonnyHamer.Engine.Entities.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JonnyHamer.Engine.Helpers
{
    static class Loader
    {
        private static ContentManager content;

        static string _contentFullPath;
        public static string ContentFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_contentFullPath))
                    _contentFullPath = content.RootDirectory;

                return _contentFullPath;
            }
        }

        public static void Initialize(ContentManager content) => Loader.content = content;

        public static Texture2D LoadTexture(string textureName) => content.Load<Texture2D>("Graphics/" + textureName);
        public static IDictionary<string, Frame[]> LoadAnimation(string aspriteJsonFile)
        {
            dynamic data = LoadDeserializedJsonFile(aspriteJsonFile);
            var frameData = new Dictionary<string, Frame[]>();

            IDictionary<string, dynamic> dataFrames = data.frames.ToObject<Dictionary<string, dynamic>>();
            dataFrames = dataFrames
                            .Select(x => (Key: x.Key.Split(" ")[1], x.Value))
                            .ToDictionary(x => x.Key, x => x.Value);

            foreach (var anim in data.meta.frameTags)
            {
                var animationName = anim.name.ToString();
                int from = anim.from;
                int to = anim.to;

                var frames = new List<Frame>();
                for (int i = from; i < to; i++)
                {
                    var jsonFrame = dataFrames[$"{i}.aseprite"];

                    int x = jsonFrame.frame.x;
                    int y = jsonFrame.frame.y;
                    int w = jsonFrame.frame.w;
                    int h = jsonFrame.frame.h;
                    int duration = jsonFrame.duration;

                    frames.Add(new Frame
                    {
                        Duration = duration,
                        Name = $"{animationName}_{i}",
                        Source = new Rectangle(x, y, w, h),
                    });
                }
                frameData.Add(animationName, frames.ToArray());

            }

            return frameData;
        }

        public static SoundEffect LoadSound(string soundName) => content.Load<SoundEffect>("Sounds/" + soundName);

        public static SpriteFont LoadFont(string fontName) => content.Load<SpriteFont>("Fonts/" + fontName);

        public static T LoadDeserializedJsonFile<T>(string fileName)
        {
            var jsonString = LoadJsonFile(fileName);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        public static object LoadDeserializedJsonFile(string fileName)
        {
            var jsonString = LoadJsonFile(fileName);
            return JsonConvert.DeserializeObject(jsonString);
        }

        private static string LoadJsonFile(string fileName) => File.ReadAllText(Path.Combine(ContentFullPath, "Data/" + fileName + ".json"));


        public static void SaveJsonFile<T>(string fileName, T data) => SaveJsonFile(fileName, JsonConvert.SerializeObject(data));

        private static void SaveJsonFile(string fileName, string jsonText) => File.WriteAllText(Path.Combine(ContentFullPath, "Data/" + fileName + ".json"), jsonText);
    }
}
