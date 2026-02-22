using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public static class DateTimeExtensions
    {
        public static string ToDayString(this DateTime dt)
        {
            return $"{dt.DayOfWeek} ({dt:dd/MM/yyyy HH:mm:ss:fff})";
        }

        public static bool IsInRange(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }

        public static int AgeBasedOnDate(this DateTime birthDate)
        {
            DateTime dateNow = DateTime.Now;
            int age = dateNow.Year - birthDate.Year;
            if (dateNow.Month < birthDate.Month || (dateNow.Month == 
                birthDate.Month && dateNow.Day < birthDate.Day)) age--;

            return age;
        }
    }
}
