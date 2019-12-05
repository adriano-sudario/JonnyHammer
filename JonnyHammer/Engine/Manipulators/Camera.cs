using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using JonnyHammer.Engine;
using Microsoft.Xna.Framework;

namespace JonnyHamer.Engine.Manipulators
{
    public static class Camera
    {
        private static Vector3 position = Vector3.Zero;

        public static Matrix ViewMatrix { get; set; }
        public static int AreaWidth { get; set; }
        public static int AreaHeight { get; set; }
        public static Vector2 Position => new Vector2(position.X, position.Y);

        public static void Update()
        {
            ViewMatrix = Matrix.CreateTranslation(position);
        }

        public static void GoTo(Vector2 position)
        {
            Camera.position.X = position.X;
            Camera.position.Y = position.Y;
            Update();
        }

        public static void CenterOn(Vector2 position)
        {
            AdjustPosition(position, 1, 1);
            Update();
        }

        public static void Follow(Entity body)
        {
            var sprite = body.GetComponent<SpriteRenderer>();
            int width = sprite?.Width ?? 0;
            int height = sprite?.Height ?? 0;

            var spriteSource = (sprite?.Origin ?? Vector2.Zero) * (body.Transform.Scale);
            AdjustPosition(
                (body.Transform.Position) - spriteSource,
                width,
                height
            );
            Update();
        }

        public static void ScrollHorizontally(Vector2 followPosition, int followWidth, int scrollIncrement)
        {
            if (followPosition.X + (followWidth / 2) >= (Screen.Width / 2) &&
                followPosition.X + (followWidth / 2) <= AreaWidth - (Screen.Width / 2))
                position.X -= scrollIncrement;
        }

        public static void ScrollVertically(Vector2 followPosition, int followHeight, int scrollIncrement)
        {
            if (followPosition.Y + (followHeight / 2) >= (Screen.Height / 2) &&
                followPosition.Y + (followHeight / 2) <= AreaHeight - (Screen.Height / 2))
                position.Y -= scrollIncrement;
        }

        static float GetDifferencePixels(float area, float axisSize)
        {
            var newArea = area;

            while (newArea < axisSize)
                newArea += area;

            return axisSize - newArea;
        }

        static void AdjustPosition(Vector2 followPosition, int followWidth, int followHeight)
        {
            var positionHorizontal = -(followPosition.X.ScaleScreen() - (Screen.Width / 2) + (followWidth.ScaleScreen() / 2));
            var minWidth = AreaWidth.ScaleScreen() - Screen.Width + GetDifferencePixels(AreaWidth, Screen.Width);
            float maxWidth = 0;

            float positionVertical = -(followPosition.Y.ScaleScreen() - (Screen.Height / 2) + (followHeight.ScaleScreen() / 2));
            float minHeight = AreaHeight.ScaleScreen() - Screen.Height + GetDifferencePixels(AreaHeight, Screen.Height);
            float maxHeight = 0;

            position.X = MathHelper.Clamp(positionHorizontal, -minWidth, maxWidth);
            position.Y = MathHelper.Clamp(positionVertical, -minHeight, maxHeight);

        }
    }
}
