using BusStation_CourseWork_Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using ClosedXML.Excel;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;



namespace BusStation_CourseWork_Interface
{
    public partial class Form1 : Form
    {

        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        string xml = "";
        public Form1()
        {
            InitializeComponent();

           

            if (File.Exists("data.json"))           //deserialization
            { 
                string data = File.ReadAllText("data.json");
                Data a = JsonSerializer.Deserialize<Data>(data);
                textBoxStartWorkH.Text = Convert.ToString(a.StartWorkH);
                textBoxEndWorkH.Text = Convert.ToString(a.EndWorkH);
                if(a.StartWorkMin<10) textBoxStartWorkM.Text ="0" + Convert.ToString(a.StartWorkMin);
                else textBoxStartWorkM.Text = Convert.ToString(a.StartWorkMin);
                if(a.EndWorkMin<10) textBoxEndWorkM.Text = "0"+Convert.ToString(a.EndWorkMin);
                else textBoxEndWorkM.Text = Convert.ToString(a.EndWorkMin);

                textBoxStartBreakH.Text = Convert.ToString(a.StartBreakH);
                textBoxEndBreakH.Text = Convert.ToString(a.EndBreakH);
                if(a.StartBreakMin<10) textBoxStartBreakM.Text = "0"+ Convert.ToString(a.StartBreakMin);
                else textBoxStartBreakM.Text = Convert.ToString(a.StartBreakMin);
                if(a.EndBreakMin<10) textBoxEndBreakM.Text = "0"+Convert.ToString(a.EndBreakMin);
                else textBoxEndBreakM.Text = Convert.ToString(a.EndBreakMin);

                textBoxCustomerCount.Text = Convert.ToString(a.quantityCustomers);
                textBoxBenefitPercent.Text = Convert.ToString(a.percentBenefits);

                textBoxCashierCount.Text = Convert.ToString(a.quantityCashiers);
                textBoxRouteName.Text = a.Route;

                textBoxRouteCount.Text = Convert.ToString(a.quantityRouts);
                textBoxCustomerPercent.Text = Convert.ToString(a.percentCustomers);
            }
    }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        List<string> results = new List<string>();

        private void tickets_AddCustomers()
        {
            for(int i=0; i<Tickets.tickets.Count; i++)
            {
                if(i== Tickets.tickets.Count)
                {
                    Tickets.tickets[i].Customers = Convert.ToInt32(textBoxCashierCount.Text) - Tickets.customersSum();
                    break;
                }
                Tickets.tickets[i].Customers = Tickets.tickets[i].Customers * Convert.ToInt32(textBoxCashierCount.Text);
            }
        }

        void analysis()                         //result of modeling
        {
            if (Tickets.ticketsForAdd.Count != 0)
            {
                label21.Text += "Не хватило следущих билетов: ";
                for (int i = 0; i < Tickets.ticketsForAdd.Count; i++)
                {
                    if (i > 0) label21.Text += ",\n";
                    label21.Text += $"{Tickets.ticketsForAdd[i].Route}({Tickets.ticketsForAdd[i].Customers} шт)";
                }
            }

            if(Tickets.ticketsForRemove.Count != 0)
            {
                label21.Text += "\nОстались следущие билеты: ";
                for (int i = 0; i < Tickets.ticketsForRemove.Count; i++)
                {
                    if (i > 0) label21.Text += ",\n";
                    label21.Text += $"{Tickets.ticketsForRemove[i].Route}({Tickets.ticketsForRemove[i].Quantity} шт)";
                }
            }

            if(WAITcustomers.customers.Count!=0)
            {
                label21.Text += $"\nНа автобус не успело {WAITcustomers.customers.Count} покупателей";
            }

            label21.Text += "\nНеобслужанные клиенты: ";
            label21.Text += $"{Customer.Quantity_Serviced}";
        }

