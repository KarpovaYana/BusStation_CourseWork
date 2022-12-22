using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using static BusStation_CourseWork_Interface.exception;

namespace BusStation_CourseWork_Interface
{
    internal class Cashier
    {
        public List <Customer> customers = new List <Customer> ();
        int queue=0;

        public int Queue
        {
            set { queue = value; }
            get { return queue; }
        }

        public void deleteCustomer(int index)
        {
           // MessageBox.Show($"size: {customers.Count}\nindex: {index}");
            customers.RemoveAt(index);

            for (int i = 0; i < customers.Count; i++) customers[i].PlaceInQueue--;

        }
    }

    internal class Cashiers
    {
        static public List<Cashier> cashiers = new List<Cashier>();
        static int quantity;

        static public int Quantity
        {
            set
            {
                quantity = (int)Exception(value, ExceptionsType.quantityCashier); 
            }
            get { return quantity; }
        }

        static public int findMinQueue()
        {
            int minIndex = 0;
            for(int i=0; i < Cashiers.cashiers.Count; i++)
            {
                int min=int.MaxValue;

                if (Cashiers.cashiers[i].Queue < min) {min = Cashiers.cashiers[i].Queue; minIndex = i;}
            }

            return minIndex;
        }

        
    }
}
