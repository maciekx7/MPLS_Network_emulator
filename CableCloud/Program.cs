using System;
using Tools;

namespace CableCloud
{
    class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            args = new string[1];
            args[0] = "cableCloud.json";
#endif

            

            try
            {
                CableCloud cableCloud = new CableCloud(args[0]);
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
