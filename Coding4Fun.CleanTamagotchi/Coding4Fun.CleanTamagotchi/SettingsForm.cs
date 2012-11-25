using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coding4Fun.CleanTamagotchi
{
    public partial class SettingsForm : Form
    {
        private SettingsManager _manager;

        public SettingsForm()
        {
            _manager = new SettingsManager();
            InitializeComponent();

            var settings = _manager.GetSettings();
            textBox1.Text = settings.ServerUrl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.ServerUrl = textBox1.Text;
            _manager.SetSettings(settings);
        }
    }
}
