using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tools;
namespace ManagementSystem
{
    public class ManagementSystem
    {
       public IPEndPoint ManagementSystemPoint { get; set; }
        public String Name { get; set; }

        // List of routers
        public Dictionary <String, String> DevicesList { get; set; }

        // List of routers hosts and cc 
        public Dictionary<String, IPEndPoint> AllDevicesList { get; set; }

        UdpClient udp;

        public ManagementSystem(String filename)
        {
            ManagementSystemConfigReader.LoadManagementSystemConfig(this, filename);
            Console.WriteLine($"Management System: {ManagementSystemPoint.Address}:{ManagementSystemPoint.Port}");

            AsyncStart();
        }

        public void AsyncStart()
        {
            udp = new UdpClient(ManagementSystemPoint);

            Listener();

            while(true)
            {
                String close = Console.ReadLine();
                if (close == "EXIT")
                {
                    foreach(KeyValuePair<String, IPEndPoint> device in AllDevicesList)
                    {
                        Package package = Package.SendTerminateOrder();
                        byte[] bytes = package.toBytes();
                        udp.Send(bytes, bytes.Length, device.Value);
                    }
                    Environment.Exit(0);
                    break;
                }
            }


            while (true)
            {
                Console.ReadLine();
            }
        }

        public void Listener()
        {
            
            Task.Run(async () =>
            {
                using var updClient = udp;
                while (true)
                {
                    var result = await updClient.ReceiveAsync();
                    try
                    {
                        byte[] resultBytes = result.Buffer;
                        Package package = Package.fromBytes(resultBytes);
                        if (package.isManagementMessage())
                        {
                            ManagementMessage message = Package.ReceiveManagementMessage(package);

                            if(message.isRoutingTableRequest())
                            {
                                Logs.LogsReceiveRoutingTableRequest(message);
                                String routeTable = gtJSONAnswer(message);
                                ManagementAnswer answer = ManagementAnswer.RouterAnswer(routeTable);
                                byte[] package1 = Package.RoutingTables(answer).toBytes();
                                Logs.LogsSendRoutingTable(message);
                                udp.Send(package1, package1.Length, message.getRequesterEndPoint());
                            }

                            }
                       
                    }
                    catch(Exception e)
                    {
                        Logs.LogsManagementException(e);
                    }
                    
                }
            });
        }
        public String gtJSONAnswer(ManagementMessage message)
        {

            var jsonFile = File.ReadAllText(DevicesList[message.getMessage()]);
            return jsonFile;

        }

    }
}
