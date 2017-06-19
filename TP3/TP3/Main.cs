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
using TP3.com.microsofttranslator.api;
namespace TP3
{
    public partial class Main : Form
    {
        private readonly string API_KEY = ConfigurationManager.AppSettings["Microsoft_Key"];
        private readonly SoapService MicrosoftTranslator = new SoapService();

        public Main()
        {
            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            label1.Text = await Translate("Adeus", "pt", "en");
        }

        private async Task<String> Translate(string message, string fromLanguage, string toLanguage)
        {
            return await Task.Run(
                () => MicrosoftTranslator.Translate(API_KEY, message, fromLanguage, toLanguage, "text/html", "general", "")
            );
        }

        
    }
}
