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
namespace Router
{

    public class Router
    {
       
        public IPEndPoint EndPoint { get; set; }
        public IPEndPoint CableCloudEndPoint { get; set; }
        public IPEndPoint ManagementEndPoint { get; set; }
        public String Name { get; set; }
        
        public Boolean nhfletable_set = false;

        public Dictionary<int, NHLFE.NHFLERow> ComutationList { get; set; }

        UdpClient udp;

        public Router(String filename)
        {
            RouterConfigReader.LoadRouterConfig(this, filename);

            Console.WriteLine($"ROUTER {Name}: {EndPoint.Address}:{EndPoint.Port}");
            Console.Title = Name;


            udp = new UdpClient(EndPoint);
            asyncStart();
        }


        public void asyncStart()
        {
           
            listener();

            while(true)
            {
                ManagementMessage message = ManagementMessage.RoutingTableRequest(Name, EndPoint);


                Package package2 = Package.ManagementMessage_(message);
                byte[] messageBytes = package2.toBytes();
                Logs.LogsSendRoutingTableRequest(message, Name);
                udp.Send(messageBytes, messageBytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.2"), 4321));
                Thread.Sleep(5000);
                if (nhfletable_set == true)
                {
                    break;
                }
            }

            while (true)
            {
                Console.ReadLine();
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

                        if(package.isRoutingCommunication() && nhfletable_set == false)
                        {
                            ManagementAnswer answer1 = Package.ReceiveRoutingTables(package);

                            ComutationList =  NHLFE.setRoutingTables(answer1);
                            nhfletable_set = true;
                            Logs.LogsReceiveRoutingTable(Name);
                        }

                        if (package.isMPLS())
                        {
                            if(nhfletable_set)
                            {
                                MPLSPackage MPLSpackage = Package.ReceiveMPLSPackage(package);
                                Console.WriteLine($"otrzymany TTL: {MPLSpackage.GetTTL()}");
                                MPLSpackage.decrementTTL();
                                if (MPLSpackage.GetTTL() > 0)
                                {
                                    Logs.LogsRouterReceivePackage(MPLSpackage, Name);
                                    var InPort = MPLSpackage.GetPort();
                                    int label = MPLSpackage.GetLastLabel();
                                    int row_id = get_nhfle_row_id(InPort, label);
                                    if (row_id > -1)
                                    {
                                        switch (ComutationList[row_id].method)
                                        {
                                            case "swap":
                                                MPLSpackage.swap(ComutationList[row_id].label_out, ComutationList[row_id].port_out);
                                                break;
                                            case "push":
                                                push(ComutationList[row_id], MPLSpackage);
                                                break;
                                            case "pop":
                                                pop(ComutationList[row_id], MPLSpackage);
                                                break;
                                        }
                                        Console.WriteLine($"wysyłany TTL: {MPLSpackage.GetTTL()}");
                                        Package package1 = Package.MPLSPackage_(MPLSpackage);
                                        Logs.LogsRouterSendPackage(MPLSpackage, Name);
                                        udp.Send(package1.toBytes(), package1.toBytes().Length, CableCloudEndPoint);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Podana etykieta jest niepoprawna");
                                    }
                                    
                                    //Thread.Sleep(1000);
                                }

                            }
                            
                        }
                        else if (package.isTerminateOrder())
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            });
        }


        

        private int get_nhfle_row_id(int port, int label)
        {
            foreach (KeyValuePair<int, NHLFE.NHFLERow> item in ComutationList)
            {
                if (item.Value.label_in == label && item.Value.port_in == port)
                {
                    return item.Key;
                }
            }
            return -1;
        }

        public void push(NHLFE.NHFLERow entry, MPLSPackage MPLSpackage)
        {
            
            MPLSpackage.swap(entry.label_out, entry.port_out);
            MPLSpackage.ChangeLabel("push");
            NHLFE.NHFLERow nextentry = ComutationList[entry.next_id];
            if (nextentry.method == "swap")
            {
                MPLSpackage.swap(nextentry.label_out, nextentry.port_out);
                return;
            }
            while (nextentry.method != "swap")
            {
                MPLSpackage.swap(nextentry.label_out, nextentry.port_out);
                MPLSpackage.ChangeLabel("push");
                nextentry = ComutationList[nextentry.next_id];
                
            }
            MPLSpackage.swap(nextentry.label_out, nextentry.port_out);
        }

        public void pop(NHLFE.NHFLERow entry, MPLSPackage MPLSpackage)
        {
            
            MPLSpackage.ChangeLabel("pop");
            NHLFE.NHFLERow nextentry = ComutationList[entry.next_id];
            if (nextentry.method == "swap")
            {
                MPLSpackage.swap(nextentry.label_out, nextentry.port_out);
                return;
            }
            while (nextentry.method != "swap")
            {
                MPLSpackage.ChangeLabel("pop");
                nextentry = ComutationList[nextentry.next_id];

            }
            MPLSpackage.swap(nextentry.label_out, nextentry.port_out);
        }

    }
}
