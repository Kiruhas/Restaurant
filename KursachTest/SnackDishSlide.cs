using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class SnackDishSlide : UserControl
    {
        public SnackDishSlide()
        {
            var snackDish = new List<string>();
            string Category = "Закуски";
            Functions.snack = this;

            InitializeComponent();
            Functions.GetInformationFromBase(snackDish, Category);
            Functions.spawnButtons(snackDish, "snack");
        }
    }
}
