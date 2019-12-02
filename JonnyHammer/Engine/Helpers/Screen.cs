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
        public static int Width { get; set; }
        public static int Height { get; set; }

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

        private static void AdjustScreen()
        {
            if (graphics.IsFullScreen)
            {
                Width = graphicsDevice.DisplayMode.Width;
                Height = graphicsDevice.DisplayMode.Height;

                graphics.PreferredBackBufferWidth = Width;
                graphics.PreferredBackBufferHeight = Height;
                graphics.ApplyChanges();

                decimal scaleX = (decimal)Width / GraphicsDeviceManager.DefaultBackBufferWidth;
                decimal scaleY = (decimal)Height / GraphicsDeviceManager.DefaultBackBufferHeight;
                Scale = (int)Math.Ceiling(scaleX > scaleY ? scaleX : scaleY);
            }
            else
            {
                Width = GraphicsDeviceManager.DefaultBackBufferWidth;
                Height = GraphicsDeviceManager.DefaultBackBufferHeight;
                Scale = 2f;
            }
        }
    }

    public static class Extensions
    {
        public static Rectangle Scale(this Rectangle rectangle) =>
            new Rectangle(
                (int)(rectangle.X * Screen.Scale),
                (int)(rectangle.Y * Screen.Scale),
                (int)(rectangle.Width * Screen.Scale),
                (int)(rectangle.Height * Screen.Scale));

        public static int Scale(this int value) => (int)(value * Screen.Scale);
        public static float Scale(this float value) => (value * Screen.Scale);
        public static Vector2 Scale(this Vector2 value) => (value * Screen.Scale);
    }
}
