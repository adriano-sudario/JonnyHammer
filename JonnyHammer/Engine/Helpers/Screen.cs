using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JonnyHamer.Engine.Helpers
{
    public static class Screen
    {
        private static GraphicsDeviceManager graphics;
        private static GraphicsDevice graphicsDevice;

        public static float Scale { get; set; }
        public static int Width => graphics.PreferredBackBufferWidth;
        public static int Height => graphics.PreferredBackBufferHeight;

        public static int RenderWidth => GraphicsDeviceManager.DefaultBackBufferWidth;
        public static int RenderHeight => GraphicsDeviceManager.DefaultBackBufferHeight;

        public static void Initialize(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
        {
            Screen.graphics = graphics;
            Screen.graphicsDevice = graphicsDevice;
        }

        public static void ChangeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        public static void ToggleFullScreen(Action afterToggle = null)
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            AdjustScreen();
            afterToggle?.Invoke();
        }

        public static void Adjust(bool isFullScreen, Action afterAdjustment = null)
        {
            graphics.IsFullScreen = isFullScreen;
            AdjustScreen();
            afterAdjustment?.Invoke();
        }

        static void AdjustScreen()
        {
            if (graphics.IsFullScreen)
            {
                var width = graphicsDevice.DisplayMode.Width;
                var height = graphicsDevice.DisplayMode.Height;

                ChangeResolution(width, height);

                AdjustScale();
            }
            else
            {
                ChangeResolution(RenderWidth, RenderHeight);
                Scale = 1f;
            }
        }

        public static void AdjustScale()
        {
            var scaleX = (double)Width / RenderWidth;
            var scaleY = (double)Height / RenderHeight;
            Scale = (int)Math.Ceiling(scaleX > scaleY ? scaleX : scaleY);
        }
    }

    public static class Extensions
    {
        public static Rectangle ScaleScreen(this Rectangle rectangle) =>
            new Rectangle(
                (int)(rectangle.X * Screen.Scale),
                (int)(rectangle.Y * Screen.Scale),
                (int)(rectangle.Width * Screen.Scale),
                (int)(rectangle.Height * Screen.Scale));

        public static int ScaleScreen(this int value) => (int)(value * Screen.Scale);
        public static float ScaleScreen(this float value) => (value * Screen.Scale);
        public static Vector2 ScaleScreen(this Vector2 value) => (value * Screen.Scale);
    }
}
