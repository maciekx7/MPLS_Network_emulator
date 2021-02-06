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
using System.Threading;

namespace CableCloud
{
    public class CableCloud
    {
        public IPEndPoint CableCloudEndPoint { get; set; }

        public IPEndPoint NextEndPoint { get; set; }

        public IPEndPoint LastEndPoint { get; set; }

        
        // Port (MPLS)
        public Dictionary<int, Cable> ConnectedCableList { get; set; }

        // IP and port (UDP)
        public Dictionary<String, IPEndPoint> IPlist { set; get; }

        UdpClient udp;


        public class Cable
        {
            public int OutPort { get; set; }
            public String OutNodeName { get; set; }
            public Boolean isAvailable { get; set; }
            public int Capacity { get; set; }

            public Cable(String Name, int Port, int Capacity)
            {
                this.OutNodeName = Name;
                this.OutPort = Port;
                this.isAvailable = true;
                this.Capacity = Capacity;
            }

        }
        

        public CableCloud(String filename)
        {
            CableCloudConfigReader.LoadCableCloudConfig(this, filename);
            Console.WriteLine($"Cloud: {CableCloudEndPoint.Address}:{CableCloudEndPoint.Port}");
            Console.Title = "Cable Cloud";


            asyncStart();

        }


        public void asyncStart()
        {
            udp = new UdpClient(CableCloudEndPoint);

            listener();
            while (true)
            {
                UserInterface();
            }


        }

        public void listener()
        {
            Task.Run(async () =>
            {
                using (var updClient = udp)
                {
                    while (true)
                    {
                        
                        var result = await updClient.ReceiveAsync();
                        Package package = Package.fromBytes(result.Buffer);

                        if (package.isMPLS())
                        {
                            MPLSPackage MPLSpackage = Package.ReceiveMPLSPackage(package);
                            Logs.LogsCableCloudReceivePackage(MPLSpackage);

                            var InPort = MPLSpackage.GetPort();
                            var cable = ConnectedCableList[InPort];

                            if (ConnectedCableList[InPort].isAvailable == false)
                            {
                                Logs.LogsCableCloudSendingError(ConnectedCableList[InPort].OutNodeName, ConnectedCableList[ConnectedCableList[InPort].OutPort].OutNodeName);
                            }
                            else
                            {

                                MPLSpackage.swapPort(cable.OutPort);
                                var outNodeName = cable.OutNodeName;
                                var outIPEndPoint = IPlist[outNodeName];

                                
                                IPEndPoint destination = outIPEndPoint;

                                Package package1 = Package.MPLSPackage_(MPLSpackage);
                                Logs.LogsCableCloudSendPackage(MPLSpackage);
                                udp.Send(package1.toBytes(), package1.toBytes().Length, destination);
                                package1 = null;
                                MPLSpackage = null;
                            }
                        }
                        else if (package.isTerminateOrder())
                        {
                            Environment.Exit(0);
                        }
                        package = null;
                    }
                }
            });
        }


        public void UserInterface()
        {


            Console.WriteLine("\n\n---------Available options---------\n 1 -> Shows list of cables \n 2 -> Select cable to kill\n 3 -> Select cable to repare\n");
            String Command = Console.ReadLine();

            switch (Command)
            {
                case "1":
                    for (int i = 0; i < ConnectedCableList.Count; i++)
                    {
                        Console.WriteLine("NodeIn Name: {0}, Port in: {1}, NodeOut Name: {2}, Port out: {3}, Capacity: {4} Gb/s, is Available: {5} ", ConnectedCableList[ConnectedCableList.ElementAt(i).Value.OutPort].OutNodeName, ConnectedCableList.ElementAt(i).Key, ConnectedCableList.ElementAt(i).Value.OutNodeName, ConnectedCableList.ElementAt(i).Value.OutPort, ConnectedCableList.ElementAt(i).Value.Capacity, ConnectedCableList.ElementAt(i).Value.isAvailable);
                        i++;
                    }
                    break;

                case "2":
                    try
                    {
                        Console.WriteLine("Type port of cable:");
                        String PortToKill = Console.ReadLine();
                        ConnectedCableList[int.Parse(PortToKill)].isAvailable = false;
                        var portToKill_2 = ConnectedCableList[int.Parse(PortToKill)];
                        ConnectedCableList[portToKill_2.OutPort].isAvailable = false;
                        Logs.LogsCableCloudKillCable(ConnectedCableList[int.Parse(PortToKill)].OutNodeName, ConnectedCableList[ConnectedCableList[int.Parse(PortToKill)].OutPort].OutNodeName);
                    
                    }
                    catch
                    {
                        Console.WriteLine("This port number doesn't exist");
                    }
                    break;

                case "3":
                    try
                    {
                        Console.WriteLine("Type port of cable:");
                        String PortToKill = Console.ReadLine();
                        ConnectedCableList[int.Parse(PortToKill)].isAvailable = true;
                        var portToKill_2 = ConnectedCableList[int.Parse(PortToKill)];
                        ConnectedCableList[portToKill_2.OutPort].isAvailable = true;
                        Logs.LogsCableCloudRepareCable(ConnectedCableList[int.Parse(PortToKill)].OutNodeName, ConnectedCableList[ConnectedCableList[int.Parse(PortToKill)].OutPort].OutNodeName);
                    }
                    catch
                    {
                        Console.WriteLine("This port number doesn't exist");
                    }
                    break;
               
                default:
                    Console.WriteLine("Wrong command");
                    break;
            }


        }




    }
}
