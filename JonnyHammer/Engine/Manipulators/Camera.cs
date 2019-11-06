using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace JonnyHamer.Engine.Manipulators
{
    public static class Camera
    {
        private static Vector3 position = Vector3.Zero;

        public static Matrix ViewMatrix { get; set; }
        public static int AreaWidth { get; set; }
        public static int AreaHeight { get; set; }

        public static void Update()
        {
            ViewMatrix = Matrix.CreateTranslation(position);
        }

        public static void Update(Vector2 followPosition, int followWidth, int followHeight)
        {
            AdjustPosition(followPosition, followWidth, followHeight);
            Update();
        }

        public static void Update(Entity body)
        {
            var spriteSource = (body.GetComponent<SpriteComponent>()?.Origin ?? Vector2.Zero) * (body.Scale * Screen.Scale);
            AdjustPosition(body.Position - spriteSource, body.Width, body.Height);
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

        private static void AdjustPosition(Vector2 followPosition, int followWidth, int followHeight)
        {
            float positionHorizontal = -(followPosition.X - (Screen.Width / 2) + (followWidth / 2));
            float minWidth = -(AreaWidth - Screen.Width);
            float maxWidth = 0;
            float positionVertical = -(followPosition.Y - (Screen.Height / 2) + (followHeight / 2));
            float minHeight = -(AreaHeight - Screen.Height);
            float maxHeight = 0;
            position.X = MathHelper.Clamp(positionHorizontal, minWidth, maxWidth);
            position.Y = MathHelper.Clamp(positionVertical, minHeight, maxHeight);
        }
    }
}
