using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ADUserUpdater
{
    class User
    {
        public string GivenName;
        public string Surname;
        public string TitlePL;
        public string TitleEN;
        public string MobilePhone;
        public string SID;
        public string ManagerEmailAddress;


        public User(PSObject UserObject)
        {
            GivenName = GetParameterFromRegex(UserObject, "GivenName");
            Surname = GetParameterFromRegex(UserObject, "Surname");
            TitlePL = GetParameterFromRegex(UserObject, "Title");
            MobilePhone = GetParameterFromRegex(UserObject, "MobilePhone");
            SID = GetParameterFromRegex(UserObject, "SID");
        }

        public void UpdateManager(string email)
        {
            ManagerEmailAddress = email;
        }

        public void UpdateTitleEN(string title)
        {
            TitleEN = title;
        }

        private string GetParameterFromRegex(PSObject psobject, string parameter)
        {
            try
            {
                string text = JsonConvert.SerializeObject(psobject, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

                Regex rx = new Regex(@"<S N=\\""" + parameter + @"\\"">(.|[^<]+)<\/S>");
                MatchCollection matches = rx.Matches(text);
                if (matches.Count == 0)
                    return String.Empty;
                else
                    return matches[0].Groups[1].Value;
            }
            catch (Exception e)
            {
                throw new Exception("Regex exception: " + e.Message);
            }
        }

    }
}
