using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;

namespace BusStation_CourseWork_Interface
{
    public class Ticket
    {
        private string route;
        private int quantity;
        private int percent;
        private int customers;
        public string Route
        {
            set { route = value; }
            get { return route; }
        }

        public int Quantity
        {
            set { quantity = (int)exception.Exception(value, exception.ExceptionsType.quantityTicket); }
            get { return quantity; }
        }

        public int Percent
        {
            set { percent = (int)exception.Exception(value, exception.ExceptionsType.quantityTicket); }
            get { return percent; }
        }
        public int Customers
        {
            set { customers = value; }
            get { return customers; }
        }
    }

    internal class Tickets
    {
        static public List<Ticket> tickets = new List<Ticket>();
        static public List<Ticket> ticketsForAdd = new List<Ticket> ();
        static public List<Ticket> ticketsForRemove = new List<Ticket> ();
        static public List<Ticket> SAVEDtickets = new List<Ticket> () ;

        static public int quantity=0;
        static public int percent = 0;

        static public int Quantity
        {
            set { quantity = (value >= 0) ? value : throw new Exception(); }
            get { return quantity; }
        }
        static public int Percent
        {
            set { Percent = (value >= 0.0) ? value : throw new Exception(); }
            get {  return percent; }
        }

        static public int customersSum()
        {
            int sum = 0;

            for(int i=0; i<tickets.Count-1;i++)
                sum += tickets[i].Customers;

            return sum;
        }

        static void giveTicket(int index)
        {
            if(Tickets.tickets[index].Quantity !=0)
            {
                if(--Tickets.tickets[index].Quantity == 0)
                {
                    if (Tickets.tickets[index].Customers > 1)
                    {
                        Tickets.ticketsForAdd.Add(Tickets.tickets[index]);
                        tickets.RemoveAt(index);
                        return;
                    }
                    else if (Tickets.tickets[index].Customers != 0) --Tickets.tickets[index].Customers;
                }
            }

            if(Tickets.tickets[index].Customers!=0)
            {
                if(--Tickets.tickets[index].Customers==0)
                {
                    if (Tickets.tickets[index].Quantity > 1)
                    {
                        ticketsForRemove.Add(Tickets.tickets[index]);
                        tickets.RemoveAt(index);
                        return;
                    }

                    else if (Tickets.tickets[index].Quantity != 0) --Tickets.tickets[index].Quantity;
                }
            }
            WAITcustomer wait_customer = new WAITcustomer();
            wait_customer.route = Tickets.tickets[index].Route;
            WAITcustomers.customers.Add(wait_customer);
            if (Tickets.tickets[index].Customers == 0 && Tickets.tickets[index].Quantity == 0) tickets.RemoveAt(index);
           

        }
        static public int chooseRoute()
        {
            if (Tickets.tickets.Count == 0) return -1;
            Random rnd = new Random();
            int a = rnd.Next(0, Tickets.tickets.Count-1);

            giveTicket(a);
            return a;
        }

    }
}
