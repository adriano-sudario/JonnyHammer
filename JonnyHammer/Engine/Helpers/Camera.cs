using Microsoft.Xna.Framework;

namespace JonnyHammer.Engine.Helpers
{
    public static class Camera2D
    {
        static float _zoom;
        static float _rotation;
        static Vector2 _position;
        static Matrix _transform = Matrix.Identity;
        static bool _isViewTransformationDirty = true;
        static Matrix _camTranslationMatrix = Matrix.Identity;
        static Matrix _camRotationMatrix = Matrix.Identity;
        static Matrix _camScaleMatrix = Matrix.Identity;
        static Matrix _resTranslationMatrix = Matrix.Identity;
        static Vector3 _camTranslationVector = Vector3.Zero;
        static Vector3 _camScaleVector = Vector3.Zero;
        static Vector3 _resTranslationVector = Vector3.Zero;


        public static void Initialize()
        {
            Zoom = 1f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
            Position = new Vector2(Screen.VirtualWidth / 2, Screen.VirtualHeight / 2);
            //Position = new Vector2(ResolutionIndependentRenderer.ScreenWidth / 2, ResolutionIndependentRenderer.ScreenHeight / 2);

        }

        public static Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _isViewTransformationDirty = true;
            }
        }

        public static void Move(Vector2 amount)
        {
            Position += amount;
        }

        public static void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public static float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
                _isViewTransformationDirty = true;
            }
        }

        public static float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                _isViewTransformationDirty = true;
            }
        }

        public static Matrix GetViewTransformationMatrix()
        {
            if (_isViewTransformationDirty)
            {
                _camTranslationVector.X = -_position.X;
                _camTranslationVector.Y = -_position.Y;

                Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);
                Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

                _camScaleVector.X = _zoom;
                _camScaleVector.Y = _zoom;
                _camScaleVector.Z = 1;

                Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

                _resTranslationVector.X = Screen.VirtualWidth * 0.5f;
                _resTranslationVector.Y = Screen.VirtualHeight * 0.5f;
                _resTranslationVector.Z = 0;

                Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

                _transform = _camTranslationMatrix *
                             _camRotationMatrix *
                             _camScaleMatrix *
                             _resTranslationMatrix *
                             Screen.GetTransformationMatrix();

                _isViewTransformationDirty = false;
            }

            return _transform;
        }

        public static void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }
    }
}
