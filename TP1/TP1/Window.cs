using ClientClass;
using Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP1
{
    public partial class Window : Form
    {
        private readonly IClientInterface clientController;
        private bool isConnectedToServer = false;

        private List<String> studentOptions;

        public Window()
        {
            InitializeComponent();
            clientController = new ClientClassImpl();
            initComboBox();
            changeServerControllersState();    
        }

        private void initComboBox()
        {
            studentOptions = ConfigurationManager.AppSettings.AllKeys.ToList();
            studentBox.Items.AddRange(studentOptions.ToArray());

        }

        private void changeServerControllersState()
        {
            pushButton.Enabled = isConnectedToServer;
            pullButton.Enabled = isConnectedToServer;
            deleteButton.Enabled = isConnectedToServer;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

            progressBar.Visible = true;

            Task.Factory.StartNew(() =>
            {
                String result = clientController.associateWithServer();

                descriptionBox.Text += result;

                if (result.Contains("Associated"))
                {
                    isConnectedToServer = true;
                    changeServerControllersState();
                    progressBar.Visible = false;
                    connectButton.Enabled = false;
                }
            });
        }

        private void pushButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;

            if (key.Length == 0)
            {
                descriptionBox.Text += "Input a key first\n";
                return;
            }

            int student = studentBox.SelectedIndex;

            Task.Factory.StartNew(() =>
            {
                descriptionBox.Text += (student != -1) ? clientController.storePairOnServer(key, student) : "Select a student first\n";
            });
        }

        private void pullButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;

            if (key.Length == 0)
            {
                descriptionBox.Text += "Input a key first\n";
                return;
            }

            Task.Factory.StartNew(() =>
            {
                descriptionBox.Text += clientController.readPairFromServer(key);
            });
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            String key = keyText.Text;

            if (key.Length == 0)
            {
                descriptionBox.Text += "Input a key first\n";
                return;
            }

            Task.Factory.StartNew(() =>
            {
                descriptionBox.Text += clientController.deletePairFromServer(key);
            });

        }
    }
}
