using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using TP3.com.microsofttranslator.api;
using IReceiver = Interfaces.IReceiver;
using Interfaces;
using System.Security.Policy;

namespace TP3
{
    public partial class Main : Form, IReceiver
    {
        //Represents the current user
        private ChatUser user = new ChatUser();

        public Main(string[] userConfig, bool selfInit)
        {
            InitializeComponent();

            //@TODO: Define the user URI in the app.config file.
            user.URI = userConfig[userConfig.Length - 1];

            //Disable by default because we arent connected
            changeControllersState(false);

            if (selfInit)
                handleConnect(userConfig[0], userConfig[1]);
        }


        /// <summary>
        /// Connect Button click Event
        /// When clicked will launch a new form to handle the login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            new Login(handleConnect).Show();
        }

        /// <summary>
        /// Handles the connection between the server and I.
        /// Returns true/false indicating wether or not the connection was successful
        /// </summary>
        /// <param name="username"></param>
        /// <param name="nativeLanguage"></param>
        private void handleConnect(String username, String nativeLanguage)
        {
            user.name = username;
            user.language = nativeLanguage;

            //Handle login here!


            //Finally enable all actions
            changeControllersState(true);

            logBox.Text += "Connected as: " + username + ", language: " + nativeLanguage + "\n";
        }

        /// <summary>
        /// Used to disable UI Controls
        /// </summary>
        /// <param name="setupDone">If the user has been connected to the server</param>
        private void changeControllersState(bool setupDone)
        {
            messageBox.Enabled = setupDone;
            logBox.Enabled = setupDone;
            sendMessage.Enabled = setupDone;
            ConnectButton.Enabled = !setupDone;
        }


        //Method is called when we receive a message
        public void handleNewMessage(ChatMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
