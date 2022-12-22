using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static BusStation_CourseWork_Interface.exception;

namespace BusStation_CourseWork_Interface
{
    internal class Customer
    {
        
        static int quantity_forQueue;
        static int quantity_Serviced;

        protected int placeInQueue=0;

        public Time timeService;
        static public int Quantity_forQueue
        {
            set { quantity_forQueue = (int)Exception(value, ExceptionsType.quantityCustomer); }
            get { return quantity_forQueue; }
        }

        static public int Quantity_Serviced
        {
            set { quantity_Serviced = (int)Exception(value, ExceptionsType.quantityCustomer); }
            get { return quantity_Serviced; }
        }

        virtual public int goToCashier() { return 0; }

        public int PlaceInQueue
        {
            set { placeInQueue = value; }
            get { return placeInQueue; }
        }
    }

    internal class usualCustomer:Customer
    {
        static int quantityUsual;
       
        static public int QuantityUsual
        {
            set { quantityUsual = (int)Exception(value, ExceptionsType.quantityCustomer); }
            get { return quantityUsual; }
        }


        public override int goToCashier() 
        {
            placeInQueue = ++Cashiers.cashiers[Cashiers.findMinQueue()].Queue;

            Random random = new Random();

            int index = Cashiers.findMinQueue();
            Cashiers.cashiers[index].customers.Add(this);
            quantityUsual++;
            return index;

        }

    }

    internal class benefitCustomer : Customer
    {
        static int quantityBenefit;

        static public int QuantityBenefit
        {
            set { quantityBenefit = (int)Exception(value, ExceptionsType.quantityCustomer); }
            get { return quantityBenefit; }
        }
        public override int goToCashier()
        {
            placeInQueue = ++Cashiers.cashiers[Cashiers.findMinQueue()].Queue;

            Random random = new Random();
            
            quantityBenefit++;
            int index = Cashiers.findMinQueue();
            Cashiers.cashiers[index].customers.Add(this);
            return index;
        }
    }

    internal class Caller
    {
        public int call_goToCashier(Customer a)
        {
           return  a.goToCashier();
        }
    }
}
