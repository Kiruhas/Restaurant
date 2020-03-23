using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class OrderSlide : UserControl
    {
        
        public OrderSlide()
        {
            InitializeComponent();
            Functions.order = this;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SendToBack();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SendToBack();
            Functions.f2.DeleteOrderFromList(label2.Text);  
        }
    }
}
