using Chamboco.Engine.Entities.Components.Sprites;
using Chamboco.Engine.Helpers;
using Microsoft.Xna.Framework;
using System;

namespace Chamboco.Engine.Entities.Components
{
    public enum TweenMode { Persist, OneShot, Loop, Yoyo, Restart };
    public enum TweenProperty { X, Y, ScaleX, ScaleY, Angle, Opacity };

    public class Tween : Component
    {
        Vector2 position;
        Vector2 scale;
        float angle;
        float opacity;
        float custom;
        double elapsedTime;
        SpriteRenderer entitySprite;
        Func<float> getCustomValue;
        Action<float> setCustomValue;

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
                if (getCustomValue != null)
                    return getCustomValue();

                switch (Property)
                {
                    case TweenProperty.X:
                        return Entity.Transform.X;

                    case TweenProperty.Y:
                        return Entity.Transform.Y;

                    case TweenProperty.ScaleX:
                        return Entity.Transform.Scale.X;

                    case TweenProperty.ScaleY:
                        return Entity.Transform.Scale.Y;

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
                if (setCustomValue != null)
                {
                    setCustomValue(MathHelper.Lerp(custom, TargetValue, value) );
                    return;
                }

                switch (Property)
                {
                    case TweenProperty.X:
                        Entity.Transform.MoveAndSlideHorizontally(MathHelper.Lerp(position.X, TargetValue, value));
                        break;

                    case TweenProperty.Y:
                        Entity.Transform.MoveAndSlideVertically(MathHelper.Lerp(position.Y, TargetValue, value));
                        break;

                    case TweenProperty.ScaleX:
                        Entity.Transform.Scale = Entity.Transform.Scale.WithX(MathHelper.Lerp(scale.X, TargetValue, value));
                        break;
                    case TweenProperty.ScaleY:
                        Entity.Transform.Scale = Entity.Transform.Scale.WithY(MathHelper.Lerp(scale.Y, TargetValue, value));
                        break;

                    case TweenProperty.Angle:
                        Entity.Transform.Rotation = MathHelper.Lerp(angle, MathHelper.ToRadians(TargetValue), value);
                        break;

                    case TweenProperty.Opacity:
                        if (entitySprite != null)
                            entitySprite.Opacity = MathHelper.Lerp(opacity, TargetValue, value);
                        break;

                    default:
                        throw new Exception($"[Tween]: Invalid Tween Property '{Property}'.");
                }
            }
        }
        public float TargetValue { get; set; }

        public EaseFunction.Ease Ease;
        public float Percent { get; private set; }

        private Tween(TweenMode mode, float value, EaseFunction.Ease easer, float millisecondsDuration, Action onStart = null, Action onFinish = null)
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

        public Tween(TweenMode mode, TweenProperty property, float value, EaseFunction.Ease easer, float millisecondsDuration, Action onStart = null, Action onFinish = null) :
            this(mode, value, easer, millisecondsDuration, onStart, onFinish)
        {
            Property = property;
        }

        public override void Start()
        {
            entitySprite = Entity.GetComponent<SpriteRenderer>();
            Begin();
        }

        public override void Update(GameTime gameTime)
        {
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

        public Tween(
            TweenMode mode,
            Func<float> valueGetter,
            Action<float> valueSetter,
            float value,
            EaseFunction.Ease easer,
            float millisecondsDuration,
            Action onStart = null,
            Action onFinish = null) :
            this(mode, value, easer, millisecondsDuration, onStart, onFinish)
        {
            getCustomValue = valueGetter;
            setCustomValue = valueSetter;
        }
        private void Increment()
        {
            Eased = Ease?.Invoke(Percent) ?? Percent;
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
                position = Entity.Transform.Position;
                scale = Entity.Transform.Scale;
                angle = Entity.Transform.Rotation;

                if (getCustomValue != null)
                    custom = getCustomValue();

                if (entitySprite != null)
                    opacity = entitySprite.Opacity;
                else if (Property == TweenProperty.Opacity)
                    throw new Exception($"[Tween]: Couldn't start Tween property '{Property}'. Entity Sprite is null.");
            }
            else
                throw new Exception($"[Tween]: Couldn't start Tween property '{Property}'. Entity is null.");
        }
    }
}
