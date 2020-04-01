using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class Form3 : Form
    {

        List<Product> productList = new List<Product>();
        List<Sklad> skladList = new List<Sklad>();
        List<Production> productionList = new List<Production>();
        List<DishWithComponent> dishList = new List<DishWithComponent>();

        public Form3()
        {
            InitializeComponent();
            List<string> products = new List<string>();
            Functions.GetProducts(products);
            for (int i = 0; i < products.Count; i++)
            {
                comboBox4.Items.Add(products[i]);
            }
            Functions.GetSklad(skladList);
            for(int i=0; i<skladList.Count; i++)
            {
                comboBox5.Items.Add(skladList[i].number.ToString());
            }
            Functions.GetProductions(productionList);
            for (int i=0; i< productionList.Count; i++)
            {
                string[] items = {productionList[i].name,
                                  productionList[i].sklad.ToString(),
                                  productionList[i].count.ToString()
                };
                listView2.Items.Add(new ListViewItem(items));
            }
            Functions.GetDishWithComponent(dishList);
            for (int i = 0; i < dishList.Count; i++)
            {
                string[] items = {dishList[i].name,
                                  dishList[i].price.ToString(),
                                  dishList[i].weight.ToString(),
                                  dishList[i].category.ToString()
                };
                listView3.Items.Add(new ListViewItem(items));
            }
        }

        /*BUTTONS*/
        private void Button1_Click(object sender, EventArgs e)
        {
            CheckNewSotr();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            form1.Close();
        }
        private void Button3_Click(object sender, EventArgs e) // Кнопка добавить продукт
        {
            AddProductToList();
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            string price = textBox6.Text.Trim();
            bool isPrice = false;
            if (price.Length != 0)
                isPrice = int.TryParse(price, out _);

            string weight = textBox7.Text.Trim();
            bool isWeight = false;
            if (weight.Length != 0)
                isWeight = int.TryParse(weight, out _);

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

                                DishWithComponent item = new DishWithComponent();
                                item.name = textBox2.Text;
                                item.price = Convert.ToInt32(textBox6.Text);
                                item.weight = Convert.ToInt32(textBox7.Text);
                                item.category = comboBox2.Text;
                                item.productList = new List<Product>();
                                dishList.Add(item);
                                for (int i = 0; i < productList.Count; i++)
                                {
                                    Product prod = new Product();
                                    prod.name = productList[i].name;
                                    prod.count = productList[i].count;
                                    dishList[dishList.Count - 1].productList.Add(prod);
                                }

                                string[] items = {item.name, item.price.ToString(),
                                                  item.weight.ToString(), item.category
                                };
                                listView3.Items.Add(new ListViewItem(items));
                                ClearDishBoxes();

                                panel6.Visible = true;
                                panel6.BringToFront();
                                aTimer.Interval = 1000;
                                aTimer.Tick += new EventHandler(OnTimeEvent);
                                aTimer.Start();
                            }
                            else
                            {
                                label25.Visible = true;
                                listView1.BackColor = Color.FromArgb(255, 192, 192);
                                comboBox4.BackColor = Color.FromArgb(255, 192, 192);
                            }
                        }
                        else
                        {
                            label24.Visible = true;
                            comboBox2.BackColor = Color.FromArgb(255, 192, 192);
                        }
                    }
                    else
                    {
                        label23.Visible = true;
                        textBox7.BackColor = Color.FromArgb(255, 192, 192);
                    }
                }
                else
                {
                    label22.Visible = true;
                    textBox6.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
            else
            {
                label21.Visible = true;
                textBox2.BackColor = Color.FromArgb(255, 192, 192);
            }

        }
        private void Button5_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collection = listView1.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView1.SelectedIndices[0];
                string name = listView1.Items[select].SubItems[0].Text;
                listView1.Items.RemoveAt(collection[0]);
                for (int i = 0; i < productList.Count; i++)
                {
                    if (productList[i].name == name)
                        productList.RemoveAt(i);
                }
            }
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            string count = textBox9.Text;
            string nameProd = textBox8.Text;

            bool isNumber = false, isUpdated = false;
            if (textBox9.TextLength != 0)
                isNumber = int.TryParse(count, out _);

            if (textBox8.TextLength != 0)
            {
                if (isNumber)
                {
                    if (comboBox5.SelectedItem != null)
                    {
                        string sklad = comboBox5.SelectedItem.ToString();
                        List<string> product = new List<string>();
                        for (int i = 0; i < productionList.Count; i++)
                        {
                            if (productionList[i].name == nameProd & productionList[i].sklad.ToString() == sklad)
                            {
                                productionList[i].count += Convert.ToInt32(count);
                                listView2.Items[i].SubItems[2].Text = productionList[i].count.ToString();
                                product.Add(nameProd);
                                product.Add(sklad);
                                product.Add(productionList[i].count.ToString());
                                Functions.UpdateProducts(product);
                                isUpdated = true;
                            }
                        }
                        if (!isUpdated)
                        {
                            Functions.AddProduct(nameProd, Convert.ToInt32(sklad), Convert.ToInt32(count));
                            string[] item = { nameProd, sklad, count };

                            listView2.Items.Add(new ListViewItem(item));
                        }
                        label41.Visible = true;
                        label41.BringToFront();
                        textBox8.Text = "";
                        textBox9.Text = "";
                        comboBox5.SelectedIndex = -1;
                        aTimer.Interval = 1000;
                        aTimer.Tick += new EventHandler(OnTimeEvent);
                        aTimer.Start();
                    }
                    else
                    {
                        comboBox5.BackColor = Color.FromArgb(255, 192, 192);
                    }
                }
                else
                {
                    textBox9.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
            else
            {
                label56.Visible = true;
                textBox8.BackColor = Color.FromArgb(255, 192, 192);
            }
        }
        private void Button7_Click(object sender, EventArgs e)
        {

        }
        private void Button7_Click_1(object sender, EventArgs e)
        {
            bool isNumber = false, isTemp = false;
            string number = textBox10.Text.Trim();
            if (textBox10.TextLength != 0)
                isNumber = int.TryParse(number, out _);

            string temp = textBox11.Text.Trim();
            if (textBox11.TextLength != 0)
                isTemp = int.TryParse(number, out _);

            bool isIT = false;

            if (isNumber)
            {
                if (isTemp)
                {
                    if ((Convert.ToInt32(temp) >= 0 & Convert.ToInt32(temp) <= 5))
                    {
                        for (int i = 0; i < skladList.Count; i++)
                        {
                            if (skladList[i].number.ToString() == textBox10.Text)
                            {
                                label39.Visible = true;
                                label39.BringToFront();
                                aTimer.Interval = 1000;
                                aTimer.Tick += new EventHandler(OnTimeEvent);
                                aTimer.Start();
                                isIT = true;
                            }
                        }
                        if (!isIT)
                        {
                            Functions.AddSklad(textBox10.Text, textBox11.Text);
                            label40.Visible = true;
                            label40.BringToFront();
                            textBox10.Text = "";
                            textBox11.Text = "";
                            aTimer.Interval = 1000;
                            aTimer.Tick += new EventHandler(OnTimeEvent);
                            aTimer.Start();
                            Functions.GetSklad(skladList);
                            comboBox5.Items.Clear();
                            for (int p = 0; p < skladList.Count; p++)
                            {
                                comboBox5.Items.Add(skladList[p].number.ToString());
                            }
                        }
                    }
                }
                else
                {
                    label34.Visible = true;
                    textBox11.BackColor = Color.FromArgb(255, 192, 192);
                }
            }
            else
            {
                label33.Visible = true;
                textBox10.BackColor = Color.FromArgb(255, 192, 192);
            }
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collection = listView2.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView2.SelectedIndices[0];

                string nameProd = listView2.Items[select].SubItems[0].Text;
                string sklad = listView2.Items[select].SubItems[1].Text;
                bool canDeleted = true;

                for (int i = 0; i < dishList.Count; i++)
                {
                    for (int j = 0; j < dishList[i].productList.Count; j++)
                    {
                        if (nameProd == dishList[i].productList[j].name)
                        {
                            canDeleted = false;
                            panel7.Visible = true;
                            panel7.BringToFront();
                            aTimer.Interval = 1000;
                            aTimer.Tick += new EventHandler(OnTimeEvent);
                            aTimer.Start();
                            label53.Text = dishList[i].name;
                        }
                    }
                }
                if (canDeleted)
                {
                    listView2.Items.RemoveAt(collection[0]);
                    productionList.RemoveAt(select);
                    Functions.DeleteProduct(nameProd, Convert.ToInt32(sklad));

                    label42.Visible = true;
                    label42.BringToFront();
                    aTimer.Interval = 1000;
                    aTimer.Tick += new EventHandler(OnTimeEvent);
                    aTimer.Start();
                }
            }
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collection = listView3.SelectedIndices;
            if (collection.Count != 0)
            {
                panel5.Visible = true;
                panel5.BringToFront();
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collection = listView3.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView3.SelectedIndices[0];
                label48.Text = "";

                for (int i = 0; i < dishList.Count; i++)
                {
                    if (listView3.Items[select].SubItems[0].Text == dishList[i].name)
                    {
                        for (int j = 0; j < dishList[i].productList.Count; j++)
                        {
                            label48.Text += dishList[i].productList[j].name + ", ";
                        }
                    }
                }

            }
        }
        private void Button11_Click(object sender, EventArgs e)
        {
            int select = listView3.SelectedIndices[0];
            Functions.DeleteDishWithComponent(listView3.Items[select].SubItems[0].Text);
            panel5.SendToBack(); listView3.Items.RemoveAt(select);
            dishList.RemoveAt(select);

            label51.Visible = true;
            label51.BringToFront();
            aTimer.Interval = 1000;
            aTimer.Tick += new EventHandler(OnTimeEvent);
            aTimer.Start();
        }
        private void Button12_Click(object sender, EventArgs e)
        {
            panel5.SendToBack();
        }
        private void Button13_Click(object sender, EventArgs e) // Кнопка плюс количество продукта
        {
            ListView.SelectedIndexCollection collection = listView2.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView2.SelectedIndices[0];
                bool canEdit = CheckCountProd(true, select);

                if (canEdit)
                {
                    string name = listView2.Items[select].SubItems[0].Text;
                    string sklad = listView2.Items[select].SubItems[1].Text;
                    string count = listView2.Items[select].SubItems[2].Text;

                    int countProd = Convert.ToInt32(count);
                    countProd += Convert.ToInt32(textBox12.Text);
                    listView2.Items[select].SubItems[2].Text = countProd.ToString();

                    productionList[select].count = countProd;
                    List<string> product = new List<string>();
                    product.Add(name);
                    product.Add(sklad);
                    product.Add(countProd.ToString());
                    Functions.UpdateProducts(product);
                }
            }
        }
        private void Button14_Click(object sender, EventArgs e) // Кнопка минус количество продукта
        {

            ListView.SelectedIndexCollection collection = listView2.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView2.SelectedIndices[0];
                bool canEdit = CheckCountProd(false, select);

                if (canEdit)
                {
                    string name = listView2.Items[select].SubItems[0].Text;
                    string sklad = listView2.Items[select].SubItems[1].Text;
                    string count = listView2.Items[select].SubItems[2].Text;

                    int countProd = Convert.ToInt32(count);
                    countProd -= Convert.ToInt32(textBox12.Text);
                    listView2.Items[select].SubItems[2].Text = countProd.ToString();

                    productionList[select].count = countProd;
                    List<string> product = new List<string>();
                    product.Add(name);
                    product.Add(sklad);
                    product.Add(countProd.ToString());
                    Functions.UpdateProducts(product);
                }
                if (!canEdit)
                {
                    label55.Visible = true;
                    label55.BringToFront();
                    aTimer.Interval = 1000;
                    aTimer.Tick += new EventHandler(OnTimeEvent);
                    aTimer.Start();
                }
            }
        }
        /*BUTTONS---END*/


        /*TEXTBOXES AND COMBOBOXES CHANGED*/
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            label7.Visible = false;
            textBox1.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox2_TextChanged_1(object sender, EventArgs e)
        {
            label21.Visible = false;
            textBox2.BackColor = Color.FromArgb(228, 209, 211);
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
        private void TextBox8_TextChanged_1(object sender, EventArgs e)
        {
            label56.Visible = false;
            textBox8.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox9_TextChanged(object sender, EventArgs e)
        {
            textBox9.BackColor = Color.FromArgb(228, 209, 211);
            comboBox5.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox10_TextChanged(object sender, EventArgs e)
        {
            label33.Visible = false;
            textBox10.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox11_TextChanged(object sender, EventArgs e)
        {
            label34.Visible = false;
            textBox11.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox10_TextChanged_1(object sender, EventArgs e)
        {
            label33.Visible = false;
            textBox10.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void TextBox11_TextChanged_1(object sender, EventArgs e)
        {
            label34.Visible = false;
            textBox11.BackColor = Color.FromArgb(228, 209, 211);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label8.Visible = false;
            comboBox1.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label24.Visible = false;
            comboBox2.BackColor = Color.FromArgb(228, 209, 211);
        }
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            label25.Visible = false;
            comboBox4.BackColor = Color.FromArgb(228, 209, 211);
            listView1.BackColor = Color.FromArgb(228, 209, 211);
        }
        /*TEXTBOXES AND COMBOBOXES CHANGED---END*/


        /*CLEAR FUNCTIONS*/
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
            productList.Clear();
        }
        private void CheckNewSotr()
        {
            string stazh = textBox3.Text.Trim();
            bool isNum = false;
            if (stazh.Length != 0)
                isNum = int.TryParse(stazh, out _);

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

                            }
                            else
                            {
                                label11.Visible = true;
                                textBox5.BackColor = Color.FromArgb(255, 192, 192);
                            }
                        }
                        else
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
        /*CLEAR FUNCTIONS---END*/

        /*HIDE PANELS*/ 
        Timer aTimer = new Timer();
        int j = 0;
        public void HidePanel()
        {
            if (j == 1)
            {
                panel2.Visible = false;
                label39.Visible = false;
                label40.Visible = false;
                label41.Visible = false;
                label42.Visible = false;
                panel6.Visible = false;
                label51.Visible = false;
                panel7.Visible = false;
                label55.Visible = false;
                aTimer.Stop();
                j = 0;
            }
            else
                j++;
        }
        private void OnTimeEvent(Object source, EventArgs e)
        {
            HidePanel();
        }
        /*HIDE PANELS---END*/


        /*ADD FUNCTIONS*/
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
        private void AddProductToList()
        {
            if (comboBox4.SelectedItem != null & comboBox3.SelectedItem != null)
            {
                Product item = new Product();
                item.name = comboBox4.SelectedItem.ToString();
                item.count = Convert.ToDouble(comboBox3.SelectedItem.ToString());
                bool isIn = false;
                for (int i = 0; i < productList.Count; i++)
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
        private bool CheckCountProd(bool znak, int select)
        {
            bool yes = false;

            if (textBox12.TextLength != 0)
                yes = int.TryParse(textBox12.Text, out _);

            if (!znak & yes)
            {
                int res = Convert.ToInt32(textBox12.Text);
                if (Convert.ToInt32(listView2.Items[select].SubItems[2].Text) > res)
                    return true;
            }
            if (znak & yes) return true;
            return false;

        }
        /*ADD FUNCTIONS---END*/

        /*EMPTY FUNCTIONS*/
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void TextBox8_TextChanged(object sender, EventArgs e)
        {

        }
        private void Label32_Click(object sender, EventArgs e)
        {

        }
        private void PictureBox9_Click(object sender, EventArgs e)
        {

        }
        private void Panel5_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ListView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /*EMPTY FUNCTIONS---END*/
    }
}

