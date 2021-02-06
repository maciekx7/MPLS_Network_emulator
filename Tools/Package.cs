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
namespace Tools
{
    public class Package
    {
        private String Key { set; get; }
        private String data { set; get; }

        private String getKey() => Key;


        private Package() { }

        //---------------Management Respond about Label to Host----------------
        /// <summary>
        /// Check if package comming from Management to Host
        /// </summary>
        /// <returns>True if yes, false if no</returns>
        public bool isLabelCommunication()
        {
            if(Key == "MANAGEMENTTOHOST")
            {
                return true;
            } else
            {
                return false;
            }
        }
        /// <summary>
        /// Prepare ManagementAnswer to send via UDP/TCP
        /// To send via UDP, use Label(package).toBytes()
        /// </summary>
        /// <param name="answer">Prepared ManagementAnswer package</param>
        /// <returns>Package</returns>
        public static Package Label(ManagementAnswer answer)
        {
            Package package = new Package();
            package.Key = "MANAGEMENTTOHOST";
            package.data = answer.toString();

            return package;
        }



        /// <summary>
        /// Creates Label ManagementAnsware from bytes
        /// Management sends in respond available label or "null"
        /// </summary>
        /// <param name="respond">Bytes from which ManagementAnswer will be created</param>
        /// <returns>ManagementAnswer created from HostAnswer</returns>
        public static ManagementAnswer ReceiveLabel(byte[] respond)
        {
            Package package = Package.fromBytes(respond);
            return ManagementAnswer.HostAnswer(int.Parse(package.data));
        }

        /// <summary>
        /// Creates Label ManagementAnsware from previously opened package.
        /// Management sends in respond available label or "null"
        /// </summary>
        /// <param name="package">Opened Package got from UDP/TCP</param>
        /// <returns>ManagementAnswer created from HostAnswer</returns>
        public static ManagementAnswer ReceiveLabel(Package package)
        {
            return ManagementAnswer.HostAnswer(int.Parse(package.data));
        }


        /*
        //--------------Management Respond about tunel closign-------------
        /// <summary>
        /// Check if package comming from Management to Host with closing tunel acceptation
        /// </summary>
        /// <returns>True if yes, false if no</returns>
        public bool isTunelCloseAcceptation()
        {
            if (Key == "TUNELCLOSEACCEPT")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Prepare ManagementAnswer to send via UDP/TCP
        /// To send via UDP, use Label(package).toBytes()
        /// </summary>
        /// <param name="answer">Prepared ManagementAnswer package</param>
        /// <returns>Package</returns>
        public static Package TunelCloseAcceptation(ManagementAnswer answer)
        {
            Package package = new Package();
            package.Key = "TUNELCLOSEACCEPT";
            package.data = answer.toString();

            return package;
        }
        */

        /*
        /// <summary>
        /// Creates TunelCloseAccept ManagementAnsware from bytes
        /// Management sends in respond closingAcceptation
        /// </summary>
        /// <param name="respond">Bytes from which ManagementAnswer will be created</param>
        /// <returns>ManagementAnswer created from HostAnswer</returns>
        public static ManagementAnswer ReceiveTunelCloseAcceptation(byte[] respond)
        {
            Package package = Package.fromBytes(respond);
            return ManagementAnswer.TunelCloseAccept(int.Parse(package.data));
        }

        /// <summary>
        /// Creates Label ManagementAnsware from previously opened package.
        /// Management sends in respond available label or "null"
        /// </summary>
        /// <param name="package">Opened Package got from UDP/TCP</param>
        /// <returns>ManagementAnswer created from HostAnswer</returns>
        public static ManagementAnswer ReceiveTunelCloseAcceptation(Package package)
        {
            return ManagementAnswer.TunelCloseAccept(int.Parse(package.data)); 
        }
        */
        //---------------Management Respond about RoutingTables----------------

