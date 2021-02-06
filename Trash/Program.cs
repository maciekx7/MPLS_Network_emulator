using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Trash
{
    class Program
    {
        static void Main(string[] args)
        {
            /*List<int> list = new List<int> { 10, 12, 13 };
            String tam = "";
            foreach(int element in list)
            {
                tam += $"{element}, " ;
            }
            Console.WriteLine(tam);
            */
            /*
            JSONReader.ReadConfig("config.json");
            Console.Write("[WRITE MESSAGE]> ");
            String userData = Console.ReadLine();
            Console.Write("[SELECT HOST {Hx}]> ");
            String data = Console.ReadLine();
            Console.WriteLine(data);


            
            Console.ReadLine();*/
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo("./logs");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
