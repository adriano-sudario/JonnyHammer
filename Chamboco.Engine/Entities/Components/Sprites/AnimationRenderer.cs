using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamboco.Engine.Entities.Components.Sprites
{
    public class FrameChangeEventArgs : EventArgs
    {
        public Frame LastFrame { get; set; }
        public Frame CurrentFrame { get; set; }
        public bool HasCompletedCicle { get; set; }
    }

    public class Frame
    {
        public string Name { get; set; }
        public Rectangle Source { get; set; }
        public int Duration { get; set; }
    }

    public class AnimationRenderer : SpriteRenderer
    {
        private int elapsedTime;
        private IDictionary<string, Frame[]> sequences;
        private Frame[] currentSequence;
        private int currentFrameIndex;

        private Frame CurrentFrame => currentSequence[currentFrameIndex];

        public bool IsPlaying { get; private set; }
        public string CurrentName { get; set; }
        public bool IsLooping { get; set; }
        public bool AutoPlay { get; set; }

        public override int SpriteWidth => currentSequence?[currentFrameIndex].Source.Width ?? 0;
        public override int SpriteHeight => currentSequence?[currentFrameIndex].Source.Height ?? 0;
        public override Rectangle Source
        {
            get => CurrentFrame.Source;
            set => base.Source = value;
        }

        public event EventHandler<FrameChangeEventArgs> OnFrameChange;

        public AnimationRenderer(Texture2D spriteStrip, IDictionary<string, Frame[]> sequences, bool isLooping = true, bool autoPlay = true,
            EventHandler<FrameChangeEventArgs> onFrameChange = null, Rectangle source = default,
            float opacity = 1f, Vector2 origin = default) : base(spriteStrip, source: source, opacity: opacity, origin: origin)
        {
            if (spriteStrip == null)
                throw new Exception("spriteStrip não pode ser nulo");


            if (sequences == null || sequences.Count == 0)
                throw new Exception("Não existe nenhuma sequência de animação");

            AutoPlay = autoPlay;
            this.sequences = sequences;
            Change(sequences.ElementAt(0).Key, isLooping);
            OnFrameChange = onFrameChange;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsPlaying)
                return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime < CurrentFrame.Duration)
                return;

            ChangeFrame();
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            elapsedTime = 0;
            currentFrameIndex = 0;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Change(string name, bool isLooping = true)
        {
            if (name == CurrentName)
                return;

            CurrentName = name;
            currentSequence = sequences[CurrentName];
            IsLooping = isLooping;
            elapsedTime = 0;
            currentFrameIndex = 0;

            if (AutoPlay)
                Play();
        }

        public void Change(Frame[] frames, bool isLooping = true)
        {
            if (frames == null || frames.Length == 0)
                throw new Exception("Não existe nenhum frame");

            currentSequence = frames;
            IsLooping = isLooping;
            elapsedTime = 0;
            currentFrameIndex = 0;

            if (AutoPlay)
                Play();
        }

        public List<string> GetAnimationNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < sequences.Count; i++)
                names.Add(sequences.ElementAt(i).Key);

            return names;
        }

        private void ChangeFrame()
        {
            FrameChangeEventArgs arguments = new FrameChangeEventArgs();
            arguments.LastFrame = CurrentFrame;
            currentFrameIndex++;
            arguments.HasCompletedCicle = currentFrameIndex >= currentSequence.Length;

            if (arguments.HasCompletedCicle)
                currentFrameIndex = !IsLooping ? currentSequence.Length - 1 : 0;

            arguments.CurrentFrame = CurrentFrame;
            OnFrameChange?.Invoke(this, arguments);
            elapsedTime = 0;
        }
    }
}
