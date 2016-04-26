using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyListen.VoiceCommands
{
    public interface IVoiceCommand
    {
        string GetWord();

        void PerformAsyncAction();
    }
}
