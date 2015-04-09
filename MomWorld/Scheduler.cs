using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MomWorld
{
    public class Scheduler : IHttpModule
    {
        protected static Timer timer;
        protected static object someAction;


        static Scheduler()
        {
            someAction = new object();
        }
        public void Init(HttpApplication application)
        {
            //there should only be one timer otherwise it would be messy
            if (timer == null)
            {
                var timerCallback = new TimerCallback(ProcessSomething);
                var startTime = 0;
                var timerInterval = 5000; // 5 seconds
                timer = new Timer(timerCallback, null, startTime, timerInterval);
            }
        }

        protected void ProcessSomething(object state)
        {

            //this protects everthing inside from other threads that might be invoking this
            //which is good for long running processes on the background
            lock (someAction)
            {
                System.Diagnostics.Debug.WriteLine("SomeText");
                //To simulate long running process
                System.Threading.Thread.Sleep(20000);
            }

        }
        public void Dispose() { }
    }
}