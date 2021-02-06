using System;
using System.Collections.Generic;
using Tools;
using System.Configuration;
using System.Collections.Specialized;

namespace Host
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("[Host app opened]");
#if DEBUG
            args = new string[1];
            args[0] = "host.json";
#endif

            try
            {
                Host host = new Host(args[0]);
            } catch(Exception e)
            {
                Console.WriteLine(e);
            } finally
            {
                while(true)
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
