using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusStation_CourseWork_Interface
{
    [Serializable]
    public class Data
    {
        public int StartWorkH { get; set; } public int StartWorkMin { get; set; }
        public int EndWorkH { get; set; }
        public int EndWorkMin { get; set; }
        public int StartBreakH { get; set; }
        public int StartBreakMin { get; set; }
        public int EndBreakH { get; set; }
        public int EndBreakMin { get; set; }

        public int quantityCustomers { get; set; }
        public double percentBenefits { get; set; }
        public int quantityCashiers { get; set; }
        public string Route { get; set; }
        public int quantityRouts { get; set; }
        public double percentCustomers { get; set; }

    }
}
