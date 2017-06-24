using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;
using User.ChatService;
using User.TranslatorService;
using IChatService = Interfaces.IReceiver;
using User.RegisterService;

namespace User {

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Main : Form, IChatService {

        private static readonly string BINDING_NAME;
        private ChatUser user = new ChatUser();

        //Microsoft Service
        private readonly string API_KEY = ConfigurationManager.AppSettings["Microsoft_Key"];
        private readonly LanguageServiceClient MicrosoftTranslator = new LanguageServiceClient();

        private readonly RegisterClient server;
        private readonly Callbacks callbacks;

        private ServiceHost host;

        //Represents the current user
        private ChatUser currentUser;

        static Main()
        {
            ClientSection client = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
            foreach (ChannelEndpointElement endpoint in client.Endpoints)
            {
                if (endpoint.Contract.Contains("IReceiver"))
                {
                    BINDING_NAME = endpoint.BindingConfiguration;
                    break;
                }
            }
        }



        public Main(string[] userConfig, bool selfInit)
        {
            InitializeComponent();

            callbacks = new Callbacks();

            server = new RegisterClient(new InstanceContext(callbacks));

            //@TODO: Define the user URI in the app.config file.
            user.URI = userConfig[userConfig.Length - 1];

            //Disable by default because we arent connected
            changeControllersState(false);

            if (selfInit)
                handleConnect(userConfig[0], userConfig[1]);

        }

        protected override void OnFormClosing(FormClosingEventArgs e) {

            DialogResult res = DialogResult.No;
            Task t = Task.Factory.StartNew(() => {
                try {
                    server.Unsubscribe(currentUser);
                    res = MessageBox.Show(@"Chat Disconnect", @"Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }catch (Exception) {
                    MessageBox.Show(@"Server not responding try later", @"Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
            t.Wait();

            if(host != null)
                host.Close();

            if (res == DialogResult.OK)
                base.OnFormClosing(e);
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


        public void ReceiveMessage(ChatMessage msg) {
            string message = msg.message;
            try {
                Task.Factory.StartNew(async () =>
                {
                    if (!currentUser.language.Equals(msg.language))
                        message = await Translate(message, msg.language, currentUser.language);

                    messageBox.Text += "New message from " + msg.username + ": " + message;
                });
            }catch (Exception) {
                MessageBox.Show(@"Receiving message", @"ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            //InitService;
            initService();

            currentUser = getChatUser(username, nativeLanguage);
            
            try
            {
                List<ChatUser> onlineUsers = server.Subscribe(currentUser).ToList();
                onlineUsers.ForEach(user => callbacks.NotifySubscribe(user));

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

        private ChatUser getChatUser(string username, string nativeLanguage) {
            return new ChatUser {
                name = username,
                language = nativeLanguage,
                URI = host.BaseAddresses[0].AbsoluteUri,
                Binding = BINDING_NAME
            };
        }

        private void initService() {
            host = new ServiceHost(this);
            host.Open();
        }


        private void sendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() => {
                    foreach (ChatUser u in callbacks.userList)
                    {
                        ReceiverClient rs = new ReceiverClient(currentUser.Binding, new EndpointAddress(currentUser.URI));

                        ChatMessage msg = new ChatMessage();
                        msg.language = user.language;
                        msg.username = user.name;
                        msg.message = messageBox.Text;

                        try
                        {
                            rs.ReceiveMessage(msg);
                        }
                        catch (EndpointNotFoundException)
                        {
                            callbacks.NotifyUnsubscribe(u);
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
