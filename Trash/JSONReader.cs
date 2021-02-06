using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Trash
{
    public class JSONReader
    {
        public JSONReader() { }

        public class ObjList
        {
            public int Example { set; get; }
            public String Example1 { set; get; }
        }

        public class Model
        {
            public String First { set; get; }
            public int Second { set; get; }
            public List<ObjList> ObjList { set; get; }
        }

        public static void ReadConfig(String filename)
        {
            var jsonFile = File.ReadAllText(filename);
            Model model = JsonSerializer.Deserialize<Model>(jsonFile);

            Console.WriteLine($"OBJECT: {model.First}, {model.Second}");
            foreach(ObjList obj in model.ObjList)
            {
                Console.WriteLine($"{obj.Example}, {obj.Example1}");
            }

        }
    }
}
