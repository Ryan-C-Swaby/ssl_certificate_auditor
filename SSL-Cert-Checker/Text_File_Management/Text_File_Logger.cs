using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Text_File_Management
{
    public class Text_File_Logger
    {
        private string _logPath;
        
        public Text_File_Logger(string logPath)
        {
            _logPath = logPath;
        }

        public void WriteToTextFile(string messageToLog)
        {
            using (var writer = File.CreateText(_logPath))
            {
                writer.WriteLine(messageToLog); 
            }
        }

    }
}
