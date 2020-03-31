using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace ADUserUpdater
{
    public static class Extensions
    {
        public static string PSPath; //= Properties.Settings.Default.ScriptPath + @"\";
        public static readonly string PSCommandRead = "ADUserUpdateRead.ps";
        public static readonly string PSCommandWrite = "ADUserUpdateWrite.ps";


        public static Tuple<Collection<PSObject>, List<Tuple<string, int>>> RunPowerShellScript(string command, string[] args)
        {
            List<Tuple<string, int>> info = new List<Tuple<string, int>>();
            info.Add(Tuple.Create("Running script [" + command + "] with parameters : [" + string.Join(", ", args) + "]", 0));

            Collection<PSObject> col = null;
            using (PowerShell ps = PowerShell.Create())
            {
                string cmd = GetCommand(PSPath + command, args);
                info.Add(Tuple.Create("Preparing to run PSScript {" + Environment.NewLine + cmd + Environment.NewLine + "}", 0));
                ps.AddScript(cmd);
                try
                {
                    info.Add(Tuple.Create("Invoking script", 0));
                    col = ps.Invoke();
                    info.Add(Tuple.Create("Script invokation completed succesfully", 0));
                }
                catch (Exception e)
                {
                    info.Add(Tuple.Create("Script invokation failed", 0));
                    info.Add(Tuple.Create(e.Message, 2));

                    Collection<ErrorRecord> errorPS = ps.Streams.Error.ReadAll();
                    foreach (ErrorRecord err in errorPS)
                    {
                        info.Add(Tuple.Create(err.Exception.Message, 2));
                    }
                }
            }
            return Tuple.Create(col, info);
        }

        private static string GetCommand(string path, string[] args)
        {
            string command = File.ReadAllText(path, Encoding.UTF8);
            for (int i = 0; i < args.Length; i++)
            {
                command = command.Replace("$args[" + i + "]", "\"" + args[i] + "\"");
            }

            return command;
        }
    }
}
