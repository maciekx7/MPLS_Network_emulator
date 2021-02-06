using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using System.Text.Json;

namespace Router
{
    public class NHLFE
    {
        public class NHFLERow
        {

            public int port_in { get; set; }
            public int label_in { get; set; }
            public int port_out { get; set; }
            public int label_out { get; set; }
            public int StackPtr { get; set; }
            public String method { get; set; }
            public int next_id { get; set; }

            public NHFLERow(int port_in, int label_in, int port_out, int label_out, int StackPtr, String method, int next_id)
            {

                this.port_in = port_in;
                this.label_in = label_in;
                this.port_out = port_out;
                this.label_out = label_out;
                this.StackPtr = StackPtr;
                this.method = method;
                this.next_id = next_id;
            }

        }

        public class NHFLETableModel
        {
            public List<NHFLEEntry> NHLFEEntries { set; get; }
        }

        public class NHFLEEntry
        {

            public int id { get; set; }
            public int port_in { get; set; }
            public int label_in { get; set; }
            public int port_out { get; set; }
            public int label_out { get; set; }
            public int StackPtr { get; set; }
            public String method { get; set; }
            public int next_id { get; set; }
        }

        public static Dictionary<int, NHLFE.NHFLERow> setRoutingTables(ManagementAnswer answer)
        {
            Dictionary<int, NHLFE.NHFLERow> ComutationList = new Dictionary<int, NHLFE.NHFLERow>();

            String jsonFile = answer.answer;
            NHLFE.NHFLETableModel TableModel = JsonSerializer.Deserialize<NHLFE.NHFLETableModel>(jsonFile);
            ComutationList = new Dictionary<int, NHLFE.NHFLERow>();

            try
            {
                foreach (NHLFE.NHFLEEntry element in TableModel.NHLFEEntries)
                {
                    ComutationList.Add(element.id, new NHLFE.NHFLERow(element.port_in, element.label_in, element.port_out, element.label_out, element.StackPtr, element.method, element.next_id));
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }

            return ComutationList;
        }
    }
}
