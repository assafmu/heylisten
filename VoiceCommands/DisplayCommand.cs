using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyListen.VoiceCommands
{
    public class DisplayCommand :IVoiceCommand
    {
        private string _word;
        public DisplayCommand(string word)
        {
            this._word = word;

        }
        public string GetWord()
        {
            return _word;
        }

        public void PerformAsyncAction()
        {
            MessageBox.Show(_word);
        }
    }
}
