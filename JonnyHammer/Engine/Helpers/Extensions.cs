using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JonnyHammer.Engine
{
    public static class Extensions
    {

        public static void DrawEntity(
            this SpriteBatch spriteBatch,
            Texture2D spriteStrip,
            Transform transform,
            Rectangle source,
            Color color,
            Vector2 origin,
            SpriteEffects effect,
            float layerDepth) =>
            spriteBatch.DrawnEntity(spriteStrip, transform.Position, transform.Scale, source, color, transform.Rotation, origin, effect, layerDepth);

        public static void DrawnEntity(
            this SpriteBatch spriteBatch,
            Texture2D spriteStrip,
            Vector2 position,
            float scale,
            Rectangle source,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effect,
            float layerDepth
            )
        {

            var rotateOrigin = new Vector2(source.Width / 2f, source.Height / 2f);
            spriteBatch.Draw(
                spriteStrip,
                (position),
                //(position + (rotateOrigin * scale)),
                source,
                color,
                rotation,
                 //rotateOrigin + origin,
                 origin,
                 scale,
                effect, layerDepth);
        }

        public static Rectangle ScaleScreen(this Rectangle rectangle) =>
            new Rectangle(
                (int)(rectangle.X * Screen_.Scale),
                (int)(rectangle.Y * Screen_.Scale),
                (int)(rectangle.Width * Screen_.Scale),
                (int)(rectangle.Height * Screen_.Scale));

        public static int ScaleScreen(this int value) => (int)(value * Screen_.Scale);
        public static float ScaleScreen(this float value) => (value * Screen_.Scale);
        public static Vector2 ScaleScreen(this Vector2 value) => (value * Screen_.Scale);

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
