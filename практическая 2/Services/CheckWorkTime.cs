using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace практическая_2.Services
{
    internal class CheckWorkTime
    {
        public static bool CheckWork()
        {
            DateTime currentTime = DateTime.Now;

            TimeSpan startTime = new TimeSpan(10,0,0);
            TimeSpan endTime = new TimeSpan(19,0,0);

            bool isWork = currentTime.TimeOfDay >= startTime && currentTime.TimeOfDay <= endTime;

            return isWork;
        }

        public static string CheckDayTime()
        {
            TimeSpan startMorning = new TimeSpan(5,1,0);
            TimeSpan endMorning = new TimeSpan(12, 0, 0);

            TimeSpan startAfternoon = new TimeSpan(12, 1, 0);
            TimeSpan endAfternoon = new TimeSpan(17, 0, 0);

            TimeSpan startEvening = new TimeSpan(17, 1, 0);
            TimeSpan endEvening = new TimeSpan(21, 0, 0);

            TimeSpan startNight = new TimeSpan(21, 1,0);
            TimeSpan endNight = new TimeSpan(5, 0, 0);

            DateTime currentTime = DateTime.Now;

            string timeOfDay = null;

            if(startMorning <= currentTime.TimeOfDay && currentTime.TimeOfDay <= endMorning)
            {
                timeOfDay = "Доброе утро, ";
            }
            else if (startAfternoon <= currentTime.TimeOfDay && currentTime.TimeOfDay <= endAfternoon)
            {
                timeOfDay = "Добрый день, ";
            }
            else if (startEvening <= currentTime.TimeOfDay && currentTime.TimeOfDay <= endEvening)
            {
                timeOfDay = "Добрый вечер, ";
            }
            if (startNight <= currentTime.TimeOfDay && currentTime.TimeOfDay <= endNight)
            {
                timeOfDay = "Доброй ночи, ";
            }
            return timeOfDay;
        }
    }
}
