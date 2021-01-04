using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chamboco.Engine.Entities
{
    public class CoroutineManager
    {
        public bool Done => !coroutines.Any();

        IList<CoroutineTask> coroutines = new List<CoroutineTask>();
        public void UpdateCoroutines(GameTime gameTime)
        {
            for (var i = 0; i < coroutines.Count; i++)
            {
                var coroutine = coroutines[i];
                if (coroutine.Done)
                {
                    coroutines.Remove(coroutine);
                    continue;
                }

                coroutine.Update(gameTime);
            }
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            var c = new CoroutineTask(coroutine);
            coroutines.Add(c);
        }

        public void StartCoroutine(IEnumerator coroutine, TimeSpan wait)
        {
            var c = new CoroutineTask(coroutine, new WaitTime(wait));
            coroutines.Add(c);
        }

        public void StartCoroutine(IEnumerator coroutine, int numberOfFrames)
        {
            var c = new CoroutineTask(coroutine, new WaitFrames(numberOfFrames));
            coroutines.Add(c);
        }

        public void StopCoroutines() => coroutines.Clear();
        public void StopCoroutine(IEnumerator coroutine) =>
            coroutines = coroutines.Where(x => x.Routine != coroutine).ToList();
    }

    public class CoroutineTask
    {
        public IEnumerator Routine { get; }
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
        CoroutineManager manager;

        public WaitCoroutine(IEnumerator coroutine)
        {
            manager = new CoroutineManager();
            manager.StartCoroutine(coroutine);
        }

        public bool ShouldWait() => !manager.Done;

        public void Update(GameTime gameTime)
        {
            manager.UpdateCoroutines(gameTime);
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
        int frames;

        public WaitFrames(int frames) => this.frames = frames;

        public bool ShouldWait() => frames > 0;

        public void Update(GameTime gameTime) => frames -= 1;
    }

    public class WaitUntil : ICoroutineWaitable
    {
        Func<bool> predicate;

        public WaitUntil(Func<bool> predicate) => this.predicate = predicate;

        public virtual bool ShouldWait() => !predicate();

        public void Update(GameTime gameTime) { }
    }
    public class WaitWhile : WaitUntil
    {
        public WaitWhile(Func<bool> predicate) : base(predicate) { }
    }
}
