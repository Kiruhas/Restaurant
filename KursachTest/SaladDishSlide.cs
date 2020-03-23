using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class SaladDishSlide : UserControl
    {
        public SaladDishSlide()
        {
            var saladDish = new List<string>();
            string Category = "Салаты";
            Functions.salad = this;

            InitializeComponent();
            Functions.GetInformationFromBase(saladDish, Category);
            Functions.spawnButtons(saladDish, "salad");
        }
    }
}
