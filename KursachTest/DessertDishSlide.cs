using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class DessertDishSlide : UserControl
    {
        public DessertDishSlide()
        {
            var dessertDish = new List<string>();
            string Category = "Десерты";
            Functions.dessert = this;

            InitializeComponent();
            Functions.GetInformationFromBase(dessertDish, Category);
            Functions.spawnButtons(dessertDish, "dessert");
        }

    }
}
