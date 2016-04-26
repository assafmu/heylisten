using HeyListen.VoiceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace HeyListen.Providers
{
    public class CmdVoiceCommandsProvider : ICommandProvider
    {
        private readonly string _browserPath;
        private string _filename;
        public CmdVoiceCommandsProvider(string filename)
        {
            this._filename = filename;
            this._browserPath = System.Configuration.ConfigurationManager.AppSettings["BrowserPath"];
        }

        public IEnumerable<IVoiceCommand> GetVoiceCommands()
        {
            XDocument doc = XDocument.Load(_filename);
            IEnumerable<XElement> elements = from el in doc.Element("commands").Elements()
                                             select el;
            var enumerable = elements.Select(BuildCommand).ToArray();
            return enumerable;

        }

        internal IVoiceCommand BuildCommand(XElement element)
        {
            if (element.Name.LocalName.ToLower().Equals("command") && element.Attribute("program") != null)
            {
                return new CMDVoiceCommand(element.Attribute("word").Value, element.Attribute("program").Value);
            }
            if (element.Name.LocalName.ToLower().Equals("site") && element.Attribute("url") != null)
            {
                return new CMDVoiceCommand(element.Attribute("word").Value, _browserPath, element.Attribute("url").Value);
            }

            var programActionStrings = element.Elements("program").Select(x => x.Attribute("value").Value);
            var programActions = programActionStrings.Select<string, Action>(s =>
                 (
                     () => { CMDVoiceCommand.CallCMDInBackground(s); }
                 )
            );

            var internetActionStrings = element.Elements("site").Select(x => x.Attribute("value").Value);
            var internetActions = internetActionStrings.Select<string, Action>(s =>
                (
                    () => { CMDVoiceCommand.CallCMDInBackground(_browserPath, s); }
                )
            );
            return new CompositeCommand(element.Attribute("word").Value, programActions.Concat(internetActions).ToArray());
        }
    }
}
