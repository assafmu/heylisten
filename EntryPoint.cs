using HeyListen.Providers;
using HeyListen.VoiceCommands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyListen
{
    static class EntryPoint
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var onWord = ConfigurationManager.AppSettings["ToggleOnWord"];
            var offWord = ConfigurationManager.AppSettings["ToggleOffWord"];
            float parsedConfidence;
            float confidence = float.TryParse(ConfigurationManager.AppSettings["Confidence"], out parsedConfidence) ? parsedConfidence : 0.8f;
            SpeechListener speechListener = new SpeechListener(onWord, offWord, confidence);
            speechListener.AddCommands(new ConfirmableExitCommand());
            var cmdCommands = new CmdVoiceCommandsProvider(Environment.CurrentDirectory + @"\Resources\commands.xml").GetVoiceCommands();
            speechListener.AddCommands(cmdCommands.ToArray());
            speechListener.StartAsyncListening();
        }
    }
}
