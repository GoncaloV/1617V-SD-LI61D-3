using ClientClass;
using Interface;
using System;
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

        private void connectButton_Click(object sender, EventArgs e)
        {
            descriptionBox.Text += clientController.associateWithServer();
        }

        private void pushButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;
            String value = valueText.Text;

            descriptionBox.Text += clientController.storePairOnServer(key, value);
        }

        private void pullButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;

            descriptionBox.Text += clientController.readPairFromServer(key);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;

            descriptionBox.Text += clientController.deletePairFromServer(key);

        }
    }
}
