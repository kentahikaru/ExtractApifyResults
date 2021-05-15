using System;

namespace ExtractApifyResults.Contracts.TaskObject
{
   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class TaskObject
    {
        public Data data { get; set; }
    }
    public class Stats
    {
        public int totalRuns { get; set; }
    }

    public class Options
    {
        public string build { get; set; }
        public int timeoutSecs { get; set; }
        public int memoryMbytes { get; set; }
    }

    public class Input
    {
        public string hello { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string actId { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime modifiedAt { get; set; }
        public string removedAt { get; set; }
        public Stats stats { get; set; }
        public Options options { get; set; }
        public Input input { get; set; }
    }
}