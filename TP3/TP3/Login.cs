using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace TP3
{
    public partial class Login : Form
    {
        private readonly Func<string, string, bool> connectFunction;
        private IList<Language> supportedLanguages;

        public Login(Func<string, string, bool> connectFunction)
        {
            InitializeComponent();
            this.connectFunction = connectFunction;
            this.supportedLanguages = new List<Language>();
            loginButton.Enabled = false;

            getSupportedLanguages();

            comboBox1.DataSource = supportedLanguages;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            String username = usernameBox.Text;
            String language = ((Language)comboBox1.SelectedValue).LanguageCode;

            connectFunction(username, language);
            this.Hide();
        }

        /// <summary>
        /// Reads the config file for the supported languages.
        /// The supported languages are stored in the Supported_Languages key, in the following format:
        /// Lg1:Lg1Code;Lg2:Lg2Code.....
        /// </summary>
        private void getSupportedLanguages()
        {
            string languages = ConfigurationManager.AppSettings["Supported_Languages"];
            string[] tmp = languages.Split(';');

            foreach(string lang in tmp)
            {
                string[] final = lang.Split(':');
                supportedLanguages.Add(new Language(final[0], final[1]));
            }
        }

        private class Language
        {
            //What is showed in the comboBox - Eg: Portugues, English
            public string FriendlyName { get; }
            //What is used to connect to the translate service - Eg: pt, en
            public string LanguageCode { get; }

            public Language(string name, string code)
            {
                this.FriendlyName = name;
                this.LanguageCode = code;
            }

            public override string ToString()
            {
                return FriendlyName;
            }
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
            loginButton.Enabled = usernameBox.Text.Length != 0;
        }
    }
}
