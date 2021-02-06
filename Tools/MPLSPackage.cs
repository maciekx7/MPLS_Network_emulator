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
    public class MPLSPackage
    {
        private IPEndPoint DestinationEndPoint { get; set; }
        private IPEndPoint StartEndPoint { get; set; }
        private String Data { get; set; }
        private int TTL { get; set; }
        private int StackPtr { get; set; }
        private List<int> Label { get; set; }
        private int Port { get; set; }
        
                
        //------GETTERS-----------------------------
        public IPEndPoint GetDestinationEndPoint() => DestinationEndPoint;
        public IPEndPoint GetStartAddress() => StartEndPoint;
        public String GetData() => Data;
        public int GetPort() => Port;

        public int GetTTL() => TTL;
         

        //------CONSTRUCTORS-----------------------------
        private MPLSPackage() { }  //We don't want user to create empty package
                                   //but maybe empty constructor will be useful
                                   //in this class. 
                                   //IF NOT, we can delete it later
        public MPLSPackage(IPEndPoint startAddress, IPEndPoint destination, String data, int port, int label)
        {
            this.DestinationEndPoint = destination;
            this.StartEndPoint = startAddress;
            this.Port = port;
            this.Label = new List<int> { label };
            this.Data = data;
            this.TTL = 255;

        }

        public MPLSPackage(IPEndPoint startAddress, IPEndPoint destination, String data)
        {
            this.DestinationEndPoint = destination;
            this.StartEndPoint = startAddress;
            this.Port = StartEndPoint.Port;
            this.Data = data;
            this.TTL = 255;
            this.Label = new List<int> { 30 };
            if(Label.Count == 1)
            {
                this.StackPtr = 0;
            } else
            {
                this.StackPtr = 1;
            }
        }

        //Maybe other constructors will be useful
        //IF YES feel free to create one, but remember to fill every mandatory field
        

        //------DATA VISUALIZATION-----------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String: "'Data' to DestinationIPEndPoint from StartEndPoint"</returns>
        public String ShowData()
        {
            String visualisation = $"'{Data}' to {DestinationEndPoint.Address}:{DestinationEndPoint.Port} " +
                $"from {StartEndPoint.Address}:{StartEndPoint.Port}";

            return visualisation;
        }


        //------CONVERTERS-----------------------------

        //===============TO BYTES CONVERTER================================================= 
        /// <summary>
        /// MPLSPackage to Bytes[] converter
        /// </summary>
        /// <returns>Bytes array of converted MPLS package</returns>
        public byte[] toBytes()
        {
            return Encoding.ASCII.GetBytes(this.toString());
        }

        
        /// <summary>
        /// MPLSPackage to String converter
        /// Elements splitted by "#"
        /// </summary>
        /// <returns>String of MPLSPackage elements splitted by "#"</returns>
        public String toString()
        {
            //Splitting mark -> "#"
            String stringPackage = this.DestinationEndPoint.Address + ":" + this.DestinationEndPoint.Port + "#" +
                                   this.StartEndPoint.Address + ":" + this.StartEndPoint.Port + "#" +
                                   this.Data + "#" + this.TTL + "#" + this.StackPtr + "#" + this.Port + "#" + Label.Count  +"#";

            for (int i = 0; i < Label.Count; i++)
            {
                if (i == 0)
                {
                    stringPackage += Label[i];
                }
                else
                {
                    stringPackage += ":" + Label[i];
                }
            }

            return stringPackage;
        }
        //==========FROM BYTES CONVERTER====================================================== 
        /// <summary>
        /// Bytes[] to MPLSPackage converter
        /// Uses fromString() method. First converts package to String
        /// and then String to bytes.
        /// </summary>
        /// <param name="data">Bytes array from which MPSL package will be created</param>
        /// <returns>MPLSPackage created from bytes array</returns>
        public static MPLSPackage fromBytes(byte[] data)
        {
            String dataString = System.Text.Encoding.UTF8.GetString(data);

            return fromString(dataString);
        }

        /// <summary>
        /// String to MPLSPackage converter
        /// and then String to bytes.
        /// </summary>
        /// <param name="stringPackage">String of encoded MPLSPackage</param>
        /// <returns>MPLSPackage created from encoded String</returns>
        public static MPLSPackage fromString(String stringPackage)
        {
            String[] data = stringPackage.Split('#');

            MPLSPackage package = new MPLSPackage();

            package.DestinationEndPoint = new IPEndPoint(IPAddress.Parse(data[0].Split(":")[0]), int.Parse(data[0].Split(":")[1]));
            package.StartEndPoint = new IPEndPoint(IPAddress.Parse(data[1].Split(":")[0]), int.Parse(data[1].Split(":")[1]));
            package.Data = data[2];
            package.TTL = int.Parse(data[3]);
            package.StackPtr = int.Parse(data[4]);
            package.Port = int.Parse(data[5]);

            int labelCounter = int.Parse(data[6]);
            
            List<int> labelList = new List<int>();

            string[] labelElements = data[7].Split(":");

            for (int i = 0; i < labelCounter; i++)
            {
                labelList.Add(int.Parse(labelElements[i]));
            }

            package.Label = new List<int>(labelList);

            return package;
        }


        /// <summary>
        /// Swap last item in Label with value given as newlabel parameter
        /// </summary>
        /// <param name="newlabel">new label value</param>
        public void swap(int newlabel, int port)
        {
           // Console.WriteLine("elo");
            this.Label[Label.Count - 1] = newlabel;
            this.Port = port;
        }

        /// <summary>
        /// add new item or remove last one from Label based on method parameter
        /// </summary>
        /// <param name="method">push - add one entry to Label; pop - remove last entry from Label</param>
        public void ChangeLabel(String method)
        {
            if (method == "push")
            {
                this.Label.Add(-1);
                return;
            }
            else if(method == "pop")
            {
                if(this.Label.Any())
                {
                    this.Label.RemoveAt(Label.Count - 1);
                }
                else
                {
                    Console.WriteLine("MPLSPackage.ChangeLabel: Nie mozna usunac obiektu z pustej listy!!!");
                }
                return;
            }
        }

        /// <summary>
        /// Getting last label from list
        /// </summary>
        /// <returns>[int] Last label from list.  
        /// Returns '-1' if list is empty or out of array
        /// </returns>
        public int GetLastLabel()
        {
            try
            {
                return Label[Label.Count - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return -1;
        }

        /// <summary>
        /// Swapping port of MPLSPackage to new.
        /// </summary>
        /// <param name="port">New port</param>
        public void swapPort(int port)
        {
            this.Port = port;
        }

        /// <summary>
        /// Get list of labels from LabelStack in String
        /// </summary>
        /// <returns>String of labels list</returns>
        public String ShowAllLabels()
        {
            String labels = "";
            foreach(int element in Label)
            {
                labels += $"{element}, ";
            }
            //labels += " PORT: " + Port;
            return labels;
        }


        public void decrementTTL()
        {
            if(TTL > 0)
            {
                TTL--;
            }
        }

        public bool isMpslAlive()
        {
            if(TTL>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
