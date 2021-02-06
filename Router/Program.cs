using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Router
{
    
    class Program
    {
        static void Main(string[] args)
        {

            
            
            try
            {
               // Console.SetWindowSize(20, 90);
                Router router = new Router(args[0]);
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
