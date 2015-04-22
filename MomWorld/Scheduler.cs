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
        private IdentityDb identityDb = new IdentityDb();

        protected static Timer timer;
        protected static Timer timer2;
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
                var startTime = 15000;
                var timerInterval = 24 * 3600 * 1000; //  1 day
                timer = new Timer(timerCallback, null, startTime, timerInterval);

            }

            if (timer2 == null)
            {
                var startTime = 5000;
                var timerInterval2 = 3600 * 1000; // 1 hour
                var timerCallback2 = new TimerCallback(SMSUserTasks);
                timer2 = new Timer(timerCallback2, null, startTime, timerInterval2);
            }
        }

        protected void AutoSMSReminder(object state)
        {
            var subs = subscriberDb.Subscribers.ToList().FindAll(s => s.PhoneNumber != null && s.DatePregnancy != null);
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
                            SMSServices.Send(sub.PhoneNumber,
                    string.Format("Hay kiem tra tinh hinh mang thai cua ban tai http://localhost:4444/NineMonthArticles/Details/{0}", nines[index + 1].Id));
                        }
                    }
                }
            }


        }

        public void SMSUserTasks(object state)
        {
            var remindings = subscriberDb.UserTasks.ToList().FindAll(u => !u.IsCompleted && u.DueDate > DateTime.Now && u.DueDate < (DateTime.Now.AddHours(2)));

            foreach (var reminding in remindings)
            {
                var phone = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(reminding.UserName)).PhoneNumber;
                SMSServices.Send(phone,
                    string.Format("Vao luc: {0},ban co mot cong viec can hoan thanh: {1}", reminding.DueDate, reminding.Description));
            }

        }

        public void Dispose() { }

    }
}