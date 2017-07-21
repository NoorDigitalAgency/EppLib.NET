using System.Text;
using System.IO;
using System.Globalization;

namespace EppLib
{
    internal class SimpleLogger : IDebugger
    {
        public static string LogFilename = "easyepplog.txt";

        public void Log(byte[] bytes)
        {
            LogMessageToFile(Encoding.UTF8.GetString(bytes));
        }

        public void Log(string str)
        {
            LogMessageToFile(str);
        }

        private static void LogMessageToFile(string msg)
        {
            StreamWriter sw = File.AppendText(LogFilename);

            try
            {
                string logLine = string.Format( CultureInfo.InvariantCulture,"{0:G}: {1}.", System.DateTime.Now, msg);

                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