        void Clear_Information()            //clear information when data was changed
        {
            for(int i=Cashiers.cashiers.Count-1; i>=0; i--) Cashiers.cashiers.RemoveAt(i);
            Cashiers.Quantity = 0;

            Customer.Quantity_forQueue = 0;
            Customer.Quantity_Serviced = 0;

            usualCustomer.QuantityUsual = 0;
            benefitCustomer.QuantityBenefit = 0;

            for(int i= WAITcustomers.customers.Count-1; i>=0;i--) WAITcustomers.customers.RemoveAt(i);
        }

        int checkTime(int hour1, int min1, int hour2, int min2)         //check if endTime bigger than startTime
        {
            if (hour1 > hour2) return 0;
            if(hour1==hour2)
            {
                if (min1 > min2) return 0;
                if (min1 == min2) return 2;
                return 1;
            }
            return 1;
        }

        //checking if BreakTime is between WorkTime
        int checkTimeBreak() 
        {
            if (checkTime(Time.StartWorkH, Time.StartWorkMin, Time.StartBreakH, Time.StartBreakMin) == 0) return 0;
            if(checkTime(Time.EndBreakH, Time.EndBreakMin, Time.EndWorkH, Time.EndWorkMin) ==0) return 0;
            return 1;
        }

        //Check all Time
        int checkingTime()
        {
            if (checkTime(Time.StartWorkH, Time.StartWorkMin, Time.EndWorkH, Time.EndWorkMin) == 0) return 0;
            if (checkTime(Time.StartWorkH, Time.StartWorkMin, Time.EndWorkH, Time.EndWorkMin) == 2)
            {
                MessageBox.Show("Время работы равно 0. Пожалуйста, увеличьте время работы");
                return 2;
            }
            if (checkTime(Time.StartBreakH, Time.StartBreakMin, Time.EndBreakH, Time.EndBreakMin) == 0) return 0;
            if (checkTimeBreak() == 0) return 0;
            return 1;
        }
        void serialize()
        {
            Data b = new Data
            {
                StartWorkH = Convert.ToInt32(textBoxStartWorkH.Text),
                EndWorkH = Convert.ToInt32(textBoxEndWorkH.Text),
                StartWorkMin = Convert.ToInt32(textBoxStartWorkM.Text),
                EndWorkMin = Convert.ToInt32(textBoxEndWorkM.Text),

                StartBreakH = Convert.ToInt32(textBoxStartBreakH.Text),
                EndBreakH = Convert.ToInt32(textBoxEndBreakH.Text),
                StartBreakMin = Convert.ToInt32(textBoxStartBreakM.Text),
                EndBreakMin = Convert.ToInt32(textBoxEndBreakM.Text),

                quantityCustomers = Convert.ToInt32(textBoxCustomerCount.Text),
                percentBenefits = Convert.ToInt32(textBoxBenefitPercent.Text),
                quantityCashiers = Convert.ToInt32(textBoxCashierCount.Text),
                Route = textBoxRouteName.Text,
                quantityRouts = Convert.ToInt32(textBoxRouteCount.Text),
                percentCustomers = Convert.ToInt32(textBoxCustomerPercent.Text)
        };
            string personJson = JsonSerializer.Serialize(b, typeof(Data));
            StreamWriter file = File.CreateText("data.json");
            file.WriteLine(personJson);
            file.Close();

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxStartWorkH.Text=="" || textBoxStartWorkM.Text == "" || textBoxEndWorkH.Text == "" || textBoxEndWorkM.Text == "" || textBoxStartBreakH.Text == "" || textBoxStartBreakM.Text == "" || textBoxEndBreakH.Text == "" || textBoxEndBreakM.Text == "" || textBoxCashierCount.Text=="" || textBoxCustomerCount.Text=="")
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
                return;
            }
            if(Tickets.percent<100)
            {
                MessageBox.Show("Сумма процентов не равна 100\nПожалуйста, добавьте ещё маршруты,\nчтобы сумма процентов покупателей стала равна 100%", "Ошибка");
                return;
            }

            //For updating data
            Clear_Information();
            
