using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStation_CourseWork_Interface
{
    internal class Bus
    {
        string route;
        public Time dTimeArrive = new Time();
        int quantity = 0;
        int places;

        public Time actualTime = new Time();
        public Time nextTime = new Time();

        public int Places
        {
            set { places = exception.Exception(value, exception.ExceptionsType.quantityCustomer); }
            get { return places; }
        }
        public string Route
        {
            set { route = value; }
            get { return route; }
        }


        public int Quantity
        {
            set { quantity = exception.Exception(value, exception.ExceptionsType.quantityCustomer); }
            get { return quantity;}
        }
    }

    internal class Buses
    {
        static public List<Bus> buses = new List<Bus>();
    }
}
