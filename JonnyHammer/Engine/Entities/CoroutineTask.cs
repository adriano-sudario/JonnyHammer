using System;
using System.Collections;

namespace JonnyHammer.Engine.Entities
{
    public class CoroutineTask
    {
        public CoroutineTask(IEnumerator coroutine, TimeSpan wait, int waitFrames = 0)
        {
            Routine = coroutine;
            WaitTime = wait;
            WaitFrames = waitFrames;
        }

        public IEnumerator Routine { get; }
        public TimeSpan WaitTime { get; set; }
        public int WaitFrames { get; set; }
    }
}
