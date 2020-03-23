using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class SeasonSlide : UserControl
    {

        public SeasonSlide()
        {
            var seasonDish = new List<string>();
            string Category = "Сезонное";
            Functions.season = this;

            InitializeComponent();
            Functions.GetInformationFromBase(seasonDish, Category);
            Functions.spawnButtons(seasonDish, "season");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void SeasonSlide_Load(object sender, EventArgs e)
        {
        }
    }
}
