﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyListen.VoiceCommands
{
    public class CMDVoiceCommand : IVoiceCommand
    {
        string _word;
        string _command;
        private readonly string _param;
        private readonly bool _verbose;

        public CMDVoiceCommand(bool verbose,string word, string command,string param=null)
        {
            _command = command;
            _word = word;
            _param = param;
            _verbose = verbose;
        }
        public string GetWord()
        {
            return _word;
        }

        public void PerformAsyncAction()
        {
            CallCMDInBackground(_verbose,_command,_param);
            
        }

        public static void CallCMDInBackground(bool verbose,string cmdArgs,string param=null)
        {
            string formattedArgs = string.Format(@"/c ""{0}""", cmdArgs);
            if (param != null)
            {
                formattedArgs += " " + param;
            }
            if (verbose)
            {
                MessageBox.Show(formattedArgs);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "CMD.exe";
            startInfo.Arguments = formattedArgs;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            using (Process processTemp = new Process())
            {
                processTemp.StartInfo = startInfo;
                processTemp.EnableRaisingEvents = true;
                try
                {
                    processTemp.Start();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
