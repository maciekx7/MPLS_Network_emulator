using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Tools
{
    public class Logs
    {
        public static String getActualTime()
        {
            // return DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss:ffff");
            return DateTime.Now.ToString("HH:mm:ss:ffff");
        }
        
        public static void LogsHostSendPackage(MPLSPackage mpls, String destHost, String HostName)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE SEND] '{mpls.GetData()}' TO: {destHost} <{mpls.GetDestinationEndPoint().ToString()}> LABEL: {mpls.ShowAllLabels()}";

            Console.WriteLine(log);
            WriteToFile(log, HostName);
        }

        public static void LogsHostReceivePackage(MPLSPackage package, String Hostname)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE RECEIVE] '{package.GetData()}' FROM: {package.GetStartAddress().ToString()}, LABELS: {package.ShowAllLabels()}";

            Console.WriteLine(log);
            WriteToFile(log, Hostname);
        }

        public static void LogsRouterSendPackage(MPLSPackage package, String Routername)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE SEND] LABELS: {package.ShowAllLabels()} PORT: {package.GetPort()}";

            Console.WriteLine(log);
            WriteToFile(log, Routername);
        }

        public static void LogsRouterReceivePackage(MPLSPackage package, String Routername)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE RECEIVE] LABELS: {package.ShowAllLabels()} PORT: {package.GetPort()}";

            Console.WriteLine(log);
            WriteToFile(log, Routername);
        }

        public static void LogsCableCloudSendPackage(MPLSPackage package)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE SEND] PORT: {package.GetPort()})";

            Console.WriteLine(log);
            WriteToFile(log, "CableCloud");
        }

        public static void LogsCableCloudReceivePackage(MPLSPackage package)
        {
            String log;

            log = $"({getActualTime()}) [MPLS PACKAGE RECEIVE] PORT: {package.GetPort()})";

            Console.WriteLine(log);
            WriteToFile(log, "CableCloud");
        }

        public static void LogsCableCloudRepareCable(String name1, String name2)
        {
            String log;

            log = $"({getActualTime()}) [CABLE REPARED] Connection between : {name1} - {name2} repared) ";
            Console.WriteLine(log);
            WriteToFile(log, "CableCloud");
        }

        public static void LogsCableCloudKillCable(String name1, String name2)
        {
            String log;

            log = $"({getActualTime()}) [CABLE DESTROYED] Connection between : {name1} - {name2} destroyed) ";
            Console.WriteLine(log);
            WriteToFile(log, "CableCloud");
        }

        public static void LogsCableCloudSendingError(String name1, String name2)
        {
            String log;

            log = $"({getActualTime()}) [SENDING ERROR] Connection between : {name1} - {name2} doesn't exist) ";
            Console.WriteLine(log);
            WriteToFile(log, "CableCloud");
        }

        public static void LogsSendRoutingTable(ManagementMessage message)
        {
            String log;

            log = $"({getActualTime()}) [ROUTING TABLE SEND] FOR {message.getMessage()}";

            Console.WriteLine(log);
            WriteToFile(log, "Management");
        }

        public static void LogsReceiveRoutingTableRequest(ManagementMessage message)
        {
            String log;

            log = $"({getActualTime()}) [ROUTING TABLE REQUEST RECEIVE] FROM {message.getMessage()}";

            Console.WriteLine(log);
            WriteToFile(log, "Management");
        }

        public static void LogsSendRoutingTableRequest(ManagementMessage message, String Routername)
        {
            String log;

            log = $"({getActualTime()}) [ROUTING TABLE REQUEST SEND]";

            Console.WriteLine(log);
            WriteToFile(log, Routername);
        }

        public static void LogsReceiveRoutingTable(String Routername)
        {
            String log;

            log = $"({getActualTime()}) [ROUTING TABLE RECEIVE]";

            Console.WriteLine(log);
            WriteToFile(log, Routername);
        }
        public static void LogsManagementException(Exception e)
        {
            String log;
            log = $"[EXCEPTION] ({getActualTime()}) Wrong Message {e} ";
            Console.WriteLine(log);
            WriteToFile(log, "Management");
        }
        public static void LogsManagementgtJSONAnswer(String jsonFile)
        {
            String log;
            log = $"({getActualTime()}) [ROUTETABLE] TO {jsonFile} ";
            Console.WriteLine(log);
            WriteToFile(log, "Management");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="nodetype">dla managementu - "Management", dla chmury - "CableCloud", dla hostow typu "Hx", dla routerow typu "Rx"</param>
        private static void WriteToFile(String log, String nodetype)
        {
            string path = $"./logs/logs{nodetype}.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(log);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(log);
                }
            }
                
        }

    }
}
