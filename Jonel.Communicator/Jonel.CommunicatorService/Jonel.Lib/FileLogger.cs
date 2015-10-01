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

        static string LogFile
        {
            get { return Path.Combine(_filePath, _logFile); }
        }

        static FileLogger()
        {
            if (!File.Exists(LogFile))
                File.Create(LogFile);
        }

        private void WriteLogEntry(string message, string stackTrace)
        {
            try
            {
                using (FileStream fs = new FileStream(LogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fs))
                    {
                        string log = string.Format("{0} {1} {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message, stackTrace);
                        streamWriter.WriteLine(message);
                    }
                }
            }
            catch
            {
            }
        }

        public static void Info(string message)
        {
        }

        public static void Error(string message)
        {
        }

        public static void Error(string message, Exception ex)
        {
        }

        public void InfoFormat(string message)
        {
        }

        #region ILogger Members

        public void Log(string error)
        {
            WriteLogEntry(error, null);
        }

        public void Log(Exception ex)
        {
            WriteLogEntry(ex.Message, ex.StackTrace);
        }

        #endregion
    }
}
