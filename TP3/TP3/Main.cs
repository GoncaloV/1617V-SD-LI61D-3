using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using TP3.com.microsofttranslator.api;
using IReceiver = Interfaces.IReceiver;
using Interfaces;

namespace TP3
{
    public partial class Main : Form, IReceiver
    {
        //Represents the current user
        private ChatUser user = new ChatUser();

        //Microsoft Service
        private readonly string API_KEY = ConfigurationManager.AppSettings["Microsoft_Key"];
        private readonly SoapService MicrosoftTranslator = new SoapService();

        public Main()
        {
            InitializeComponent();

            //@TODO: Define the user URI in the app.config file.
            user.URI = ConfigurationManager.AppSettings["Microsoft_Key"];
            
            //Disable by default because we arent connected
            changeControllersState(false);
        }

        
        /// <summary>
        /// Uses the Microsoft service to translate a message.
        /// This method is async, which means that when you are going to call it you must call it like: await Translate(...);
        /// Why not use the async method that the service provides? 
        /// In my opinion the setup required, to do that is not worth it, and this gives a must better controller.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns>The translated message</returns>
        private async Task<String> Translate(string message, string fromLanguage, string toLanguage)
        {
            return await Task.Run(
                () => MicrosoftTranslator.Translate(API_KEY, message, fromLanguage, toLanguage, "text/html", "general", "")
            );
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
