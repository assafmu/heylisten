using HeyListen.VoiceCommands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyListen
{
    class SpeechListener
    {
        private readonly float _confidence;
        IDictionary<string, Action> wordsToActions = new Dictionary<string, Action>();
        private readonly string _toggleOnWord;
        private readonly string _toggleOffWord;

        public bool Awake { get; set; }

        private readonly bool _toggleActive;

        public SpeechListener(string toggleOnWord, string toggleOffWord, float confidence)
        {
            _confidence = confidence;
            if (string.IsNullOrWhiteSpace(toggleOnWord) || string.IsNullOrWhiteSpace(toggleOffWord))
            {
                _toggleActive = false;
                return;
            }
            _toggleOnWord = toggleOnWord;
            _toggleOffWord = toggleOffWord;
            Awake = true;
            _toggleActive = true;
        }

        public void StartAsyncListening()
        {
            using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US")))
            {
                IEnumerable<string> words = wordsToActions.Keys;
                if (_toggleActive)
                {
                    words = words.Concat(new[] { _toggleOffWord, _toggleOnWord });
                }

                Choices wordChoices = new Choices(words.ToArray());
                GrammarBuilder gb = new GrammarBuilder(wordChoices);
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the process running.
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }

        public void AddCommands(params IVoiceCommand[] commands)
        {
            foreach (IVoiceCommand command in commands)
            {
                wordsToActions.Add(command.GetWord(), command.PerformAsyncAction);
            }
        }

        // Create a simple handler for the SpeechRecognized event.
        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < _confidence)
            {
                return;
            }
            if (_toggleActive && e.Result.Text.Equals(_toggleOffWord))
            {
                Awake = false;
            }
            if (_toggleActive && e.Result.Text.Equals(_toggleOnWord))
            {
                Awake = true;
            }
            if (_toggleActive && !Awake)
            {
                return;
            }
            if (wordsToActions.ContainsKey(e.Result.Text))
            {
                wordsToActions[e.Result.Text]();
            }
        }
    }
}
