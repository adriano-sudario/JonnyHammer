using Chamboco.Engine.Entities;
using Chamboco.Engine.Entities.Components.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace Chamboco.Engine.Helpers
{
    public static class Camera2D
    {
        static float zoom;
        static float rotation;
        static Vector2 position;
        static Matrix transform = Matrix.Identity;
        static bool isViewTransformationDirty = true;
        static Matrix camTranslationMatrix = Matrix.Identity;
        static Matrix camRotationMatrix = Matrix.Identity;
        static Matrix camScaleMatrix = Matrix.Identity;
        static Matrix resTranslationMatrix = Matrix.Identity;
        static Vector3 camTranslationVector = Vector3.Zero;
        static Vector3 camScaleVector = Vector3.Zero;
        static Vector3 resTranslationVector = Vector3.Zero;

        public static float BoundWidth { get; private set; }
        public static float BoundHeight { get; private set; }

        public static float MinScale { get; private set; }
        public static float MaxScale { get; set; } = 5;

        public static void Initialize()
        {
            Zoom = 1f;
            rotation = 0.0f;
            position = Vector2.Zero;

            Center();
            InferMinZoon();
        }

        public static void Center() =>
            Position = new Vector2(Screen.VirtualWidth, ((float)Screen.VirtualHeight) / 2);

        public static void SetBounds(float width, float height) => (BoundWidth, BoundHeight) = (width, height);

        public static void ZoomUp(float scale) => Zoom = MathHelper.Clamp(Zoom + scale, MinScale, MaxScale);
        public static void ZoomDown(float scale) => Zoom = MathHelper.Clamp(Zoom - scale, MinScale, MaxScale);

        public static Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                isViewTransformationDirty = true;
            }
        }

        public static void Move(Vector2 amount) => Position += amount;

        public static void SetPosition(Vector2 position) => Position = position;

        public static float Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                {
                    zoom = 0.1f;
                }
                isViewTransformationDirty = true;
            }
        }

        public static float Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                isViewTransformationDirty = true;
            }
        }

        public static Matrix GetViewTransformationMatrix()
        {
            if (!isViewTransformationDirty) return transform;

            camTranslationVector.X = -(int)position.X;
            camTranslationVector.Y = -(int)position.Y;

            Matrix.CreateTranslation(ref camTranslationVector, out camTranslationMatrix);
            Matrix.CreateRotationZ(rotation, out camRotationMatrix);

            camScaleVector.X = zoom;
            camScaleVector.Y = zoom;
            camScaleVector.Z = 1;

            Matrix.CreateScale(ref camScaleVector, out camScaleMatrix);

            resTranslationVector.X = Screen.VirtualWidth * 0.5f;
            resTranslationVector.Y = Screen.VirtualHeight * 0.5f;
            resTranslationVector.Z = 0;

            Matrix.CreateTranslation(ref resTranslationVector, out resTranslationMatrix);

            transform = camTranslationMatrix *
                        camRotationMatrix *
                        camScaleMatrix *
                        resTranslationMatrix *
                        Screen.GetTransformationMatrix();

            isViewTransformationDirty = false;

            return transform;
        }

        public static void RecalculateTransformationMatrices() => isViewTransformationDirty = true;

        public static void InferMinZoon()
        {
            var scaleX = (float)Screen.VirtualWidth / Screen.ScreenWidth;
            var scaleY = (float)Screen.VirtualHeight / Screen.ScreenHeight;
            MinScale = MathF.Max(scaleX, scaleY);
        }

        public static void Follow(GameObject player)
        {
            var (x, y) = player.Transform.Position;

            var size = player.GetComponent<SpriteRenderer>();
            var halfCharWidth = Screen.IsFullScreen ? size?.Width / 2f ?? 0f : 0;
            var halfCharHeight = Screen.IsFullScreen ? size?.Height / 2f ?? 0f : 0;

            var halfRenderWidth = Screen.VirtualWidth / 2;
            var halfRenderHeight = Screen.VirtualHeight / 2;

            var minWidth = halfRenderWidth;
            var minHeight = halfRenderHeight;


            var newPosition = new Vector2(
                MathHelper.Clamp(x, minWidth, BoundWidth - halfRenderWidth + halfCharWidth),
                MathHelper.Clamp(y, minHeight, BoundHeight - halfRenderHeight + halfCharHeight)
            );

            Position = newPosition;
        }
    }
}
