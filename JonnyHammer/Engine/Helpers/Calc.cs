using System;
using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Helpers
{
    public static class Calc
    {
        // Floor Vector2 to explicit (int) cast Vector2
        public static Vector2 Floor(this Vector2 val)
        {
            return new Vector2((int)Math.Floor(val.X), (int)Math.Floor(val.Y));
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }

        /*
         *           ANGLE ROTARION & ORIENTATION
         * 
         *   Engine angles start at: 0º   pointing  right  (1,0)
         *      spins down and goes: 90º  pointing  down   (0,1)
         *  keep spinning clockwise: 180º pointing  left  (-1,0)
         *             follow up to: 270º pointing  up    (0,-1)
         *      and finish back to 360º/0º.
         */
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 Round(Vector2 value)
        {
            Vector2 rounded;
            rounded.X = (float)Math.Round(value.X);
            rounded.Y = (float)Math.Round(value.Y);

            return rounded;
        }

        public static string GetHumanReadableTime(int minutes, int seconds)
        {
            int _minutes = minutes;
            int _seconds = seconds;

            if (_minutes < 10)
            {
                if (_seconds < 10)
                    return "0" + _minutes + ":0" + _seconds;
                else
                    return "0" + _minutes + ":" + _seconds;
            }
            else
            {
                if (_seconds < 10)
                    return _minutes + ":0" + _seconds;
                else
                    return _minutes + ":" + _seconds;
            }
        }

        public static string GetHumanReadableTime(int minutes, int seconds, int milliseconds)
        {
            int _milliseconds = milliseconds;
            string displayMS;

            if (_milliseconds < 10)
                displayMS = ":0" + _milliseconds;
            else
                displayMS = ":" + _milliseconds;

            return GetHumanReadableTime(minutes, seconds) + displayMS;
        }

        public static string GetHumanReadableTime(TimeSpan timeSpan, bool withMilliseconds = false)
        {
            if (withMilliseconds)
                return GetHumanReadableTime(timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

            return GetHumanReadableTime(timeSpan.Minutes, timeSpan.Seconds);
        }

        private static int _seed = Environment.TickCount;
        private static Random random = new Random(_seed);


        public static float RandomFloat()
        {
            return (float)random.NextDouble();
        }

        public static float RandomFloat(float max)
        {
            double mantissa = (random.NextDouble() * (max + 1)) - max;
            double exponent = Math.Pow(2.0, random.Next(-126, 127));
            return (float)(mantissa * exponent);
        }

        public static float RandomFloat(float min, float max)
        {
            return min + RandomFloat(max - min);
        }

        public static Color RandomColor()
        {
            return new Color(RandomFloat(), RandomFloat(), RandomFloat());
        }

        public static int Random(int max)
        {
            return random.Next(max + 1);
        }

        public static int Random(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static T Choose<T>(params T[] choices)
        {
            return choices[random.Next(choices.Length)];
        }
    }
}
