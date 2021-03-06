﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace User
{
    public partial class Login : Form
    {
        private readonly Action<string, string> connectFunction;
        private IList<Language> supportedLanguages;

        public Login(Action<string, string> connectFunction)
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

            foreach (string lang in tmp)
            {
                string[] final = lang.Split(':');
                supportedLanguages.Add(new Language(final[0], final[1]));
            }
        }

        private class Language
        {
            //What is showed in the comboBox - Eg: Portugues, English
            public string FriendlyName { get; set; }
            //What is used to connect to the translate service - Eg: pt, en
            public string LanguageCode { get; set; }

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
