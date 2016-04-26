using HeyListen.VoiceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyListen.Providers
{
    public interface ICommandProvider
    {
        IEnumerable<IVoiceCommand> GetVoiceCommands();
    }
}
