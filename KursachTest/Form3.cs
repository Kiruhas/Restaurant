using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class Form3 : Form
    {

        List<Product> productList = new List<Product>();
        

        public Form3()
        {
            InitializeComponent();
            List<string> products = new List<string>();
            Functions.GetProducts(products);
            for (int i = 0; i < products.Count; i++)
            {
                comboBox4.Items.Add(products[i]);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CheckNewSotr();
        }

        private void ClearStaffBoxes()
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void ClearDishBoxes()
        {
            textBox2.Clear();
            textBox6.Clear();
            comboBox2.SelectedIndex = -1;
            textBox7.Clear();
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            foreach (ListViewItem item in listView1.Items) listView1.Items.Remove(item);
        }
        private void CheckNewSotr()
        {
            string stazh = textBox3.Text.Trim();
            bool isNum = int.TryParse(stazh, out _);

            if (textBox1.Text.Length - 1 > 6)
            {
                if (comboBox1.SelectedItem != null)
                {
                    if (isNum)
                    {
                        if (textBox4.Text.Length > 4)
                        {
                            if (textBox5.Text.Length > 5)
                            {
                                NewSotr();
                                ClearStaffBoxes();
                                Functions.UpdateConnect();
                            } else
                            {
                                label11.Visible = true;
                                textBox5.BackColor = Color.FromArgb(255, 192, 192);
                            }
                        } else
                        {
                            label10.Visible = true;
                            textBox4.BackColor = Color.FromArgb(255, 192, 192);
                        }
                    }
                    else
                    {
                        label9.Visible = true;
                        textBox3.BackColor = Color.FromArgb(255, 192, 192);
                    }
                }
                else
                {
                    label8.Visible = true;
                    comboBox1.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
            else
            {
                label7.Visible = true;
                textBox1.BackColor = Color.FromArgb(255, 192, 192);
            }
        }

        private void NewSotr()
        {
            string name = textBox1.Text, dolzhn = comboBox1.SelectedItem.ToString(),
                stazh = textBox3.Text, login = textBox4.Text.ToLower(),
                pass = Functions.GetHash(textBox5.Text);
            Functions.AddStaff(name, dolzhn, stazh, login, pass);

            panel2.Visible = true;
            panel2.BringToFront();
            aTimer.Interval = 1000;
            aTimer.Tick += new EventHandler(OnTimeEvent);
            aTimer.Start();
        }

        Timer aTimer = new Timer();
        int j = 0;
        public void HidePanel()
        {
            if (j == 1)
            {
                panel2.Visible = false;
                aTimer.Stop();
            }
            else
                j++;
        }
        private void OnTimeEvent(Object source, EventArgs e)
        {
            HidePanel();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            form1.Close();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            label7.Visible = false;
            textBox1.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            label9.Visible = false;
            textBox3.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            label10.Visible = false;
            textBox4.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            label11.Visible = false;
            textBox5.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label8.Visible = false;
            comboBox1.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void Button3_Click(object sender, EventArgs e) // Кнопка добавить продукт
        {
            AddProductToList();
        }

        private void AddProductToList()
        {
            if (comboBox4.SelectedItem != null & comboBox3.SelectedItem != null)
            {
                Product item = new Product();
                item.name = comboBox4.SelectedItem.ToString();
                item.count = Convert.ToDouble(comboBox3.SelectedItem.ToString());
                bool isIn = false;
                for (int i=0; i < productList.Count; i++)
                {
                    if (productList[i].name == comboBox4.SelectedItem.ToString())
                        isIn = true;
                }
                if (!isIn)
                {
                    productList.Add(item);
                    string[] items = { comboBox4.SelectedItem.ToString(),
                                       comboBox3.SelectedItem.ToString() };
                    listView1.Items.Add(new ListViewItem(items));
                }
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string stazh = textBox6.Text.Trim();
            bool isPrice = int.TryParse(stazh, out _);

            stazh = textBox7.Text.Trim();
            bool isWeight = int.TryParse(stazh, out _);

            if (textBox2.Text.Length > 3)
            {
                if (isPrice)
                {
                    if (isWeight)
                    {
                        if (comboBox2.SelectedItem != null)
                        {
                            if (listView1.Items.Count != 0)
                            {
                                Functions.AddDish(textBox2.Text, 
                                                  Convert.ToDouble(textBox6.Text),
                                                  Convert.ToDouble(textBox7.Text), 
                                                  comboBox2.Text,
                                                  productList);
                                Functions.UpdateConnect();
                                ClearDishBoxes();
                            } else
                            {
                                label25.Visible = true;
                                listView1.BackColor = Color.FromArgb(255, 192, 192);
                                comboBox4.BackColor = Color.FromArgb(255, 192, 192);
                            }
                        } else
                        {
                            label24.Visible = true;
                            comboBox2.BackColor = Color.FromArgb(255, 192, 192);
                        }
                    } else
                    {
                        label23.Visible = true;
                        textBox7.BackColor = Color.FromArgb(255, 192, 192);
                    }
                } else
                {
                    label22.Visible = true;
                    textBox6.BackColor = Color.FromArgb(255, 192, 192);
                }
            } else
            {
                label21.Visible = true;
                textBox2.BackColor = Color.FromArgb(255, 192, 192);
            }
            
        }

        private void TextBox2_TextChanged_1(object sender, EventArgs e)
        {
            label21.Visible = false;
            textBox2.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            label22.Visible = false;
            textBox6.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            label23.Visible = false;
            textBox7.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label24.Visible = false;
            comboBox2.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            label25.Visible = false;
            comboBox4.BackColor = Color.FromArgb(228, 209, 211);
            listView1.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collection = listView1.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView1.SelectedIndices[0];
                string name = listView1.Items[select].SubItems[0].Text;
                listView1.Items.RemoveAt(collection[0]);
                for (int i=0; i < productList.Count; i++)
                {
                    if (productList[i].name == name)
                        productList.RemoveAt(i);
                }
            }
        }
    }
}
