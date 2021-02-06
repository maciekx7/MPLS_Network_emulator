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
    public class HostConfigReader
    {
        public HostConfigReader() { }

            public class AvailableHost
            {
                public String HostName { set; get; }
                public String IpAddress { set; get; }
                public int Port { get; set; }
            }

            public class HostModel
            {
                public string HostName { get; set; }
                public string IpAddress { get; set; }
                public int Port { get; set; }
                public string CloudIP { get; set; }
                public int CloudPort { get; set; }
                public string ManagementIP { get; set; }
                public int ManagementPort { get; set; }
            public List<AvailableHost> HostList { set; get; }
        }
        

        public static void LoadHostConfig(Host host, String filename)
        {
            var jsonFile = File.ReadAllText(filename);
            HostModel hostModel = JsonSerializer.Deserialize<HostModel>(jsonFile);


            host.HostName = hostModel.HostName;
            host.EndPoint = new IPEndPoint(IPAddress.Parse(hostModel.IpAddress.ToString()), hostModel.Port);
            host.CloudEndPoint = new IPEndPoint(IPAddress.Parse(hostModel.CloudIP.ToString()), hostModel.CloudPort);


            host.HostsList = new Dictionary<String, IPEndPoint>();
            Console.WriteLine("-------Possilble hosts list:--------");

            foreach (AvailableHost element in hostModel.HostList)
            {
                
                Console.WriteLine($"{element.HostName}: {element.IpAddress}:{element.Port}");
                host.HostsList.Add(element.HostName, new IPEndPoint(IPAddress.Parse(element.IpAddress), element.Port));
            }

            Console.WriteLine("------------------------------------");
        }
    }
}
