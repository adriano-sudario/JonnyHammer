using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JonnyHamer.Engine.Helpers
{
    public static class Screen
    {
        private static GraphicsDeviceManager graphics;
        private static GraphicsDevice graphicsDevice;
        private static float MaxZoon => MinScale + 5;

        public static float MinScale { get; set; }
        public static float Scale { get; set; } = 1;
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

            if (MinScale == 0)
                MinScale = Scale;
        }

        public static void ScaleUp(float scale)
        {
            var newScale = Scale + scale;
            Scale = newScale;// MathHelper.Clamp(newScale, MinScale, MaxZoon);
        }
        public static void ScaleDown(float scale)
        {
            var newScale = Scale - scale;
            Scale = newScale; //MathHelper.Clamp(newScale, MinScale, MaxZoon);
        }

        public static void AdjustScale()
        {
            var scaleX = (float)Width / RenderWidth;
            var scaleY = (float)Height / RenderHeight;
            MinScale = (MathF.Min(scaleX, scaleY));
            Scale = (MathF.Max(scaleX, scaleY));
        }
    }

}
