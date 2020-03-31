namespace ADUserUpdater.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger(Properties.Settings.Default.LogFilePath, Properties.Settings.Default.LogFileLevel, "log");

            var info = new Application(Properties.Settings.Default.LogFilePath).Update("Łukasz1","Rędziński1","StanPL","StanEN","l.redzinski@kgl.pl", "123321","w.golabb@kgl.pl");

            for(int i = 0; i < info.Count; i++)
            {
                logger.Log(info[i].Item1, info[i].Item2);
            }
        }
    }
}
