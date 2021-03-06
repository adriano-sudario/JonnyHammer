﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Linq;

namespace Chamboco.Engine.Helpers
{
    public static class Screen
    {
        static Viewport viewport;
        static float ratioX;
        static float ratioY;
        static Vector2 virtualMousePosition;
        static GraphicsDevice graphicsDevice;
        static GraphicsDeviceManager graphics;

        public static bool IsFullScreen => graphics.IsFullScreen;

        public static Color BackgroundColor => Debugger.IsAttached ? Color.DarkSlateGray : Color.Black;

        public static int VirtualHeight { get; private set; }
        public static int VirtualWidth { get; private set; }

        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }

        public static void Initialize(GraphicsDevice device, GraphicsDeviceManager graphicsDeviceManager)
        {
            graphicsDevice = device;
            graphics = graphicsDeviceManager;
            ScreenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            ScreenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

            VirtualWidth = ScreenWidth;
            VirtualHeight = ScreenHeight;
        }

        public static void SetVirtualArea(int width, int height)
        {
            VirtualWidth = width;
            VirtualHeight = height;
            Configure();
        }

        public static void ToggleFullScreen()
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            AdjustScreen();
        }

        static void AdjustScreen()
        {
            if (graphics.IsFullScreen)
            {
                var currentResolution = graphicsDevice.DisplayMode;

                var largerMode = GraphicsAdapter
                                    .DefaultAdapter
                                    .SupportedDisplayModes
                                    .First(x => x.Width >= currentResolution.Width);

                var width = largerMode.Width;
                var height = largerMode.Height;

                ChangeResolution(width, height);

            }
            else
            {
                ChangeResolution(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight);
            }

        }
        public static void ChangeResolution(int realScreenWidth, int realScreenHeight)
        {
            graphics.PreferredBackBufferWidth = realScreenWidth;
            graphics.PreferredBackBufferHeight = realScreenHeight;
            graphics.ApplyChanges();

            ScreenWidth = realScreenWidth;
            ScreenHeight = realScreenHeight;
            Configure();

            Camera2D.RecalculateTransformationMatrices();
        }

        public static void Adjust(bool isFullScreen)
        {
            graphics.IsFullScreen = isFullScreen;
            AdjustScreen();
        }


        public static void Configure()
        {
            SetupVirtualScreenViewport();

            ratioX = (float)viewport.Width / VirtualWidth;
            ratioY = (float)viewport.Height / VirtualHeight;

            dirtyMatrix = true;
        }

        public static void SetupFullViewport()
        {
            var vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = ScreenWidth;
            vp.Height = ScreenHeight;
            graphicsDevice.Viewport = vp;
            dirtyMatrix = true;
        }

        public static void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            SetupFullViewport();
            // Clear to Black
            graphicsDevice.Clear(BackgroundColor);
            // Calculate Proper Viewport according to Aspect Ratio
            SetupVirtualScreenViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
        }

        private static Matrix scaleMatrix;
        private static bool dirtyMatrix = true;

        public static Matrix GetTransformationMatrix()
        {
            if (dirtyMatrix)
                RecreateScaleMatrix();

            return scaleMatrix;
        }

        static void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out scaleMatrix);
            dirtyMatrix = false;
        }

        public static Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            var realX = screenPosition.X - viewport.X;
            var realY = screenPosition.Y - viewport.Y;

            virtualMousePosition.X = realX / ratioX;
            virtualMousePosition.Y = realY / ratioY;

            return virtualMousePosition;
        }

        public static void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            var width = ScreenWidth;
            var height = (int)(width / targetAspectRatio + .5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
            }

            // set up the new viewport centered in the backbuffer
            viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height
            };

            graphicsDevice.Viewport = viewport;
        }
    }
}
