using ClientClass;
using Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP1
{
    public partial class Window : Form
    {
        private readonly IClientInterface clientController;

        public Window()
        {
            InitializeComponent();
            clientController = new ClientClassImpl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientController.associateWithServer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientController.storePairOnServer("OLA", "OLA");
        }
    }
}
