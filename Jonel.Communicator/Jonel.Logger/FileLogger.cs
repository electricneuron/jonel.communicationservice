using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jonel.Logger;

namespace Jonel.Logger
{
    public class FileLogger : ILogger
    {
        static string _filePath = Path.GetTempPath();
        static string _logFile = "jonel_communicator.log";

        string LogFile
        {
            get { return Path.Combine(_filePath, _logFile); }
        }

        public FileLogger()
        {
            if (!File.Exists(LogFile))
                File.Create(LogFile);
        }

        private void WriteLogEntry(string appHandle, string message, string stackTrace, string entryType = "Error")
        {
            try
            {
                using (FileStream fs = new FileStream(LogFile, FileMode.Append, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(fs))
                    {
                        string log = string.Format("{0}:[{1}] - {2} {3}", appHandle, entryType, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message);
                        streamWriter.WriteLine(log);
                        if (!string.IsNullOrWhiteSpace(stackTrace))
                            streamWriter.WriteLine(stackTrace);
                    }
                }
            }
            catch
            {
            }
        }

        #region ILogger Members

        public void Error(string message, Exception ex)
        {
            WriteLogEntry("", message, ex.StackTrace);
        }

        public void Info(string mesage)
        {
            WriteLogEntry("", mesage, null, "Info");
        }

        public void Info(string appHandle, string message)
        {
            WriteLogEntry(appHandle, message, null, "Info");
        }

        public void Log(string appHandle, string message)
        {
            WriteLogEntry(appHandle, message, null);
        }

        public void Log(string appHandle, Exception ex)
        {
            WriteLogEntry(appHandle, ex.Message, ex.StackTrace);
        }

        #endregion

        public string GetLogFilePath()
        {
            return Path.GetFullPath(LogFile);
        }
    }
}
