using ClientClass;
using Interface;
using System;
using System.Windows.Forms;

namespace TP1
{
    public partial class Window : Form
    {
        private readonly IClientInterface clientController;
        private bool isConnectedToServer = false;

        public Window()
        {
            InitializeComponent();
            clientController = new ClientClassImpl();
            changeServerControllersState();
        }

        private void changeServerControllersState()
        {
            pushButton.Enabled = isConnectedToServer;
            pullButton.Enabled = isConnectedToServer;
            deleteButton.Enabled = isConnectedToServer;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            String result = clientController.associateWithServer();

            descriptionBox.Text += result;

            if (result.Contains("Associated")) { 
                isConnectedToServer = true;
                changeServerControllersState();
            }

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
