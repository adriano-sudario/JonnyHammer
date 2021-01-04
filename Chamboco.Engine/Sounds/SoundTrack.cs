using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Chamboco.Engine.Sounds
{
    public static class SoundTrack
    {
        static SoundEffectInstance song;
        static bool isFading;
        static float fadeIncrement;
        static Action onFadeEnded;

        public static bool HasEnded { get { return song.State == SoundState.Stopped; } }
        public static TimeSpan Duration { get; private set; }
        public static TimeSpan Position { get; private set; }
        public static bool IsPlaying { get { return song != null && song.State == SoundState.Playing; } }

        public static void Load(SoundEffect song, bool play = false, bool playOnLoop = true)
        {
            SoundTrack.song = song.CreateInstance();
            Duration = song.Duration;
            Position = TimeSpan.Zero;

            if (play)
                Play(playOnLoop);
        }

        public static void Play(bool playOnLoop = true)
        {
            song.IsLooped = playOnLoop;
            song.Play();
        }

        public static void Stop(bool withFadeOut = false)
        {
            Action stopSong = () =>
            {
                Position = TimeSpan.Zero;
                song.Stop();
            };

            if (withFadeOut)
                FadeOut(onFadeEnded: stopSong);
            else
                stopSong();
        }

        public static void FadeOut(float fadeIncrement = .05f, Action onFadeEnded = null)
        {
            Fade(-Math.Abs(fadeIncrement), onFadeEnded);
        }

        public static void FadeIn(float fadeIncrement = .05f, Action onFadeEnded = null)
        {
            Fade(Math.Abs(fadeIncrement), onFadeEnded);
        }

        private static void Fade(float fadeIncrement, Action onFadeEnded)
        {
            isFading = true;
            SoundTrack.onFadeEnded = onFadeEnded;
            SoundTrack.fadeIncrement = fadeIncrement;

            if (song == null)
            {
                StopFade();
            }
        }

        public static void StopFade()
        {
            isFading = false;
            onFadeEnded?.Invoke();
        }

        public static void Update(GameTime gameTime)
        {
            if (song == null || song.State != SoundState.Playing)
                return;

            Position += gameTime.ElapsedGameTime;

            if (Position > Duration)
                Position = Duration - Position;

            if (isFading)
            {
                float volume = song.Volume + fadeIncrement;
                song.Volume = MathHelper.Clamp(volume, 0, 1);

                if ((volume >= 1 && fadeIncrement > 0) || (volume <= 0 && fadeIncrement < 0))
                    StopFade();
            }
        }
    }
}
