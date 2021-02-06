using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Text;
namespace Tools
{
    public class ManagementAnswer
    {
        public String Key { set; get; }
        public String answer { set; get; }

        public String getAnswer() => answer;

        public ManagementAnswer() { }

        public ManagementAnswer(String jsonText)
        {
            this.answer = jsonText;
        }

        //--------------RESPOND TO HOST WITH OPEN TUNEL-------------
        public static ManagementAnswer HostAnswer(int label)
        {
            ManagementAnswer answer = new ManagementAnswer();
            answer.Key = "LABELFORHOST";
            answer.answer = label.ToString();

            return answer;
        }

        public bool isToHost()
        {
            if(Key == "LABELFORHOST")
            {
                return true;
            } else
            {
                return false;
            } 
        }

        

        public bool isLabelAvaliable()
        {
            if(answer != "0")
            {
                try
                {
                    if(int.Parse(answer) is int)
                    {
                        return true;
                    }
                } catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return false;
        }
        /*
        //----------RESPOND TO HOST WITH TUNEL CLOSING-----
        public static ManagementAnswer TunelCloseAccept(int label)
        {
            ManagementAnswer answer = new ManagementAnswer();
            answer.Key = "TUNELCLOSED";
            answer.answer = label.ToString();

            return answer;
        }

        public bool isToHostTunelClosingAccept()
        {
            if (Key == "TUNELCLOSED")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */

        //-----------RESPOND TO ROUTER--------------
        public static ManagementAnswer RouterAnswer(String routingTable)
        {
            ManagementAnswer answer = new ManagementAnswer();
            answer.Key = "ROUTINGTABLE";
            answer.answer = routingTable;

            return answer;
        }

        public bool isRoutingTable()
        {
            if(Key == "ROUTINGTABLE")
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

        public static ManagementAnswer fromBytes(byte[] answerBytes)
        {
            String dataString = System.Text.Encoding.UTF8.GetString(answerBytes);

            return fromString(dataString);
        }

        public static ManagementAnswer fromString(String stringMassage)
        {
           

            ManagementAnswer answer = new ManagementAnswer();
            try
            {
                answer.answer = stringMassage;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return answer;
        }
        public String toString()
        {
            //Splitting mark -> "#"
            String stringPackage = this.answer;

            return stringPackage;
        }

        public String showData()
        {
            
            return answer;
        
        }

    }
}
