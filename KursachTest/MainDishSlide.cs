using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class MainDishSlide : UserControl
    {
        

        public MainDishSlide()
        {
            var MainDish = new List<string>();   
            string Category = "Основные блюда";
            Functions.main = this;

            InitializeComponent();
            Functions.GetInformationFromBase(MainDish, Category);
            Functions.spawnButtons(MainDish, "main");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void MainDishSlide_Load(object sender, EventArgs e)
        {

        }
    }
}
