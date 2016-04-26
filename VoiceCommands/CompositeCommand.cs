using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyListen.VoiceCommands
{
    public class CompositeCommand :IVoiceCommand
    {
        private readonly string _word;
        private Action[] _actions;
        public CompositeCommand(string word,params Action[] actions)
        {
            _word = word;
            _actions = actions;
        }
        public string GetWord()
        {
            return _word;
        }

        public void PerformAsyncAction()
        {
            foreach (var action in _actions)
            {
                action();
            }
        }
    }
}
