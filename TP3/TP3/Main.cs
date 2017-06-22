using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using TP3.com.microsofttranslator.api;
using IReceiver = Interfaces.IReceiver;
using Interfaces;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using TP3.RegisterService;
using System.Collections.Generic;
using System.Linq;

namespace TP3
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Main : Form, IReceiver
    {
        //Represents the current user
        private ChatUser user = new ChatUser();

        //Microsoft Service
        private readonly SoapService MicrosoftTranslator = new SoapService();
        private readonly string API_KEY = ConfigurationManager.AppSettings["Microsoft_Key"];


        //Ours
        private string BINDING_NAME;
        private readonly RegisterClient server;
        private readonly Callbacks userCallbacks;
        private ServiceHost host;

        public Main(string[] userConfig, bool selfInit)
        {
            InitializeComponent();

            userCallbacks = new Callbacks();

            server = new RegisterClient(new InstanceContext(userCallbacks));

            getBinding();

            //@TODO: Define the user URI in the app.config file.
            user.URI = userConfig[userConfig.Length - 1];

            //Disable by default because we arent connected
            changeControllersState(false);

            if (selfInit)
                handleConnect(userConfig[0], userConfig[1]);
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

            //InitService;
            host = new ServiceHost(this);
            host.Open();

            try
            {
                List<ChatUser> onlineUsers = server.Subscribe(user).ToList();
                onlineUsers.ForEach(user => userCallbacks.NotifySubscribe(user));

                //Finally enable all actions
                changeControllersState(true);

                logBox.Text += "Connected as: " + username + ", language: " + nativeLanguage + "\n";

            }
            catch (Exception)
            {
                MessageBox.Show(@"Error!", @"ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



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


        private void getBinding()
        {
            ClientSection client = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
            foreach (ChannelEndpointElement endpoint in client.Endpoints)
            {
                if (endpoint.Contract.Contains("IRegister"))
                {
                    BINDING_NAME = endpoint.BindingConfiguration;
                    break;
                }
            }
        }

        //Method is called when we receive a message
        public void handleNewMessage(ChatMessage message)
        {
            String newMessage = message.message;

            try
            {
                Task.Factory.StartNew(async () =>
                {
                    if (!message.language.Equals(user.language))
                        newMessage = await Translate(message.message, message.language, user.language);

                    logBox.Text += "Message from " + message.username + ": " + newMessage;
                });
            }
            catch (Exception)
            {
                MessageBox.Show(@"Service not responding....", @"Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() => {
                    foreach (ChatUser u in userCallbacks.userList)
                    {
                        //ChatServiceClient cs = new ChatServiceClient(chatInfo.Binding, new EndpointAddress(chatInfo.Uri));

                        ChatMessage msg = new ChatMessage();
                        msg.language = user.language;
                        msg.username = user.name;
                        msg.message = messageBox.Text;

                        try
                        {
                            //cs.ReceiveMessage(msg);
                        }
                        catch (EndpointNotFoundException)
                        {
                            userCallbacks.NotifyUnsubscribe(u);
                        }
                    }
                });
            }
            catch (Exception)
            {
                MessageBox.Show(@"Error Sending message", @"ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
