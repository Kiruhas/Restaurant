using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class AddNewClient : UserControl
    {
        public AddNewClient()
        {
            InitializeComponent();
            Functions.client = this;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text, tel = textBox2.Text;
            bool yesTel = false, yesAge = false, yesSale = false;
            if (textBox2.TextLength != 0)
                yesTel = double.TryParse(textBox2.Text, out _);
            if (textBox3.TextLength != 0)
                yesAge = int.TryParse(textBox3.Text, out _);
            if (textBox4.TextLength != 0)
                yesSale = int.TryParse(textBox4.Text, out _);
            if (name.Length > 5)
            {
                if (yesTel)
                {
                    if (yesAge)
                    {
                        if (yesSale)
                        {
                            int age = Convert.ToInt32(textBox3.Text);
                            int sale = Convert.ToInt32(textBox4.Text);
                            Functions.AddNewClient(name, tel, age, sale);

                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            SendToBack();
                        }
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            SendToBack();
        }
    }
}