        /// <summary>
        /// Check if package comming from Management to Router
        /// </summary>
        /// <returns>True if yes, false if no</returns>
        public bool isRoutingCommunication()
        {
            if (Key == "ROUTINGTABLE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Prepare ManagementAnswer to send via UDP/TCP
        /// To send via UDP, use Label(package).toBytes()
        /// </summary>
        /// <param name="answer">Prepared ManagementAnswer package</param>
        /// <returns>Package object</returns>
        public static Package RoutingTables(ManagementAnswer answer)
        {
            Package package = new Package();
            package.Key = "ROUTINGTABLE";
            package.data = answer.toString();

            return package;
        }
        
        /// <summary>
        /// Creates RoutingTables ManagementAnsware from bytes
        /// Management sends in respond routing tables to Router
        /// </summary>
        /// <param name="respond">Bytes from which ManagementAnswer will be created</param>
        /// <returns>ManagementAnswer created from RoutingAnswer</returns>
        public static ManagementAnswer ReceiveRoutingTables(byte[] respond)
        {
            Package package = Package.fromBytes(respond);
            return ManagementAnswer.RouterAnswer(package.data);
        }
        
        /// <summary>
        /// Creates RoutingTables ManagementAnsware from previously opened package.
        /// Management sends in respond routing tables to Router
        /// </summary>
        /// <param name="package">Opened Package got from UDP/TCP</param>
        /// <returns>ManagementAnswer created from RoutingAnswer</returns>
        public static ManagementAnswer ReceiveRoutingTables(Package package)
        {
            return ManagementAnswer.RouterAnswer(package.data);
        }




        //-----------Management Message----------------------------------------

        /// <summary>
        /// Check if message is type of ManagementMessage.
        /// Only this message should be supported by Management
        /// </summary>
        /// <returns>True if yes, False if not</returns>
        public bool isManagementMessage()
        {
            if(Key == "MANAGEMENTMESSAGE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Prepare MPLSPackage to send via UDP/TCP
        /// To send via UDP, use
        /// <example>ManagementMessage_(message).toBytes()</example>
        /// </summary>

        /// <param name="message">Prepared ManagementMessage ready to send</param>
        /// <returns>Package</returns>
        public static Package ManagementMessage_(ManagementMessage message)
        {
            Package package = new Package();
            package.Key = "MANAGEMENTMESSAGE";
            package.data = message.toString();

            return package;
        }

        /// <summary>
        /// Create ManagementMessage from bytes received from UDP/TCP client
        /// </summary>
        /// <param name="messageBytes">Bytes containing messageBytes</param>
        /// <returns>ManagementMessage object</returns>
        public static ManagementMessage ReceiveManagementMessage(byte[] messageBytes)
        {
            Package package = Package.fromBytes(messageBytes);
            return ManagementMessage.fromString(package.data);
        }

        /// <summary>
        /// Create ManagementMessage from opened package 
        /// </summary>
        /// <param name="package">Opened Package</param>
        /// <returns>ManagementMessage object</returns>
        public static ManagementMessage ReceiveManagementMessage(Package package)
        {
            return ManagementMessage.fromString(package.data);
        }



        //------------MPLS Package---------------------------------------------
        /// <summary>
        /// Chceck if Package if type of MPLSPackge
        /// </summary>
        /// <returns>True if MPLS, false if not</returns>
        public bool isMPLS()
        {
            if(Key == "MPLS")
            {
                return true;
            }else
            {
                return false;
            } 
        }

        /// <summary>
        /// Prepare MPLSPackage to send via UDP/TCP
        /// To send via UDP, use MPLSPackage_(mpls).toBytes()
        /// </summary>
        /// <param name="mpls"></param>
        /// <returns>Package object ready to send by UDP/TCP after converting to bytes</returns>
        public static Package MPLSPackage_(MPLSPackage mpls)
        {
            Package package = new Package();
            package.Key = "MPLS";
            package.data = mpls.toString();

            return package;
        }


        /// <summary>
        /// Create MPLS package from bytes received from UDP/TCP client
        /// </summary>
        /// <param name="mpls">Bytes containing MPLSPackage</param>
        /// <returns>MPLSPackage object</returns>
        public static MPLSPackage ReceiveMPLSPackage(byte[] mpls)
        {
            Package package = Package.fromBytes(mpls);
            return MPLSPackage.fromString(package.data);
        }

        /// <summary>
        /// Create MPLS package from opend Package 
        /// </summary>
        /// <param name="package">Package previously opened</param>
        /// <returns>MPLSPackage object</returns>
        public static MPLSPackage ReceiveMPLSPackage(Package package)
        {
            return MPLSPackage.fromString(package.data);
        }



        //----------------------------------TERMINATE ALL APS MESSAGE---------------------------
        public  bool isTerminateOrder()
        {
            if(Key == "TERMINATE")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public static Package SendTerminateOrder()
        {
            Package package = new Package();
            package.Key = "TERMINATE";
            package.data = "TERMINATEORDER";

            return package;
        }

        public static Package ReceiveTerminateOrder(byte[] order)
        {
            return Package.fromBytes(order);
        }
        


        /*
        //-----------PackageType-----------
        public String showPackageType()
        {
            String PackageType ="ERROR_NO_Package_type";
            if(Key == "MPLS")
            {
                PackageType = "MPLS_Package";
            }
            else if (Key == "MANAGEMENTMESSAGE")
            {
                PackageType = "Management_Message";
            }
            else if (Key == "ROUTINGTABLE")
            {
                PackageType = "Management_Answer_to_router";
            }
            else if (Key == "MANAGEMENTTOHOST")
            {
                PackageType = "Management_Answer_to_host";
            }

            return PackageType;
        }
        */








        //-------------CONVERTERS------------------
        /// <summary>
        /// Converting Package object to byte[] form.
        /// </summary>
        /// <returns>byte[] array containing Package object</returns>
        public byte[] toBytes()
        {
            return Encoding.ASCII.GetBytes(this.toString());
        }


        /// <summary>
        /// Creating Package object from byte[] array
        /// </summary>
        /// <param name="packageBytes">byte[] array. From that bytes Package object will be created</param>
        /// <returns>Package object</returns>
        public static Package fromBytes(byte[] packageBytes)
        {
            String dataString = System.Text.Encoding.UTF8.GetString(packageBytes);

            return fromString(dataString);
        }

        private static Package fromString(String stringMessage)
        {
            //Console.WriteLine(stringMessage);
            String[] dataString = stringMessage.Split('$');

            Package package = new Package();
            try
            {
                package.Key = dataString[0];
                package.data = dataString[1];

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return package;
        }

        /// <summary>
        /// Converts Package to string
        /// </summary>
        /// <returns>Pakcage as string</returns>
        public String toString()
        {
            //Splitting mark -> "$"
            String stringPackage = this.Key + "$" + this.data;

            return stringPackage;
        }



    }
}
