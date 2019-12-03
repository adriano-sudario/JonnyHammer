using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace JonnyHammer.Engine
{
    public static class Extensions
    {

        //public static void DrawScaled(
        //    this SpriteBatch spriteBatch,
        //    Texture2D spriteStrip,
        //    Transform transform,
        //    float spriteWidth,
        //    float spriteHeight,
        //    Rectangle source,
        //    Color color,
        //    SpriteEffects effect,
        //    Vector2 origin,
        //    float layerDepth
        //    )
        //{

        //    var rotateOrigin = new Vector2(spriteWidth * transform.Scale / 2f, spriteHeight * transform.Scale / 2f);
        //    spriteBatch.Draw(
        //        spriteStrip,
        //        (transform.Position + (origin * transform.Scale)) * Screen.Scale,
        //        source,
        //        color,
        //        transform.Rotation,
        //        rotateOrigin,
        //        Screen.Scale * transform.Scale,
        //        effect, layerDepth);
        //}

        public static Rectangle ScaleScreen(this Rectangle rectangle) =>
            new Rectangle(
                (int)(rectangle.X * Screen.Scale),
                (int)(rectangle.Y * Screen.Scale),
                (int)(rectangle.Width * Screen.Scale),
                (int)(rectangle.Height * Screen.Scale));

        public static int ScaleScreen(this int value) => (int)(value * Screen.Scale);
        public static float ScaleScreen(this float value) => (value * Screen.Scale);
        public static Vector2 ScaleScreen(this Vector2 value) => (value * Screen.Scale);

        public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K key, out V value) =>
            (key, value) = (pair.Key, pair.Value);

        public static IEnumerable<(int, T)> AsIndexed<T>(this IEnumerable<T> @this)
        {
            var index = 0;
            foreach (var item in @this)
                yield return (index++, item);
        }
    }
}
