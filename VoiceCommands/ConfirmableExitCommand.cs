using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyListen.VoiceCommands
{
    public class ConfirmableExitCommand:IVoiceCommand
    {
        private readonly string _word;

        public ConfirmableExitCommand()
        {
            _word = ConfigurationManager.AppSettings["ExitWord"];
        }
        public string GetWord()
        {
            return _word;
        }

        public void PerformAsyncAction()
        {
            DialogResult result = MessageBox.Show("Exit HeyListen?", "Exit", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Environment.Exit(0);
            }
        }
    }
}