            try
            {
                Time.StartWorkH = Convert.ToInt32(textBoxStartWorkH.Text);
                Time.EndWorkH = Convert.ToInt32(textBoxEndWorkH.Text);
                Time.StartWorkMin = Convert.ToInt32(textBoxStartWorkM.Text);
                Time.EndWorkMin = Convert.ToInt32(textBoxEndWorkM.Text);

                Time.StartBreakH = Convert.ToInt32(textBoxStartBreakH.Text);
                Time.StartBreakMin = Convert.ToInt32(textBoxStartBreakM.Text);
                Time.EndBreakH = Convert.ToInt32(textBoxEndBreakH.Text);
                Time.EndBreakMin = Convert.ToInt32(textBoxEndBreakM.Text);

                Cashiers.Quantity = Convert.ToInt32(textBoxCashierCount.Text);
                Customer.Quantity_forQueue = Convert.ToInt32(textBoxCustomerCount.Text);
                Customer.Quantity_Serviced = Convert.ToInt32(textBoxCustomerCount.Text);
                benefitCustomer.QuantityBenefit = Convert.ToInt32(textBoxBenefitPercent.Text)*Convert.ToInt32(textBoxCustomerCount.Text)/100;
                usualCustomer.QuantityUsual = Convert.ToInt32(textBoxCustomerCount.Text) - benefitCustomer.QuantityBenefit;
                if(checkingTime()==0) { MessageBox.Show("Некорректное время.\nПожалйста, перепроверьте время и исправьте его", "Ошибка"); return; }
                if (checkingTime() == 2) return;
                tickets_AddCustomers();
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Неверные данные!", "Ошибка");
                return;
            }
            
            //Fill Cashier List
            for (int i=0; i<Cashiers.Quantity; i++)
            {
                Cashier a = new Cashier();

                Cashiers.cashiers.Add(a);
            }

            Time StartWork = new Time();
            Time EndWork = new Time();


            //Count up Customers for all Tickets
            int customer_count = 0;
            for(int i=0; i<Tickets.tickets.Count-1;i++)
            {
                Tickets.tickets[i].Customers = (int)(Customer.Quantity_forQueue * Tickets.tickets[i].Percent/100); customer_count += Tickets.tickets[i].Customers;

            }
            if(Tickets.tickets.Count>0) Tickets.tickets[Tickets.tickets.Count - 1].Customers = (int)(Customer.Quantity_forQueue - customer_count);



            try 
            {
                StartWork.Hour = Time.StartWorkH; StartWork.Min = Time.StartWorkMin;
                EndWork.Hour = Time.EndWorkH; EndWork.Min = Time.EndWorkMin;
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Неверное время!", "Ошибка");
                return;
            }

            running(StartWork, EndWork);

