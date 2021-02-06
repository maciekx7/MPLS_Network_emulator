using System;
using System.Net;
using System.Collections.Generic;
using Tools;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Host
{
    public class Host
    {
        public IPEndPoint EndPoint { get; set; }
        public String HostName { get; set; }
        public IPEndPoint CloudEndPoint { set; get; }
        public Dictionary<String, IPEndPoint> HostsList { set; get; }

        UdpClient udp;

        
        public Host(String filename)
        {
            try
            {
                HostConfigReader.LoadHostConfig(this, filename);
                Console.WriteLine($"HOST {HostName}: {EndPoint.Address}:{EndPoint.Port}");
                Console.Title = HostName;

                udp = new UdpClient(EndPoint);

                StartSocket();
            }

            catch(Exception e)
            {
                Console.WriteLine("[HOST CONSTRUCTOR ERROR]");
                Console.WriteLine(e);
            }
        }


        public void StartSocket()
        {
            Listener();

            while (true)
            {
                String userData = HostValidationOptions.UserMessage();
                String selectedHost = HostValidationOptions.SelectHost(HostsList);
                int label = HostValidationOptions.labelSelect();

                MPLSPackage mpls = new MPLSPackage(EndPoint, HostsList[selectedHost], userData, EndPoint.Port, label);
                Package mplsSendingPackage = Package.MPLSPackage_(mpls);
                byte[] mplsSendingPackageByte = mplsSendingPackage.toBytes();
                Logs.LogsHostSendPackage(mpls, selectedHost, HostName);
                udp.Send(mplsSendingPackageByte, mplsSendingPackageByte.Length, CloudEndPoint);


            }
        }


        /// <summary>
        /// Async Task listening by UDPClinet for any sended packages.
        /// If something came to Host, is converted to Package class
        /// and then checked if message is meant for Host.
        /// If is meant for Host, proceed package.
        /// </summary>
        public void Listener()
        {
            Task.Run(async () =>
            {
                //using var updClient = udp;
                while (true)
                {
                    //Console.WriteLine("[INFO]> waiting for message");
                    var result = await udp.ReceiveAsync();
                    //Console.WriteLine("[INFO]> message received");
                    byte[] resultBytes = result.Buffer;

                    Package package = Package.fromBytes(resultBytes);
                    
                    if(package.isMPLS())
                    {
                        MPLSPackage mpls = Package.ReceiveMPLSPackage(package);
                        Logs.LogsHostReceivePackage(mpls, HostName);
                    }

                    else if(package.isTerminateOrder())
                    {
                        Environment.Exit(0);
                    }
                }
            });
        }



    }
}
