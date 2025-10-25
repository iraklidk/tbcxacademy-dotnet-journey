namespace Practices
{
    internal class Clock
    {
        int _Hour, _Minute, _Second;
        public int Hour { 
            get
            {
                return _Hour;
            }
            set
            {
                if (value < 0 || value >= 24) Console.WriteLine("Invalid value");
                else _Hour = value;
            }
        }
        public int Minute 
        { 
            get
            {
                return _Minute;
            }
            set
            {
                if (value < 0 || value >= 60) Console.WriteLine("Invalid value");
                else _Minute = value;
            }
        }
        public int Second 
        { 
            get
            {
                return _Second;
            }
            set
            {
                if (value < 0 || value >= 60) Console.WriteLine("Invalid value");
                else _Second = value;
            }
        }

        public void GetCurrentTime()
        {
            Console.WriteLine((Hour < 10 ? "0" + Hour : Hour) + ":" + (Minute < 10 ? "0" + Minute : Minute) + ":" + (Second < 10 ? "0" + Second : Second));
        }

        public void AddSecond()
        {
            if (++_Second == 60)
            {
                _Second = 0;
                if (++_Minute == 60)
                {
                    _Minute = 0;
                    if (++_Hour == 24) _Hour = 0;
                }
            }
        }

        public void AddMinute()
        {
            if (++_Minute == 60)
            {
                _Minute = 0;
                if (++_Hour == 24) _Hour = 0;
            }
        }   

        public void AddHour()
        {
            if (++_Hour == 24) _Hour = 0;
        }

        public void decreaseSecond()
        {
            if (--_Second == -1)
            {
                _Second = 59;
                if (--_Minute == -1)
                {
                    _Minute = 59;
                    if (--_Hour == -1) _Hour = 23;
                }
            }
        }

        public void decreaseMinute()
        {
            if (--_Minute == -1)
            {
                _Minute = 59;
                if (--_Hour == -1) _Hour = 23;
            }
        }

        public void decreaseHour()
        {
            if (--_Hour == -1) _Hour = 23;
        }
    }
}