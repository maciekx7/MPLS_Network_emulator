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
    public class RouterConfigReader
    {
        public RouterConfigReader()
        {
        }



        private class RouterModel
        {
            
            public String IpAddress { get; set; }
            public int Port { get; set; }
            public String Name { get; set; }
            public int CloudPort { get; set; }
            public String CloudIP { get; set; }
            public String ManagementIP { get; set; }
            public int ManagementPort { get; set; }
            
            
        }

        public static void LoadRouterConfig(Router router, String filename)
        {
            var jsonFile = File.ReadAllText(filename);
            RouterModel routerModel  = JsonSerializer.Deserialize<RouterModel>(jsonFile);


            router.EndPoint = new IPEndPoint(IPAddress.Parse(routerModel.IpAddress), routerModel.Port);
            router.CableCloudEndPoint = new IPEndPoint(IPAddress.Parse(routerModel.CloudIP), routerModel.CloudPort);
            router.ManagementEndPoint = new IPEndPoint(IPAddress.Parse(routerModel.ManagementIP), routerModel.ManagementPort);
            router.Name = routerModel.Name;


        }
    }
}
