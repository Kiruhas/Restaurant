using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class DrinksDishSlide : UserControl
    {
        public DrinksDishSlide()
        {
            var drinksDish = new List<string>();
            string Category = "Напитки";
            Functions.drinks = this;

            InitializeComponent();
            Functions.GetInformationFromBase(drinksDish, Category);
            Functions.spawnButtons(drinksDish, "drinks");
        }
    }
}
