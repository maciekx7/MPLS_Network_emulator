using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ManagementSystem
{
    public class ManagementSystemConfigReader
    {
        public ManagementSystemConfigReader()
        {
        }
        public class NetworkNode
        {
            public String NodeName { get; set; }
            public String JsonFile { get; set; }
        }

        public class Device
        {
            public String name { get; set; }
            public String IPAddress { get; set; }
            public int port { get; set; }
        }
        public class ManagementModel
        {
            public String IpAddress { get; set; }
            public int Port { get; set; }
            public String Name { get; set; }
            public List<NetworkNode> NetworkNodeList { get; set; }
            public List<Device> DevicesList { get; set; }


        }

        public static void LoadManagementSystemConfig(ManagementSystem management, String filename)
        {
            var jsonFile = File.ReadAllText(filename);
            ManagementModel managementModel = JsonSerializer.Deserialize<ManagementModel>(jsonFile);

  
            management.Name = managementModel.Name;
            management.ManagementSystemPoint = new IPEndPoint(IPAddress.Parse(managementModel.IpAddress), managementModel.Port);



            management.DevicesList = new Dictionary<String, String>();


            
            foreach (NetworkNode element in managementModel.NetworkNodeList)
            {
                
                management.DevicesList.Add(element.NodeName, element.JsonFile);
               
            }

            management.AllDevicesList = new Dictionary<string, IPEndPoint>();


            foreach(Device device in managementModel.DevicesList)
            {
                management.AllDevicesList.Add(device.name, new IPEndPoint(IPAddress.Parse(device.IPAddress), device.port));
            }



            
        }





        
    }
}
