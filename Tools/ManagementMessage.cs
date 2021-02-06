using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Text;

namespace Tools
{
    public class ManagementMessage
    {
        private IPEndPoint RequesterEndPoint { get; set; }
        private String Key { get; set; }
        private String Message { get; set; }

        public String getKey() => Key;
        public String getMessage() => Message;
        public IPEndPoint getRequesterEndPoint() => RequesterEndPoint;

        //-----------CONSTRUCTORS-------------
        public ManagementMessage(String message, IPEndPoint destination)
        {
            this.Message = message;
            this.Key = "HELLO";
            this.RequesterEndPoint = destination;
        }

        private ManagementMessage() { }

        /*
        //----------LABEL REQUEST------------
        public static ManagementMessage LabelRequest(String hostDestinationName, IPEndPoint RequesterEndPoint)
        {
            ManagementMessage message = new ManagementMessage();
            message.Key = "LABEL";
            message.Message = hostDestinationName;
            message.RequesterEndPoint = RequesterEndPoint;

            return message;
        }

        public bool isLabelRequest()
        {
            if(this.Key=="LABEL")
            {
                return true;
            } else
            {
                return false;
            }
        }


        //-----CLOSE TUNEL REQUEST----------
        public static ManagementMessage CloseTunelRequest(int labelTunelToClose, IPEndPoint RequesterEndPoint)
        {
            ManagementMessage message = new ManagementMessage();
            message.Key = "CLOSETUNEL";
            message.Message = labelTunelToClose.ToString();
            message.RequesterEndPoint = RequesterEndPoint;

            return message;
        }

        public bool isCloseTunelRequest()
        {
            if (this.Key == "CLOSETUNEL")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */

        //-------ROUTE LIST ----------------
        public static ManagementMessage RoutingTableRequest(String networkNodeName, IPEndPoint destination)
        {
            ManagementMessage message = new ManagementMessage();
            message.Key = "ROUTETABLE";
            message.Message = networkNodeName;
            message.RequesterEndPoint = destination;

            return message;
        }

        public bool isRoutingTableRequest()
        {
            if(this.Key == "ROUTETABLE")
            {
                return true;
            } else
            {
                return false;
            }
        }

        
        public byte[] toBytes()
        {
            return Encoding.ASCII.GetBytes(this.toString());
        }
        public static ManagementMessage fromBytes(byte[] data)
        {
            String dataString = System.Text.Encoding.UTF8.GetString(data);

            return fromString(dataString);
        }

        public static ManagementMessage fromString(String stringMassage)
        {
            String[] data = stringMassage.Split(':');

            ManagementMessage massage = new ManagementMessage();
            try
            {
                massage.Key = data[0];
                massage.Message = data[1];
                massage.RequesterEndPoint = new IPEndPoint(IPAddress.Parse(data[2]), int.Parse(data[3]));
                Console.WriteLine(massage.RequesterEndPoint);

            }catch(Exception e)
            {
                Console.WriteLine(e);
            }


            return massage;
        }


        public String toString()
        {
            //Splitting mark -> "#"
            String stringPackage = this.Key + ":" + this.Message + ":"
                + this.RequesterEndPoint.Address + ":" + this.RequesterEndPoint.Port;

            return stringPackage;
        }

    }
}
