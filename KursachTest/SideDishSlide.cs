using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class SideDishSlide : UserControl
    {
        public SideDishSlide()
        {
            var SideDish = new List<string>();
            string Category = "Гарниры";
            Functions.side = this;

            InitializeComponent();
            Functions.GetInformationFromBase(SideDish, Category);
            Functions.spawnButtons(SideDish, "side");
        }
    }
}
