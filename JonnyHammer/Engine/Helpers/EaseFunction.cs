using Microsoft.Xna.Framework;
using System;

namespace Caieta.Components.Utils
{
    public static class EaseFunction
    {
        public delegate float Ease(float t);

        public static readonly Ease Linear = (float t) => { return t; };

        public static readonly Ease SineIn = (float t) => { return -(float)Math.Cos(MathHelper.PiOver2 * t) + 1; };
        public static readonly Ease SineOut = (float t) => { return (float)Math.Sin(MathHelper.PiOver2 * t); };
        public static readonly Ease SineInOut = (float t) => { return -(float)Math.Cos(MathHelper.Pi * t) / 2f + .5f; };

        public static readonly Ease QuadIn = (float t) => { return t * t; };
        public static readonly Ease QuadOut = Invert(QuadIn);
        public static readonly Ease QuadInOut = Follow(QuadIn, QuadOut);

        public static readonly Ease CubeIn = (float t) => { return t * t * t; };
        public static readonly Ease CubeOut = Invert(CubeIn);
        public static readonly Ease CubeInOut = Follow(CubeIn, CubeOut);

        public static readonly Ease QuintIn = (float t) => { return t * t * t * t * t; };
        public static readonly Ease QuintOut = Invert(QuintIn);
        public static readonly Ease QuintInOut = Follow(QuintIn, QuintOut);

        public static readonly Ease ExpoIn = (float t) => { return (float)Math.Pow(2, 10 * (t - 1)); };
        public static readonly Ease ExpoOut = Invert(ExpoIn);
        public static readonly Ease ExpoInOut = Follow(ExpoIn, ExpoOut);

        public static readonly Ease BackIn = (float t) => { return t * t * (2.70158f * t - 1.70158f); };
        public static readonly Ease BackOut = Invert(BackIn);
        public static readonly Ease BackInOut = Follow(BackIn, BackOut);

        public static readonly Ease BigBackIn = (float t) => { return t * t * (4f * t - 3f); };
        public static readonly Ease BigBackOut = Invert(BigBackIn);
        public static readonly Ease BigBackInOut = Follow(BigBackIn, BigBackOut);

        public static readonly Ease ElasticIn = (float t) =>
        {
            var ts = t * t;
            var tc = ts * t;
            return (33 * tc * ts + -59 * ts * ts + 32 * tc + -5 * ts);
        };
        public static readonly Ease ElasticOut = (float t) =>
        {
            var ts = t * t;
            var tc = ts * t;
            return (33 * tc * ts + -106 * ts * ts + 126 * tc + -67 * ts + 15 * t);
        };
        public static readonly Ease ElasticInOut = Follow(ElasticIn, ElasticOut);

        private const float B1 = 1f / 2.75f;
        private const float B2 = 2f / 2.75f;
        private const float B3 = 1.5f / 2.75f;
        private const float B4 = 2.5f / 2.75f;
        private const float B5 = 2.25f / 2.75f;
        private const float B6 = 2.625f / 2.75f;

        public static readonly Ease BounceIn = (float t) =>
        {
            t = 1 - t;
            if (t < B1)
                return 1 - 7.5625f * t * t;
            if (t < B2)
                return 1 - (7.5625f * (t - B3) * (t - B3) + .75f);
            if (t < B4)
                return 1 - (7.5625f * (t - B5) * (t - B5) + .9375f);
            return 1 - (7.5625f * (t - B6) * (t - B6) + .984375f);
        };

        public static readonly Ease BounceOut = (float t) =>
        {
            if (t < B1)
                return 7.5625f * t * t;
            if (t < B2)
                return 7.5625f * (t - B3) * (t - B3) + .75f;
            if (t < B4)
                return 7.5625f * (t - B5) * (t - B5) + .9375f;
            return 7.5625f * (t - B6) * (t - B6) + .984375f;
        };

        public static readonly Ease BounceInOut = (float t) =>
        {
            if (t < .5f)
            {
                t = 1 - t * 2;
                if (t < B1)
                    return (1 - 7.5625f * t * t) / 2;
                if (t < B2)
                    return (1 - (7.5625f * (t - B3) * (t - B3) + .75f)) / 2;
                if (t < B4)
                    return (1 - (7.5625f * (t - B5) * (t - B5) + .9375f)) / 2;
                return (1 - (7.5625f * (t - B6) * (t - B6) + .984375f)) / 2;
            }
            t = t * 2 - 1;
            if (t < B1)
                return (7.5625f * t * t) / 2 + .5f;
            if (t < B2)
                return (7.5625f * (t - B3) * (t - B3) + .75f) / 2 + .5f;
            if (t < B4)
                return (7.5625f * (t - B5) * (t - B5) + .9375f) / 2 + .5f;
            return (7.5625f * (t - B6) * (t - B6) + .984375f) / 2 + .5f;
        };

        public static Ease Invert(Ease easer)
        {
            return (float t) => { return 1 - easer(1 - t); };
        }

        public static Ease Follow(Ease first, Ease second)
        {
            return (float t) => { return (t <= 0.5f) ? first(t * 2) / 2 : second(t * 2 - 1) / 2 + 0.5f; };
        }

        public static float UpDown(float eased)
        {
            if (eased <= .5f)
                return eased * 2;
            else
                return 1 - (eased - .5f) * 2;
        }
    }
}