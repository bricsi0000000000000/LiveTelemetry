using System.Collections.Generic;

namespace DataModel
{
    public class PageTemplate
    {
        public PageTemplate(int id, string name)
        {
            Id = id;
            Name = name.Trim();
            SensorNames = new List<string>();
            GroupNames = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> SensorNames { get; private set; }
        public List<string> GroupNames { get; private set; }

        public void AddSensorName(string sensorName)
        {
            SensorNames.Add(sensorName);
        }

        public void AddGroupName(string groupName)
        {
            GroupNames.Add(groupName);
        }
    }
}
