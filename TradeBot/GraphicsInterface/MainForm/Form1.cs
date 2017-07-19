using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core0.library;

namespace MainForm
{
    public partial class Form1 : Form
    {

        public static int place_orders_count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //fill algorithm selection combo box

            comboBox_Algorithms.SelectedIndex = 0;

            // Use the Text property for the button text for all cells rather
            // than using each cell's value as the text for its own button.

        }

        private void button_start_trade_Click(object sender, EventArgs e)
        {
            //Launch the new thread
            if(textBox_ticker.Text.Trim() == "" )
            {
                textBox_ticker.BackColor = Color.Red;

                return;
            }


            if (textBox_stock_num.Text.Trim() == ""  )
            {
                textBox_stock_num.BackColor = Color.Tomato;

                return;
            }
            try
            {
                int stock_count = int.Parse(textBox_stock_num.Text);
                ThreadManager.LaunchTradingThread(textBox_ticker.Text, stock_count, place_orders_count );
                //Add the entry in data grid

                place_orders_count++;

                this.dataGridView_tradeLists.Rows.Add(place_orders_count, textBox_ticker.Text, "six", "seven", "eight");
                
            }
            catch( FormatException fe)
            {
                textBox_stock_num.BackColor = Color.YellowGreen;
                return;
            }

          
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView_tradeLists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dataGridView_tradeLists.Columns[e.ColumnIndex].Name == "Exit")
            {
                int rowIndex = Convert.ToInt32(e.RowIndex); // Get the current row
                int bigStore = Convert.ToInt32(dataGridView_tradeLists.Rows[rowIndex].Cells[0].Value);

                ThreadManager.ExitTradingThread(rowIndex);
              

                dataGridView_tradeLists.Rows[rowIndex].ReadOnly = true;

            }
            else
            {
                int row = 0;
                //Draw Graph

            }

        }
    }
}
