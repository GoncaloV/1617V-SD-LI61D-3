using System;
using System.Windows.Forms;
using Interface;
using ClientClass;

namespace TP1
{
    public partial class Form1 : Form
    {
        private readonly IClientInterface clientController;

        public Form1()
        {
            InitializeComponent();
            clientController = new ClientClassImpl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientController.associateWithServer();
        }
    }
}
