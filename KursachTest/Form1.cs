using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        } // Пусто

        private void button1_Click(object sender, EventArgs e) // Выход
        {
            Close();
        }

        private void CheckPass() // Проверка что вводится что-то в текстбоксы
        {
            if (label2.Visible == true) {
                label2.Visible = false;
                textBox1.BackColor = Color.FromArgb(228, 209, 211);
                textBox2.BackColor = Color.FromArgb(228, 209, 211);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Добавить сотрудника
        {
            string login = textBox1.Text.ToLower();
            string pass = Functions.GetHash(textBox2.Text);
            switch (Functions.CheckStaff(login, pass))
            {
                case "Модератор":
                    Form form3 = new Form3();
                    form3.Show();
                    Hide();
                    break;
                case "Официант":
                    string fio = Functions.ReturnFio(login);
                    Form form2 = new Form2(fio);
                    form2.Show();
                    Hide();
                    break;
                default:
                    label2.Visible = true;
                    textBox1.BackColor = Color.FromArgb(255, 192, 192);
                    textBox2.BackColor = Color.FromArgb(255, 192, 192);
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckPass();
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            CheckPass();
        }
    }
}
