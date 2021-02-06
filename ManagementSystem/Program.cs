using System;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace ManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Management System";

# if (DEBUG == true)
            args = new string[1];
            args[0] = "managementSystem.json";
#endif
            /*
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            Console.ReadKey();
            */
            //Tools.ConnectionSocket.SocketListener("127.0.0.1", 1212);
            try
            {
                //Console.WriteLine("Welcome to ManagementSystem..");
                ManagementSystem management = new ManagementSystem(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            finally
            {
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
