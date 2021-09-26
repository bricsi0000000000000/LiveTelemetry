using System;
using System.Collections.Generic;

namespace DataModel.Live
{
    public class LiveSession
    {
        public int SessionId { get; set; }
        public bool IsLive { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<string> SensorNames { get; set; }
    }
}
