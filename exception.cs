using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStation_CourseWork_Interface
{
    static public class exception
    {
        public enum ExceptionsType
        {
            quantityTicket,
            stWorkH, stWorkM,
            endWorkH, endWorkM,
            stBreakH, stBreakM,
            endBreakH, endBreakM,

            quantityCashier,
            quantityCustomer,

            indexTickets
        }

        static public int Exception(int a, ExceptionsType x)
        {
            
            if (x == ExceptionsType.quantityTicket)
            {
                if (a < 0) throw new ArgumentException();
                else return a;
            }

            if (x == ExceptionsType.quantityCashier)
            {
                if (a < 0) throw new ArgumentException();
                else return a;
            }

            if (x == ExceptionsType.quantityCustomer)
            {
                if (a < 0) throw new ArgumentException();
                else return a;
            }

            if (x == ExceptionsType.stWorkH || x== ExceptionsType.endWorkH || x == ExceptionsType.stBreakH || x == ExceptionsType.endBreakH)
            {
                if (a < 0 && a >= 24) throw new ArgumentException();
                else return a;
            }

            if (x == ExceptionsType.stWorkM || x == ExceptionsType.endWorkM || x == ExceptionsType.stBreakM || x == ExceptionsType.endBreakM)
            {
                if (a < 0 && a >= 60) throw new ArgumentException();
                else return a;
            }

            if(x== ExceptionsType.indexTickets)
            {
                if (a < 0 && a > Tickets.tickets.Count) throw new ArgumentException();
                else return a;
            }
            return a;
        }
    }
}
