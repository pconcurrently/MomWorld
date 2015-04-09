using MomWorld.DataContexts;
using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Web;

namespace MomWorld
{
    public class Scheduler : IHttpModule
    {
        private SubscriberDb subscriberDb = new SubscriberDb();
        private NineMonthArticleDb nineMonthArticleDb = new NineMonthArticleDb();

        protected static Timer timer;
        protected static object someAction;


        #region SMS
        SerialPort port = new SerialPort();
        SMS objSMS = new SMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion


        static Scheduler()
        {
            someAction = new object();
        }
        public void Init(HttpApplication application)
        {
            if (timer == null)
            {
                var timerCallback = new TimerCallback(AutoSMSReminder);
                var startTime = 0;
                var timerInterval = 24 * 3600 * 1000; //  1 day
                timer = new Timer(timerCallback, null, startTime, timerInterval);
            }
        }

        protected void AutoSMSReminder(object state)
        {
            var subs = subscriberDb.Subscribers.ToList().FindAll(sub => sub.PhoneNumber != null && sub.DatePregnancy != null);

            //Input numbers
            string[] inputs = new string[] { "COM3", "9600", "8", "300", "300" };
            try
            {
                port = objSMS.OpenPort(inputs[0], inputs[1], inputs[2], inputs[3], inputs[4]);
                if (port == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                objSMS.ClosePort(this.port);
            }

            try
            {
                int date;
                var nines = nineMonthArticleDb.NineMonthArticles.ToList();
                foreach (var sub in subs)
                {
                    date = (sub.DatePregnancy.Value.Date - DateTime.Now.Date).Days;
                    if (date > 0)
                    {
                        for (var index = 0; index < nines.Count; index++)
                        {
                            if ((nines[index].Date < date && date < nines[index + 1].Date) && (date + 1) == nines[index + 1].Date)
                            {
                                objSMS.sendMsg(this.port, sub.PhoneNumber,
                        string.Format("Hay kiem tra tinh hinh mang thai cua ban tai http://localhost:4444/NineMonthArticles/Details/{0}", nines[index + 1].Id));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                objSMS.ClosePort(this.port);
            }

        }
        public void Dispose() { }
    }
}