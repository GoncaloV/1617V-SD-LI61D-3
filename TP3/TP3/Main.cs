using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using TP3.com.microsofttranslator.api;
namespace TP3
{
    public partial class Main : Form
    {
        //User settings
        private string username;
        private string user_language;



        //Microsoft Service
        private readonly string API_KEY = ConfigurationManager.AppSettings["Microsoft_Key"];
        private readonly SoapService MicrosoftTranslator = new SoapService();

        public Main()
        {
            InitializeComponent();

            //Disable by default because we arent connected
            changeControllersState(false);
        }


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
        /// <returns></returns>
        private void handleConnect(String username, String nativeLanguage)
        {
            Console.WriteLine("-------> User: " + username);
            Console.WriteLine("-------> Language: " + nativeLanguage);

            this.username = username;
            this.user_language = nativeLanguage;

            //Handle login here!


            //Finally enable all actions
            changeControllersState(true);
        }


        private void changeControllersState(bool setupDone)
        {
            messageBox.Enabled = setupDone;
            logBox.Enabled = setupDone;
            sendMessage.Enabled = setupDone;
            ConnectButton.Enabled = !setupDone;
        }
    }
}
