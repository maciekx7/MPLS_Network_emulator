using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Tools;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;
using System.Text;
namespace Host
{
    public class HostValidationOptions
    {
        public HostValidationOptions()
        {
        }


        public static int labelSelect()
        {
            int label;
            String SLabel;
            while(true)
            {
                try
                {
                    Console.WriteLine("[LABEL]");
                    SLabel = Console.ReadLine();
                    label = int.Parse(SLabel);
                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine("[ERROR] IT'S NOT INT!");
                }
                
                
            }
            return label;
        }

        public static String menuSelect()
        {
            String select = null;

            while(true)
            {
                Console.WriteLine("Select option:");
                Console.WriteLine("[1] Open new Tunel");
                Console.WriteLine("[2] Close tunel");
                Console.WriteLine("[3] Show opened tunnels");
                select = Console.ReadLine();

                if(select=="1" || select == "2" || select == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("[WARNING] WRONG SELECTION");
                }
            }
            return select;
        }

        public static void ShowOpenedTunnles_(Dictionary<int, String> TunelDic)
        {
            Console.WriteLine("---------------------");
            var sortedLabels = TunelDic.Keys.ToList();
            sortedLabels.Sort();
            foreach (int label in sortedLabels)
            {
                Console.WriteLine($"{label}-{TunelDic[label]}");
            }
            Console.WriteLine("---------------------");
        }


        public static int TunelToClose(Dictionary<int,String> TunelDic)
        {
            int closingTunel;
            while (true)
            {
                Console.WriteLine("Which tunel you wanna close?");
                var sortedLabels = TunelDic.Keys.ToList();
                sortedLabels.Sort();
                foreach (int label in sortedLabels)
                {
                    Console.WriteLine($"{label}-{TunelDic[label]}");
                }


                var closingTunelDec = Console.ReadLine();
                try
                {
                    closingTunel = int.Parse(closingTunelDec);
                    if (TunelDic.ContainsKey(closingTunel))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("[WARNING] WRONG TUNEL SELECTED!");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("[WARNING] WRONG TUNEL SELECTED!");
                }
                catch (FormatException)
                {
                    Console.WriteLine("[WARNING] WRONG TUNEL SELECTED!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return closingTunel;
        }


        /// <summary>
        /// Asks user to select host to whom he wanna send message
        /// from list of avaliable hosts in your network
        /// </summary>
        /// <returns>Selected host checked if is on list of abaliable hosts</returns>
        public static String SelectHost(Dictionary<String, IPEndPoint> HostsList)
        {
            String select;
            while (true)
            {
                Console.WriteLine("[SELECT HOST {Hx}]");
                select = Console.ReadLine();
                if (HostsList.ContainsKey(select))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("[WARNING]> Wrong Host!");
                }
            }
            return select;
        }

        /// <summary>
        /// Asks user for message data. Checks if user wrote prohibited characters
        /// Prohibitet characters: "#", "$", ":"
        /// </summary>
        /// <returns>User message</returns>
        public static String UserMessage()
        {
            String message;
            while (true)
            {
                Console.WriteLine("[WRITE MESSAGE]");
                message = Console.ReadLine();

                if (message.Contains("$") || message.Contains("#") || message.Contains(":"))
                {
                    Console.WriteLine("[WARNING] You cannot send '#','$' or ':' character in your message");
                }
                else
                {
                    break;
                }
            }
            return message;
        }
    }
}
