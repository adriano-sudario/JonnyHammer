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

        //public static void CenterOn(Vector2 position)
        //{
        //    AdjustPosition(position, 1, 1);
        //    Update();
        //}

        public static void Follow(Entity body)
        {
            var sprite = body.GetComponent<SpriteComponent>();
            int width = sprite?.Width ?? 1;
            int height = sprite?.Height ?? 1;

            var spriteSource = (sprite?.Origin ?? Vector2.Zero) * (body.Transform.Scale);
            AdjustPosition(
                (body.Transform.Position - spriteSource).ScaleScreen(),
                width.ScaleScreen(),
                height.ScaleScreen()
                );
            Update();
        }

        //public static void ScrollHorizontally(Vector2 followPosition, int followWidth, int scrollIncrement)
        //{
        //    if (followPosition.X + (followWidth / 2) >= (Screen.Width / 2) &&
        //        followPosition.X + (followWidth / 2) <= AreaWidth - (Screen.Width / 2))
        //        position.X -= scrollIncrement;
        //}

        //public static void ScrollVertically(Vector2 followPosition, int followHeight, int scrollIncrement)
        //{
        //    if (followPosition.Y + (followHeight / 2) >= (Screen.Height / 2) &&
        //        followPosition.Y + (followHeight / 2) <= AreaHeight - (Screen.Height / 2))
        //        position.Y -= scrollIncrement;
        //}

        static void AdjustPosition(Vector2 followPosition, int followWidth, int followHeight)
        {
            float positionHorizontal = -(followPosition.X - (Screen.Width / 2) + (followWidth / 2));
            float minWidth = -(AreaWidth.ScaleScreen() - Screen.Width);
            float maxWidth = 0;
            float positionVertical = -(followPosition.Y - (Screen.Height / 2) + (followHeight / 2));
            float minHeight = -(AreaHeight.ScaleScreen() - Screen.Height);
            float maxHeight = 0;
            position.X = MathHelper.Clamp(positionHorizontal, minWidth, maxWidth);
            position.Y = MathHelper.Clamp(positionVertical, minHeight, maxHeight);
        }
    }
}
