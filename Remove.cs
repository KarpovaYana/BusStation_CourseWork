using BusStation_CourseWork_Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusStation_CourseWork_Interface
{
    public partial class Remove : Form
    {
        public Remove()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show($"tickets Quantity: {Tickets.Quantity}\n textbox: {Convert.ToInt32(textBox1.Text)}");
            if(Convert.ToInt32(textBox1.Text) <= 0 || Convert.ToInt32(textBox1.Text)>Tickets.tickets.Count) { MessageBox.Show("Такого номера нет! Пожалуйста, введите заново", "Ошибка"); return; }

            try { Tickets.percent -= Tickets.tickets[Convert.ToInt32(textBox1.Text) - 1].Percent; }
            catch(Exception) { MessageBox.Show("ERROR"); }

            Tickets.tickets.RemoveAt(Convert.ToInt32(textBox1.Text) - 1);
            Tickets.SAVEDtickets.RemoveAt(Convert.ToInt32(textBox1.Text) - 1);
            MessageBox.Show("Маршрут удален", "Успешно");
           
        }
    }
}