            button3.Enabled = true;
            button4.Enabled = false;
            button2.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            изменитьДанныеToolStripMenuItem.Enabled = true;
            удалитьМаршрутToolStripMenuItem.Enabled = false;
            label21.Text = "";
        }

        public void AddToTable()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Tickets.tickets.Count; i++)
            {
                DataGridViewTextBoxCell number = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell name = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell count = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell percent = new DataGridViewTextBoxCell();

                number.Value = i+1;
                name.Value = Tickets.tickets[i].Route;
                count.Value = Tickets.tickets[i].Quantity;
                percent.Value = Tickets.tickets[i].Percent;

                DataGridViewRow row0 = new DataGridViewRow();
                row0.Cells.AddRange(number, name, count,percent);
                dataGridView1.Rows.Add(row0);
            }
        }

        //Modeling
        void running(Time startWork, Time endWork)
        {

            timeCurr.time.Hour = Time.StartWorkH;
            timeCurr.time.Min = Time.StartWorkMin;

            Time startBreak = new Time();
            Time endBreak = new Time();
            startBreak.Hour = Time.StartBreakH; startBreak.Min = Time.StartBreakMin;
            endBreak.Hour = Time.EndBreakH; endBreak.Min = Time.EndBreakMin;

            Time customerArrived = Time.customerArriveNext(timeCurr.time, Time.timeIncrease(timeCurr.time, 30));
            List <Time> timeserviceArr = new List <Time>();
            timeserviceArr = Time.initialize_TimeArrived(timeserviceArr, Cashiers.Quantity);
            int CustomersQuantity = Convert.ToInt32(textBoxCustomerCount.Text);


            //Initialize Time when Bus will arrive
            for(int i=0; i<Buses.buses.Count; i++)
            {
                Buses.buses[i].actualTime = Time.timeIncrease(startWork, (startWork-endWork)/CustomersQuantity);
                try
                {
                    Buses.buses[i].nextTime = Buses.buses[i].actualTime + Buses.buses[i].dTimeArrive;
                }
                catch { return; }
            }

            //Start Work
            for (; timeCurr.time<endWork; timeCurr.time++)
            {
                //if it is BreakTime now, nothing happen
                if (timeCurr.time == startBreak) timeCurr.time = ++endBreak;

                //Customer arrived
                if(timeCurr.time == customerArrived)
                {
                    //define when Next Customer will arrive
                    customerArrived = Time.customerArriveNext(timeCurr.time, Time.timeIncrease(timeCurr.time, 0));

                    if (customerArrived > startBreak && customerArrived < endBreak) customerArrived = ++endBreak;
                    if (benefitCustomer.QuantityBenefit != 0)
                    {
                        Random rnd = new Random();
                        if (rnd.Next(0, 1) == 0)      //check if it's benefit customer
                        {
                            benefitCustomer a = new benefitCustomer();
                            Caller call = new Caller();
                            int index = call.call_goToCashier(a);     //go to queue
                            if (Customer.Quantity_forQueue!=0) Customer.Quantity_forQueue--;
                            if (a.PlaceInQueue==1)
                            {
                                a.timeService = Time.timeIncrease(timeCurr.time,rnd.Next(2, 4));
                                
                                timeserviceArr[index] = a.timeService;

                            }
                        }
                        else
                        {
                            usualCustomer a = new usualCustomer();
                            Caller call = new Caller();
                            int index = call.call_goToCashier(a);    //go to queue
                            Customer.Quantity_forQueue--;

                            if (a.PlaceInQueue == 1)
                            {
                                a.timeService = Time.timeIncrease(timeCurr.time, rnd.Next(1, 3));
                                timeserviceArr[index + 1] = a.timeService;
                            }
                        }
                    }
                    else if (usualCustomer.QuantityUsual != 0)
                    {

                        if(Customer.Quantity_forQueue!=0) Customer.Quantity_forQueue--;
                        usualCustomer a = new usualCustomer();
                        Caller call = new Caller();
                        int index = call.call_goToCashier(a);               //qo to queue
                        
                        

                        Random rnd = new Random();
                        if (a.PlaceInQueue == 1)
                        {
                            a.timeService = Time.timeIncrease(timeCurr.time, rnd.Next(1, 3));
                            timeserviceArr[index] = a.timeService;
                        }
                    }        //customer arrived

                }

               // Service
               for(int i=0; i<Cashiers.Quantity;i++)
                {

                    if (timeserviceArr[i].Hour == 0 && timeserviceArr[i].Min == 0) continue;
                    if (timeCurr.time == timeserviceArr[i])
                    {
                        int choose_route = Tickets.chooseRoute();
                        if (choose_route == -1) break;


                        if (Customer.Quantity_Serviced != 0) Customer.Quantity_Serviced--;
                        else break;
                        

                        Cashiers.cashiers[i].Queue--;
                        if(Cashiers.cashiers[i].customers.Count != 0)
                            Cashiers.cashiers[i].deleteCustomer(0);
                    }
                }

               for(int i=0; i<Buses.buses.Count;i++)
                {
                    
                    if (timeCurr.time == Buses.buses[i].actualTime)
                    {
                        for(int j=0; j<WAITcustomers.customers.Count; j++)
                        {
                            if (Buses.buses[i].Route== WAITcustomers.customers[j].route)
                            {
                                if (Buses.buses[i].Places == 0) break;

                                WAITcustomers.customers.RemoveAt(j);
                                Buses.buses[i].Places--;
                            }
                        }
                        Buses.buses[i].actualTime = Buses.buses[i].nextTime;
                        Buses.buses[i].nextTime += Buses.buses[i].dTimeArrive;
                        Buses.buses[i].Quantity--;
                        if (Buses.buses[i].Quantity == 0) Buses.buses.RemoveAt(i);
                    }
         
                }
            }

        }

        void clear_TicketsInformation()
        {
            for (int i = Tickets.tickets.Count - 1; i >= 0; i--) Tickets.tickets.RemoveAt(i);
            for (int i = Tickets.ticketsForRemove.Count - 1; i >= 0; i--) Tickets.ticketsForRemove.RemoveAt(i);
            for (int i = Tickets.ticketsForAdd.Count - 1; i >= 0; i--) Tickets.ticketsForAdd.RemoveAt(i);

        }

        //Checking, will all Buses be able to arrive  
        bool checkDTimeBus()
        {
            int factor = Convert.ToInt32(textBoxBusCount.Text);
            Time check = new Time();
            check.Hour = Convert.ToInt32(textBoxStartWorkH.Text);
            check.Min = Convert.ToInt32(textBoxStartWorkM.Text);
            Time endWork = new Time();
            endWork.Hour = Convert.ToInt32(textBoxEndWorkH.Text);
            endWork.Min = Convert.ToInt32(textBoxEndWorkM.Text);

            Time check_dTime = new Time();
            check_dTime.Hour = Convert.ToInt32(textBoxdTimeH.Text);
            check_dTime.Min = Convert.ToInt32(textBoxdTimeM.Text);
            for (int i = 0; i < factor; i++)
            {
                check += check_dTime;

                if (check > endWork) return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBoxRouteName.Text=="" || textBoxRouteCount.Text == "" || textBoxCustomerPercent.Text=="" || 
                textBoxRouteCount.Text == "" || textBoxdTimeH.Text == "" || textBoxdTimeM.Text == "") 
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
                return;

            }
            

            int customer_count = 0;

            Bus bus = new Bus();
            Ticket a = new Ticket();
            Ticket b = new Ticket();
            a.Route = textBoxRouteName.Text; 
            b.Route = textBoxRouteName.Text;

            try { a.Quantity = Convert.ToInt32(textBoxRouteCount.Text); b.Quantity = Convert.ToInt32(textBoxRouteCount.Text); }
            catch(Exception) { MessageBox.Show("Неверное значение количества!", "Ошибка"); return; }

            try { a.Percent = Convert.ToInt32(textBoxCustomerPercent.Text); b.Percent = Convert.ToInt32(textBoxCustomerPercent.Text); }
            catch (Exception) { MessageBox.Show("Неверное значение процента!", "Ошибка"); return; }
            bus.Route = textBoxRouteName.Text;

            if (!checkDTimeBus())
            {
                MessageBox.Show("Некорректное значение времени автобуса!\nЗа рабочий день такое кол-во автобусов не успеет приехать.\nПожалуйста, введите заново.");
                return;
            }
            if (Convert.ToInt32(textBoxdTimeH.Text) >= 0 && Convert.ToInt32(textBoxdTimeH.Text) < 24 && Convert.ToInt32(textBoxdTimeM.Text) >= 0 && Convert.ToInt32(textBoxdTimeM.Text) < 60)
            {
                bus.dTimeArrive.Hour = Convert.ToInt32(textBoxdTimeH.Text);
                bus.dTimeArrive.Min = Convert.ToInt32(textBoxdTimeM.Text);
            }
            else { MessageBox.Show("Некорректное значение времени автобуса!\nПожалуйста, введите заново.", "Ошибка"); return; }

            if(bus.dTimeArrive.Hour==0 && bus.dTimeArrive.Min==0) { MessageBox.Show("Время автобуса равно 0!\nПожалуйста, увеличьте время автобуса."); }

            try { bus.Quantity = Convert.ToInt32(textBoxRouteCount.Text); }
            catch (Exception) { MessageBox.Show("Неверное значение количества автобусов!", "Ошибка"); return; }

            try { bus.Places = Convert.ToInt32(textBoxPlacesInBus.Text); }
            catch (Exception) { MessageBox.Show("Неверное значение количества мест в автобусе!", "Ошибка"); return; }

            bool flag = false;

            //Count how much customers want to buy tickets with this route
            Tickets.percent += Convert.ToInt32(textBoxCustomerPercent.Text);
            if (Tickets.percent >= 100)
            {
                if(Tickets.percent == 100) { MessageBox.Show("Сумма процентов достигла 100%\nВы больше не можете добавить маршруты"); }
                if (Tickets.percent > 100) 
                {
                    flag = true; 
                    a.Percent = Convert.ToInt32(textBoxCustomerPercent.Text)-(Tickets.percent-100);
                    b.Percent = Convert.ToInt32(textBoxCustomerPercent.Text) - (Tickets.percent - 100);
                }
                a.Customers = Customer.Quantity_forQueue - customer_count;
                b.Customers = Customer.Quantity_forQueue - customer_count;
                Tickets.percent = 100;
                groupBox3.Enabled = false;
            }
            a.Customers = (int)(Customer.Quantity_forQueue * a.Percent); customer_count += a.Customers;
            b.Customers = (int)(Customer.Quantity_forQueue * b.Percent);

            Tickets.tickets.Add(a);
            Tickets.SAVEDtickets.Add(b);
            Buses.buses.Add(bus);
            
            удалитьМаршрутToolStripMenuItem.Enabled = true;
            MessageBox.Show("Маршрут добавлен\nЧтобы увидеть его в таблице,\nнажмите Показать актуальные маршруты", "Успех");
            if(flag) MessageBox.Show("Сумма процентов превысила 100%, поэтому для данного маршрута применится, чтобы сумма была равна 100\nВы больше не можете добать маршрут");
            
            //AddToTable();
    
        }

        private void удалитьМаршрутToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove remove = new Remove();
            remove.ShowDialog();
            if (Tickets.percent != 100) groupBox3.Enabled = true;
            if (Tickets.tickets.Count == 0) удалитьМаршрутToolStripMenuItem.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label21.Text = "";
            analysis();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private int count_Quantity()
        {
            int count = 0;
            for(int i=0; i<Tickets.tickets.Count; i++) count += Tickets.tickets[i].Quantity;
            return count;
        }

        private double count_Percent()
        {
            double sum_Percent = 0;
            for (int i = 0; i < Tickets.tickets.Count; i++) sum_Percent += Tickets.tickets[i].Percent;

            return sum_Percent;
        }

        private void equalTickets()
        {
            clear_TicketsInformation();
            for (int i=0; i<Tickets.SAVEDtickets.Count; i++)
            {
                Ticket a = new Ticket();
                a.Route = Tickets.SAVEDtickets[i].Route;
                a.Quantity = Tickets.SAVEDtickets[i].Quantity;
                a.Percent = Tickets.SAVEDtickets[i].Percent;
                a.Customers = Tickets.SAVEDtickets[i].Customers;
                Tickets.tickets.Add(a);
            }
        }
        private void изменитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            button4.Enabled = true;
            button3.Enabled = false;
            button2.Enabled = true;
            удалитьМаршрутToolStripMenuItem.Enabled = true;
            
            equalTickets();
            Tickets.Quantity = count_Quantity();
            изменитьДанныеToolStripMenuItem.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddToTable();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void сохранитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBoxStartWorkH.Text == "" || textBoxStartWorkM.Text == "" || textBoxEndWorkH.Text == "" || textBoxEndWorkM.Text == "" || 
                textBoxStartBreakH.Text == "" || textBoxStartBreakM.Text == "" || textBoxEndBreakH.Text == "" || textBoxEndBreakM.Text == "" || 
                textBoxCashierCount.Text == "" || textBoxCustomerCount.Text == "" || textBoxRouteName.Text == "" || textBoxRouteCount.Text == "" ||
                textBoxCustomerPercent.Text == "" ||
                textBoxRouteCount.Text == "" || textBoxdTimeH.Text == "" || textBoxdTimeM.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка");
                return;
            }
            serialize();
            MessageBox.Show("Данные сохранены", "Успешно");
        }
    }
}
