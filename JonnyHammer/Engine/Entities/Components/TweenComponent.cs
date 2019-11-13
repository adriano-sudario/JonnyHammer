using Caieta.Components.Utils;
using JonnyHamer.Engine.Entities;
using JonnyHamer.Engine.Entities.Sprites;
using Microsoft.Xna.Framework;
using System;

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
        private double elapsedTime;
        private SpriteComponent entitySprite;

        public Action OnStart;
        public Action OnFinish;
        public TweenMode Mode { get; private set; }
        public bool IsReverse { get; private set; }
        public bool IsRepeating{ get; private set; }
        public TweenProperty Property { get; private set; }
        public float Eased { get; private set; }
        public float Duration { get; private set; }
        public float InitialValue { get; private set; }
        public float CurrentValue
        {
            get
            {
                if (Entity == null)
                    return 0;

                switch (Property)
                {
                    case TweenProperty.X:
                        return Entity.Position.X;

                    case TweenProperty.Y:
                        return Entity.Position.Y;

                    case TweenProperty.Scale:
                        return Entity.Scale;

                    case TweenProperty.Angle:
                        return Entity.Rotation;

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
                if (Entity == null)
                    return;

                switch (Property)
                {
                    case TweenProperty.X:
                        Entity.Position = new Vector2(MathHelper.Lerp(_position.X, TargetValue, value), 0);
                        break;

                    case TweenProperty.Y:
                        Entity.Position = new Vector2(0, MathHelper.Lerp(_position.Y, TargetValue, value));
                        break;

                    case TweenProperty.Scale:
                        Entity.Scale = MathHelper.Lerp(_scale, TargetValue, value); ;
                        break;

                    case TweenProperty.Angle:
                        Entity.Rotation = MathHelper.Lerp(_angle, MathHelper.ToRadians(TargetValue), value);
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

        public TweenComponent(TweenMode mode, TweenProperty property, float value, EaseFunction.Ease easer, float duration, Action onStart = null, Action onFinish = null)
        {
            OnStart = onStart;
            OnFinish = onFinish;
            Mode = mode;
            IsReverse = false;
            Property = property;
            TargetValue = value;
            Ease = easer;
            Eased = Percent = 0;
            Duration = duration;

            if (Mode == TweenMode.Loop || Mode == TweenMode.Yoyo || Mode == TweenMode.Restart)
                IsRepeating = true;

            if (duration <= 0)
                throw new Exception($"[Tween]: Duration must be a positive integer. Setting from '{duration}'to 0 (zero).");
        }

        public override void Start()
        {
            base.Start();

            entitySprite = Entity.GetComponent<SpriteComponent>();

            InitProperties();
            StartTween();
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
            Percent = 1;
            
            Increment();
            
            OnFinish?.Invoke();

            switch (Mode)
            {
                case TweenMode.Persist:
                    IsActive = false;
                    break;
                    
                case TweenMode.OneShot:
                    IsActive = false;
                    if (Entity != null)
                        Entity.Destroy();
                    break;
                    
                case TweenMode.Loop:
                    TargetValue = InitialValue;
                    Restart();
                    break;
                    
                case TweenMode.Yoyo:
                    StartTween();
                    IsReverse = !IsReverse;
                    break;
                    
                case TweenMode.Restart:
                    StartTween();
                    break;

                default:
                    throw new Exception($"[Tween]: Invalid Tween Mode '{Mode}'.");
            }
        }

        public void StartTween()
        {
            base.Start();
            IsActive = true;
            elapsedTime = 0;
            OnStart?.Invoke();
            Eased = Percent = 0;
        }

        public void Restart()
        {
            InitProperties();
            StartTween();
        }

        public void InitProperties()
        {
            if (Entity != null)
            {
                InitialValue = CurrentValue;
                _position = Entity.Position;
                _scale = Entity.Scale;
                _angle = Entity.Rotation;

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
