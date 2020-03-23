using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class SearchDishSlide : UserControl
    {
        List<Button> buttons = new List<Button>();

        public SearchDishSlide()
        {
            Functions.search = this;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) // кнопка Поиск
        {
            var searchDish = new List<string>(); // список блюд поиска
            string searchString = textBox1.Text; // строка поиска

            Functions.RemoveButtons(buttons); // удаление прошлых кнопок
            Functions.GetInformationFromBaseForSearch(searchDish, searchString); // Поиск в базе
            Functions.spawnButtons(searchDish, 50, buttons); // появление новых кнопок
        }
    }
}
