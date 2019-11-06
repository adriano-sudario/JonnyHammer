using System;
using System.Collections;

namespace JonnyHammer.Engine.Entities
{
    public class CoroutineTask
    {
        public CoroutineTask(IEnumerator coroutine, TimeSpan wait)
        {
            Routine = coroutine;
            WaitTime = wait;
        }

        public IEnumerator Routine { get; }
        public TimeSpan WaitTime { get; set; }
    }
}
