using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace JonnyHammer.Engine
{

    public class CoroutineTask
    {
        IEnumerator Routine { get; }
        ICoroutineReturn Return { get; set; }

        public bool Done { get; private set; }

        public CoroutineTask(IEnumerator routine, ICoroutineReturn coroutineReturn = null)
        {
            Routine = routine;
            Return = coroutineReturn;
        }

        public void Update(GameTime gameTime)
        {
            if (Return != null && Return.ShouldWait())
            {
                Return.Step(gameTime);
                return;
            }

            Next();

        }

        void Next()
        {
            Done = !Routine.MoveNext();
            Return = ICoroutineReturn.TryParseData(Routine.Current);
        }

    }

    public interface ICoroutineReturn
    {
        bool ShouldWait();

        void Step(GameTime gameTime);

        static ICoroutineReturn TryParseData(object waitable) =>
            waitable switch
            {
                ICoroutineReturn x => x,
                TimeSpan t => new WaitTime(t),
                int n => new WaitFrames(n),
                _ => null,
            };


    }
    public class WaitTime : ICoroutineReturn
    {
        TimeSpan currentTime = TimeSpan.Zero;

        public WaitTime(TimeSpan time) => currentTime = time;

        public bool ShouldWait() => currentTime > TimeSpan.Zero;
        public void Step(GameTime gameTime) => currentTime -= gameTime.ElapsedGameTime;
    }
    public class WaitFrames : ICoroutineReturn
    {
        int frames = 0;

        public WaitFrames(int frames) => this.frames = frames;

        public bool ShouldWait() => frames > 0;

        public void Step(GameTime gameTime) => frames -= 1;
    }

    public class WaitUntil : ICoroutineReturn
    {
        protected Func<bool> predicate = null;

        public WaitUntil(Func<bool> predicate) => this.predicate = predicate;

        public virtual bool ShouldWait() => !predicate();

        public void Step(GameTime gameTime) { }
    }
    public class WaitWhile : WaitUntil
    {
        public WaitWhile(Func<bool> predicate) : base(predicate) { }

        public override bool ShouldWait() => base.ShouldWait();
    }
}
