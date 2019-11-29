using Caieta.Components.Utils;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace JonnyHammer.Engine.Entities.Components
{
    public enum TweenMode { Persist, OneShot, Loop, Yoyo, Restart };
    public enum TweenProperty { X, Y, Scale, Angle, Opacity };

    public class TweenComponent : Component
    {
        private Vector2 _position;
        private float _scale;
        private float _angle;
        private float _opacity;
        private float _custom;
        private double elapsedTime;
        private SpriteComponent entitySprite;
        private object reference;
        private PropertyInfo customProperty;

        public Action OnStart;
        public Action OnFinish;
        public TweenMode Mode { get; private set; }
        public bool IsReverse { get; private set; }
        public bool IsRepeating { get; private set; }
        public TweenProperty Property { get; private set; }
        public float Eased { get; private set; }
        public float Duration { get; private set; }
        public float InitialValue { get; private set; }
        public float CurrentValue
        {
            get
            {
                if (reference != null && customProperty != null)
                    return (float)customProperty.GetValue(reference);

                switch (Property)
                {
                    case TweenProperty.X:
                        return Entity.Transform.X;

                    case TweenProperty.Y:
                        return Entity.Transform.Y;

                    case TweenProperty.Scale:
                        return Entity.Transform.Scale;

                    case TweenProperty.Angle:
                        return Entity.Transform.Rotation;

                    case TweenProperty.Opacity:
                        if (entitySprite != null)
                            return entitySprite.Opacity;
                        break;

                    default:
                        throw new Exception($"[Tween]: Invalid Tween Property '{Property}'.");
                }

                return 0;
            }
            private set
            {
                if (reference != null && customProperty != null)
                {
                    customProperty.SetValue(reference, Convert.ChangeType(MathHelper.Lerp(_custom, TargetValue, value), customProperty.PropertyType), null);
                    return;
                }

                switch (Property)
                {
                    case TweenProperty.X:
                        Entity.Transform.MoveAndSlideHorizontally(MathHelper.Lerp(_position.X, TargetValue, value));
                        break;

                    case TweenProperty.Y:
                        Entity.Transform.MoveAndSlideVertically(MathHelper.Lerp(_position.Y, TargetValue, value));
                        break;

                    case TweenProperty.Scale:
                        Entity.Transform.Scale = MathHelper.Lerp(_scale, TargetValue, value);
                        break;

                    case TweenProperty.Angle:
                        Entity.Transform.Rotation = MathHelper.Lerp(_angle, MathHelper.ToRadians(TargetValue), value);
                        break;

                    case TweenProperty.Opacity:
                        if (entitySprite != null)
                            entitySprite.Opacity = MathHelper.Lerp(_opacity, TargetValue, value);
                        break;

                    default:
                        throw new Exception($"[Tween]: Invalid Tween Property '{Property}'.");
                }
            }
        }
        public float TargetValue { get; set; }

        public EaseFunction.Ease Ease;
        public float Percent { get; private set; }

        private TweenComponent(TweenMode mode, float value, EaseFunction.Ease easer, float millisecondsDuration, Action onStart = null, Action onFinish = null)
        {
            OnStart = onStart;
            OnFinish = onFinish;
            Mode = mode;
            IsReverse = false;
            TargetValue = value;
            Ease = easer;
            Eased = Percent = 0;
            Duration = millisecondsDuration;

            if (Mode == TweenMode.Loop || Mode == TweenMode.Yoyo || Mode == TweenMode.Restart)
                IsRepeating = true;

            if (millisecondsDuration <= 0)
                throw new Exception($"[Tween]: Duration must be a positive integer. Setting from '{millisecondsDuration}'to 0 (zero).");
        }

        public TweenComponent(TweenMode mode, object reference, string propertyName, float value, EaseFunction.Ease easer, float millisecondsDuration, Action onStart = null, Action onFinish = null) :
            this(mode, value, easer, millisecondsDuration, onStart, onFinish)
        {
            this.reference = reference;
            customProperty = reference.GetType().GetProperty(propertyName);

            if (customProperty.PropertyType != typeof(float))
                throw new Exception("[Tween]: Property must be a float type.");
        }

        public TweenComponent(TweenMode mode, TweenProperty property, float value, EaseFunction.Ease easer, float millisecondsDuration, Action onStart = null, Action onFinish = null) :
            this(mode, value, easer, millisecondsDuration, onStart, onFinish)
        {
            Property = property;
        }

        public override void Start()
        {
            base.Start();

            if ((reference == null || customProperty == null) && Entity == null)
                throw new Exception("[Tween]: Entity cannot be null if it's not a custom property.");
            else if (Entity != null)
                entitySprite = Entity.GetComponent<SpriteComponent>();

            Begin();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsActive)
                return;

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            Percent = MathHelper.Clamp(Math.Min((float)elapsedTime, Duration) / Duration, 0, 1);

            if (IsReverse)
                Percent = 1 - Percent;

            Increment();

            if (elapsedTime >= Duration)
                Finish();
        }

        private void Increment()
        {
            if (Ease != null)
                Eased = Ease(Percent);
            else
                Eased = Percent;

            CurrentValue = Eased;
        }

        private void Finish()
        {
            Percent = IsReverse ? 0 : 1;

            Increment();

            switch (Mode)
            {
                case TweenMode.Persist:
                    IsActive = false;
                    break;

                case TweenMode.OneShot:
                    IsActive = false;
                    Entity?.Destroy();
                    break;

                case TweenMode.Loop:
                    TargetValue = InitialValue;
                    Begin();
                    break;

                case TweenMode.Yoyo:
                    if (!IsReverse)
                        StartTween();
                    else
                        IsActive = false;

                    IsReverse = !IsReverse;

                    if (!IsActive)
                        OnFinish?.Invoke();
                    return;

                case TweenMode.Restart:
                    StartTween();
                    break;

                default:
                    throw new Exception($"[Tween]: Invalid Tween Mode '{Mode}'.");
            }

            OnFinish?.Invoke();
        }

        public void StartTween()
        {
            base.Start();
            IsActive = true;
            elapsedTime = 0;
            OnStart?.Invoke();
            Eased = Percent = 0;
        }

        public void Begin()
        {
            InitProperties();
            StartTween();
        }

        public void InitProperties()
        {
            if (Entity != null)
            {
                InitialValue = CurrentValue;
                _position = Entity.Transform.Position;
                _scale = Entity.Transform.Scale;
                _angle = Entity.Transform.Rotation;

                if (reference != null && customProperty != null)
                    _custom = (float)customProperty.GetValue(reference);

                if (entitySprite != null)
                    _opacity = entitySprite.Opacity;
                else if (Property == TweenProperty.Opacity)
                    throw new Exception($"[Tween]: Couldn't start Tween property '{Property}'. Entity Sprite is null.");
            }
            else
                throw new Exception($"[Tween]: Couldn't start Tween property '{Property}'. Entity is null.");
        }
    }
}
