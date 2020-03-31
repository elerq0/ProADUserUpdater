using System;
using System.IO;

namespace ADUserUpdater
{
    public class Logger
    {
        private readonly string path;
        private readonly int level;
        public Logger(string logPath, int logLevel, string logName)
        {
            path = logPath + @"\" + logName + ".log";
            level = logLevel;

            try
            {
                if (!File.Exists(path))
                    File.Create(path).Close();
            }
            catch (Exception e)
            {
                throw new Exception("Wystąpił bład przy tworzeniu pliku logu: " + e.Message);
            }
        }

        public void Log(string msg, int logLevel)
        {
            if (logLevel < level)
                return;

            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(GetLogTime() + " - " + msg);
                    sw.AutoFlush = true;
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Wystąpił bład przy zapisywaniu wiadomości do logu: " + e.Message);
            }
        }

        private string GetLogTime()
        {
            DateTime now = DateTime.Now;
            return IntToLazyZeroedString(now.Day, 2) + "/" + IntToLazyZeroedString(now.Month, 2) + "/" + IntToLazyZeroedString(now.Year, 4) + " " +
                    IntToLazyZeroedString(now.Hour, 2) + ":" + IntToLazyZeroedString(now.Minute, 2) + ":" + IntToLazyZeroedString(now.Second, 2);
        }

        private string IntToLazyZeroedString(int value, int decNumber)
        {
            string str = value.ToString();
            while (str.Length < decNumber)
                str = "0" + str.ToString();
            return str;
        }
    }
}
