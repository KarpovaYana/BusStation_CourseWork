using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;

namespace BusStation_CourseWork_Interface
{
    internal class Time
    {
        static int startWorkH; static int startWorkMin;
        static int endWorkH; static int endWorkMin;

        static int startBreakH; static int startBreakMin;
        static int endBreakH; static int endBreakMin;

        

        int hour;
        int min;
        //PROPERTIES
        static public int StartWorkH
        {
            set { startWorkH = exception.Exception(value, exception.ExceptionsType.stWorkH); }
            get { return startWorkH; }
        }
        static public int StartWorkMin
        {
            set { startWorkMin = exception.Exception(value, exception.ExceptionsType.stWorkM); }
            get { return startWorkMin; }
        }
        static public int EndWorkH
        {
            set { endWorkH = exception.Exception(value, exception.ExceptionsType.endWorkH); }
            get { return endWorkH; }
        }
        static public int EndWorkMin
        {
            set { endWorkMin = exception.Exception(value, exception.ExceptionsType.endWorkM); }
            get { return endWorkMin; }
        }
        static public int StartBreakH
        {
            set { startBreakH = exception.Exception(value, exception.ExceptionsType.stBreakH); }
            get { return startBreakH; }
        }
        static public int StartBreakMin
        {
            set { startBreakMin = exception.Exception(value, exception.ExceptionsType.stBreakM); }
            get { return startBreakMin; }
        }
        static public int EndBreakH
        {
            set { endBreakH = exception.Exception(value, exception.ExceptionsType.endBreakH); }
            get { return endBreakH; }
        }
        static public int EndBreakMin
        {
            set { endBreakMin = exception.Exception(value, exception.ExceptionsType.endBreakM); }
            get { return endBreakMin; }
        }

        public int Hour
        {
            set { hour = (value>=0 && value<=24)? value: throw new ArgumentException(); }
            get { return hour; }
        }
        public int Min
        {
            set { min = (value >= 0 && value < 60) ? value : throw new ArgumentException(); }
            get { return min; }
        }

        public static bool operator >(Time ob1, Time ob2)
        {
            if (ob1.Hour > ob2.Hour) return true;

            if(ob1.Hour == ob2.Hour)
            {
                if(ob1.Min > ob2.Min) return true;
                return false;
            }
            return false;
        }
        public static bool operator <(Time ob1, Time ob2)
        {
            if (ob1.Hour < ob2.Hour) return true;
            else if(ob1.Hour == ob2.Hour)
            {
                if (ob1.Min < ob2.Min) return true;
                else return false;
            }
            return false;
        }

        public static Time operator ++(Time ob1)
        {
            if (ob1.Min == 59) { ob1.Min = 0; ob1.Hour++; }
            else ob1.Min++;

            return ob1;
        }

        public static Time operator +(Time ob1, Time ob2)
        {
            ob1.Hour += ob2.Hour;
            if(ob1.Min+ob2.Min>60)
            {
                int hour = (ob1.Min + ob2.Min) / 60;
                ob1.Hour += hour;

                ob1.Min = (ob1.Min + ob2.Min) - 60*hour;
            }

            return ob1;
        }

        public static int operator -(Time ob1, Time ob2)
        {
            int minutes=0;
            ob1.Hour = ob2.Hour - ob1.Hour;
            if (ob1.Min > ob2.Min) ob1.Hour--;
            minutes += ob1.Hour * 60;

            int a = ob2.Min - ob1.Min;
            minutes += Math.Abs(a);
            return minutes;
        }

        public static bool operator !=(Time ob1, Time ob2)
        {
            if ((ob1.Hour != ob2.Hour) || (ob2.Min != ob2.Min)) return true;
            return false;
        }
        public static bool operator ==(Time ob1, Time ob2)
        {
             if((ob1.Hour == ob2.Hour) &&(ob2.Min==ob2.Min)) return true;
            return false;
        }

        static public Time customerArriveNext(Time currTime, Time endWork)
        {
            Random rnd = new Random();

            int hour = rnd.Next(currTime.Hour, endWork.Hour);
            int min;

            if (hour == currTime.Hour) min = rnd.Next(currTime.Min, 59);
            else if(hour==endWork.Min) min = rnd.Next(0, endWork.Min);
            else min = rnd.Next(0, 59);

            Time customerArrivedAt = new Time();
            customerArrivedAt.Hour = hour;
            customerArrivedAt.Min = min;

            return customerArrivedAt;

        }

        static public Time timeIncrease(Time time, int min)
        {
            if (time.Min+min >= 60)
            {
                int Minute = time.Min + min;
                time.Hour += Minute / 60;
                time.Min = Minute - 60* Minute / 60;
            }
            else time.Min += min;

            return time;
        }

        static public List <Time> initialize_TimeArrived(List<Time> time, int index)
        {
            for(int i=0; i<index;i++)
            {
                Time a = new Time();
                a.Hour = 0;
                a.Min = 0;

                time.Add(a);
            }

            return time;
        }


    }

    internal class timeCurr
    {
        static public Time time = new Time();
    }
}
