using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace JonnyHammer.Engine
{

    public class CoroutineTask
    {
        IEnumerator Routine { get; }
        ICoroutineWaitable Return { get; set; }

        public bool Done { get; private set; }

        public CoroutineTask(IEnumerator routine, ICoroutineWaitable coroutineReturn = null)
        {
            Routine = routine;
            Return = coroutineReturn;
        }

        public void Update(GameTime gameTime)
        {
            if (Return != null && Return.ShouldWait())
            {
                Return.Update(gameTime);
                return;
            }

            Next();

        }

        void Next()
        {
            Done = !Routine.MoveNext();
            Return = ICoroutineWaitable.TryParseData(Routine.Current);
        }

    }

    public interface ICoroutineWaitable
    {
        bool ShouldWait();

        void Update(GameTime gameTime);

        static ICoroutineWaitable TryParseData(object waitable) =>
            waitable switch
            {
                ICoroutineWaitable x => x,
                TimeSpan t => new WaitTime(t),
                IEnumerator c => new WaitCoroutine(c),
                int n => new WaitFrames(n),
                _ => null,
            };


    }
    public class WaitCoroutine : ICoroutineWaitable
    {
        CoroutineTask manager;

        public WaitCoroutine(IEnumerator coroutine)
        {
            this.manager = new CoroutineTask(coroutine);
        }

        public bool ShouldWait() => !manager.Done;

        public void Update(GameTime gameTime)
        {
            manager.Update(gameTime);
        }
    }
    public class WaitTime : ICoroutineWaitable
    {
        TimeSpan currentTime = TimeSpan.Zero;

        public WaitTime(TimeSpan time) => currentTime = time;

        public bool ShouldWait() => currentTime > TimeSpan.Zero;
        public void Update(GameTime gameTime) => currentTime -= gameTime.ElapsedGameTime;
    }

    public class WaitFrames : ICoroutineWaitable
    {
        int frames = 0;

        public WaitFrames(int frames) => this.frames = frames;

        public bool ShouldWait() => frames > 0;

        public void Update(GameTime gameTime) => frames -= 1;
    }

    public class WaitUntil : ICoroutineWaitable
    {
        protected Func<bool> predicate = null;

        public WaitUntil(Func<bool> predicate) => this.predicate = predicate;

        public virtual bool ShouldWait() => !predicate();

        public void Update(GameTime gameTime) { }
    }
    public class WaitWhile : WaitUntil
    {
        public WaitWhile(Func<bool> predicate) : base(predicate) { }

        public override bool ShouldWait() => base.ShouldWait();
    }
}
