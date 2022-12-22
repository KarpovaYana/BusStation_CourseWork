using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStation_CourseWork_Interface
{
    internal class WAITcustomer
    {
       public string route;
    }

    internal class WAITcustomers
    {
        static public List<WAITcustomer> customers = new List<WAITcustomer>();
    }
}
