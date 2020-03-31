using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;

namespace ADUserUpdater.Service
{
    class App
    {
        public void Run()
        {
            Logger logger = new Logger(Properties.Settings.Default.LogFilePath, Properties.Settings.Default.LogFileLevel, DateTime.Now.ToString("yyyyMMdd"));
            try
            {
                DataModule dataModule = new DataModule(logger);
                List<string> errors = new List<string>();

                DataTable dt = dataModule.GetADInfo();
                foreach (DataRow row in dt.Rows)
                {
                    List<Tuple<string, int>> results = new Application(Properties.Settings.Default.PSPath).Update(row.Field<string>("Imie"), row.Field<string>("Nazwisko"), row.Field<string>("StanowiskoPL"), row.Field<string>("StanowiskoEN"), row.Field<string>("Email"), row.Field<string>("TelKom"), row.Field<string>("ManagerEmail"));

                    foreach (Tuple<string, int> info in results)
                    {
                        logger.Log(info.Item1, info.Item2);
                        if (info.Item2 >= 2)
                            errors.Add(info.Item1);
                    }

                    Thread.Sleep(100);
                }

                string errorKey = string.Empty;
                foreach(string error in errors)
                {
                    errorKey += error + Environment.NewLine;
                }

                MailOperator.Send(Properties.Settings.Default.AppEmailAddress,
                                    Properties.Settings.Default.AppEmailPassword,
                                    "smtp.gmail.com",
                                    Properties.Settings.Default.TargetEmailAddress,
                                    "AD Info Error", 
                                    errorKey);

                dataModule.Dispose();
            }
            catch (Exception e)
            {
                logger.Log(e.Message, 2);
            }
        }
    }
}
