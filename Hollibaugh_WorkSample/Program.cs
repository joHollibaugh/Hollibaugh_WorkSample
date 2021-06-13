using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Hollibaugh_WorkSample
{

    class Program
    {

        static void Main(string[] args)
        {
            bool running = true;
            MultiValueDictionary dict = new MultiValueDictionary();
          
            Console.WriteLine("Enter a command or '/help' for a list of commands \n");
            while (running)
            {
                // Read user input, space delimited
                var _input = Console.ReadLine();
                if(_input.ToUpper() == "EXIT")
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine(dict.HandleInput(_input) + "\n");

                }
            }
            Console.WriteLine("Finished");
        }

    }

    public class MultiValueDictionary
    {
        private Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

        public string HandleInput(string _input)
        {
            try
            {
            string[] input = _input.Split(" ");
            // Trim first char if copy pasting commands
            if (input[0] == ">")
            {
                input = input.Skip(1).ToArray();
            }
            string cmd = input[0];
            string k = string.Empty;
            string v = string.Empty;
            // call MultiValueDictionary based on input
            switch (cmd.ToUpper())
            {
                case "ADD":
                    if (input.Length != 3)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    v = input[2];
                    return this.AddMember(k, v);

                case "REMOVE":
                    if (input.Length != 3)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    v = input[2];
                    return this.RemoveMember(k, v);

                case "MEMBERS":
                    if (input.Length != 2)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    return this.GetMembersByKey(k);

                case "KEYS":
                    if (input.Length != 1)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    return this.GetKeys();

                case "REMOVEALL":
                    if (input.Length != 2)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    return this.RemoveKey(k);

                case "CLEAR":
                    if (input.Length != 1)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    return this.Clear();

                case "KEYEXISTS":
                    if (input.Length != 2)
                    {
                        return  "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    return this.KeyExists(k);

                case "MEMBEREXISTS":
                    if (input.Length != 3)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    k = input[1];
                    v = input[2];

                    return this.MemberExists(k, v);

                case "ALLMEMBERS":
                    if (input.Length != 1)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    return this.GetAllMembers(false);

                case "ITEMS":
                    if (input.Length != 1)
                    {
                        return "Invalid number of arguments for " + cmd + ".";
                    }
                    return this.GetAllMembers(true);

                case "HELP":
                    return
                        "**************************************************************************************************************** " +
                        "Enter one of the following commands. Separate arguments by a single space. " +
                        "ADD Key Value    - Adds a value to the dictionary at Key. Key is created if it doesn't already exist. " +
                        "REMOVE Key Value - Remove a member from value at Key. If Key has 0 members after the removal, Key is removed. " +
                        "MEMBERS Key      - List the members of Key a contact " +
                        "KEYS             - List all Keys " +
                        "EXIT             - Quit the application.";

                default:
                    return cmd + " is not a valid command.";
            }
            } catch(Exception e)
            {
                this.LogException(e);
                return "An Error Occurred. See log for details";
            }

        }

        public string AddMember(string key, string val)
        {
            try
            {
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new List<string>(new string[] { val }));
                    return "Added";
                }
                if (dict[key].Contains(val))
                {
                    return "ERROR, member already exists for key";
                }

                dict[key].Add(val);
                return "Added";
            }
            catch (Exception e)
            {
                LogException(e);
                return "An error occured. See log for details.";
            }
        }

        public string GetMembersByKey(string key)
        {
            if (!dict.ContainsKey(key))
            {
                return "ERROR, key does not exist.";
            }
            StringBuilder sb = new StringBuilder();
            dict[key].ForEach((val) => sb.AppendLine(val));
            return sb.ToString();
        }

        public string GetKeys()
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, List<string>>.KeyCollection keys = dict.Keys;
            foreach (string key in keys)
            {
                sb.AppendLine(key);
            }
            if (sb.ToString().Length <= 0)
            {
                return "empty set";
            }
            return sb.ToString();
        }

        public string RemoveMember(string key, string val)
        {
            try
            {
                if (!dict.ContainsKey(key))
                {
                    return "ERROR, key does not exist";
                }
                if (!dict[key].Contains(val))
                {
                    return "ERROR, member does not exist";
                }
                dict[key].Remove(val);
                if (dict[key].Count <= 0)
                {
                    dict.Remove(key);
                }
                return "Removed";
            }
            catch (Exception e)
            {
                LogException(e);
                return "An error occured";
            }
        }

        public string RemoveKey(string key)
        {
            if (!dict.ContainsKey(key))
            {
                return "ERROR, key does not exist";
            }
            dict.Remove(key);
            return "Removed";
        }

        public string KeyExists(string key)
        {
            return dict.ContainsKey(key).ToString();
        }

        public string MemberExists(string key, string val)
        {
            return (dict.ContainsKey(key) && dict[key].Contains(val)).ToString(); ;
        }

        public string GetAllMembers(bool includeKey )
        {
            StringBuilder sb = new StringBuilder();
            foreach(string key in dict.Keys)
            {
                dict[key].ForEach((val) => sb.AppendLine((includeKey? key + ": ": string.Empty) + val));
            }
            if(sb.ToString().Length <= 0)
            {
                return "empty set";
            }
            return sb.ToString();
        }

        public string Clear()
        {
            dict.Clear();
            return "Cleared";
        }


        public void LogException(Exception e)
        {
            string filePath = @"C:\VisualStudios\TestLogs\WorkExampleLog.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();
                while (e != null)
                {
                    writer.WriteLine(e.GetType().FullName);
                    writer.WriteLine("Message : " + e.Message);
                    writer.WriteLine("StackTrace : " + e.StackTrace);
                    e = e.InnerException;
                }
            }
        }

    }

}