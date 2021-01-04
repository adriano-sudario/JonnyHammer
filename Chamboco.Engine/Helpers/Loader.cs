using Chamboco.Engine.Entities.Components.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Chamboco.Engine.Helpers
{
    public static class Loader
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
        public static IDictionary<string, Frame[]> LoadAsepriteFrames(string asepriteJsonFile)
        {
            var data = LoadDeserializedJsonFile<AsepriteModel>(asepriteJsonFile);
            return ParseAsepriteJson(data);
        }


        static IDictionary<string, Frame[]> ParseAsepriteJson(AsepriteModel data)
        {
            var frameData = new Dictionary<string, Frame[]>();

            var dataFrames = data.Frames
                            .Select(x => (Key: x.Key.Split(" ")[1], x.Value))
                            .ToDictionary(x => x.Key, x => x.Value);

            foreach (var anim in data.Meta.FrameTags)
            {
                var animationName = anim.Name;
                var from = anim.From;
                var to = anim.To;

                var frames = new List<Frame>();
                for (var i = from; i < to; i++)
                {
                    var jsonFrame = dataFrames[$"{i}.aseprite"];

                    int x = jsonFrame.Frame.X;
                    int y = jsonFrame.Frame.Y;
                    int w = jsonFrame.Frame.W;
                    int h = jsonFrame.Frame.H;
                    int duration = jsonFrame.Duration;

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
            var jsonSerializer = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
            return JsonSerializer.Deserialize<T>(jsonString, jsonSerializer);
        }

        private static string LoadJsonFile(string fileName) => File.ReadAllText(Path.Combine(ContentFullPath, "Data/" + fileName + ".json"));

        public static void SaveJsonFile<T>(string fileName, T data) => SaveJsonFile(fileName, JsonSerializer.Serialize(data));

        private static void SaveJsonFile(string fileName, string jsonText) => File.WriteAllText(Path.Combine(ContentFullPath, "Data/" + fileName + ".json"), jsonText);
    }
}
