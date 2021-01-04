using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace Chamboco.Engine.Helpers
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
            Vector2 scale,
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
                (position + (rotateOrigin * scale)),
                source,
                color,
                rotation,
                 rotateOrigin + origin,
                 scale,
                effect, layerDepth);
        }

        public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K key, out V value) =>
            (key, value) = (pair.Key, pair.Value);

        public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> @this)
        {
            var index = 0;
            foreach (var item in @this)
                yield return (index++, item);
        }

        public static Vector2 WithX(this Vector2 @this, float x) => new(x, @this.Y);
        public static Vector2 WithY(this Vector2 @this, float y) => new(@this.X, y);
        public static Vector2 WithX(this Vector2 @this, int x) => new(x, @this.Y);
        public static Vector2 WithY(this Vector2 @this, int y) => new(@this.X, y);
        public static Vector2 ToVector2(this float f) => new(f, f);
        public static Vector2 ToVector2(this int i) => new(i, i);
        public static void MoveAndSlide(this Body body, Vector2 addPosition)
        {
            body.SetTransform(body.Position + (addPosition / Physics.PixelsPerMeter), 0);
        }

        public static Lazy<TR> Select<T, TR>(this Lazy<T> @this, Func<T, TR> map) =>
            new(() => map(@this.Value));

        public static Lazy<TR> SelectMany<T, TMap, TR>(this Lazy<T> @this, Func<T, Lazy<TMap>> map,
            Func<T, TMap, TR> project) =>
            new(() =>
            {
                var actual = @this.Value;
                var mapped = map(actual);
                return project(actual, mapped.Value);
            });

        public static Lazy<TR> SelectMany<T, TR>(this Lazy<T> @this, Func<T, Lazy<TR>> map) =>
            @this.SelectMany(map, (_, r) => r);
    }

}
