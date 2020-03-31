using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace ADUserUpdater
{
    public class Application
    {
        public Application(string path)
        {
            Extensions.PSPath = path + @"\";
        }

        public List<Tuple<string, int>> Update(string Imie, string Nazwisko, string StanowiskoPL, string StanowiskoEN, string Email, string TelKom, string ManagerEmail)
        {
            List<Tuple<string, int>> log = new List<Tuple<string, int>>();
            log.Add(Tuple.Create("Reading user [" + Email + "]", 1));

            try
            {
                string[] argsRead = { Email };
                var resultRead = Extensions.RunPowerShellScript(Extensions.PSCommandRead, argsRead);

                User user = new User(resultRead.Item1[0]);
                user.UpdateManager(resultRead.Item1[1].ToString());
                user.UpdateTitleEN(resultRead.Item1[2].ToString());

                foreach (Tuple<string, int> info in resultRead.Item2)
                {
                    log.Add(info);
                }

                if (user.GivenName.Equals(Imie))
                    Imie = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [Imie] needs update", 1));

                if (user.Surname.Equals(Nazwisko))
                    Nazwisko = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [Nazwisko] needs update", 1));

                if (user.TitlePL.Equals(StanowiskoPL))
                    StanowiskoPL = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [StanowiskoPL] needs update", 1));

                if (user.TitleEN.Equals(StanowiskoEN))
                    StanowiskoEN = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [StanowiskoEN] needs update", 1));

                if (user.MobilePhone.Equals(TelKom))
                    TelKom = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [TelKom] needs update", 1));

                if (user.ManagerEmailAddress.Equals(ManagerEmail))
                    ManagerEmail = "";
                else
                    log.Add(Tuple.Create("[" + Email + "]: " + "Parameter [ManagerEmail] needs update", 1));

                if (Imie != "" || Nazwisko != "" || StanowiskoPL != "" || StanowiskoEN != "" || TelKom != "" || ManagerEmail != "")
                {
                    log.Add(Tuple.Create("Starting update user [" + Email + "]", 1));

                    string[] argsWrite = { user.SID, Imie, Nazwisko, StanowiskoPL, StanowiskoEN, TelKom, ManagerEmail };
                    var resultWrite = Extensions.RunPowerShellScript(Extensions.PSCommandWrite, argsWrite);

                    foreach (Tuple<string, int> info in resultWrite.Item2)
                    {
                        log.Add(info);
                    }

                    if (resultWrite.Item1 != null)
                        foreach (PSObject info in resultWrite.Item1)
                        {
                            if (info.ToString().StartsWith("Error"))
                                log.Add(Tuple.Create("[" + Email + "]: " + info.ToString(), 2));
                            else
                                log.Add(Tuple.Create("[" + Email + "]: " + info.ToString(), 1));
                        }
                }
            }
            catch (Exception e)
            {
                log.Add(Tuple.Create(e.Message, 2));
            }
            return log;
        }
    }
}
