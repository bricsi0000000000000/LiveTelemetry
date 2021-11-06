using System.Collections.Generic;

namespace DataModel
{
    public class PageTemplateChart
    {
        public string Name { get; set; }
        public int Index { get; set; }
    }

    public class PageTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> SensorNames { get; private set; }
        public List<string> GroupNames { get; private set; }
        public List<PageTemplateChart> Charts { get; private set; }

        public PageTemplate(int id, string name)
        {
            Id = id;
            Name = name.Trim();
            SensorNames = new List<string>();
            GroupNames = new List<string>();
            Charts = new List<PageTemplateChart>();
        }

        public void AddSensorName(string sensorName, out string errorMessage)
        {
            if (!IsSensorNameExists(sensorName))
            {
                SensorNames.Add(sensorName);
                errorMessage = "";
            }
            else
            {
                errorMessage = $"{sensorName} is already exists";
            }
        }

        public void AddGroup(string groupName)
        {
            if (!IsGroupExists(groupName))
            {
                GroupNames.Add(groupName);
            }
        }

        public void RemoveSensorName(string sensorName, out string errorMessage)
        {
            if (IsSensorNameExists(sensorName))
            {
                SensorNames.RemoveAt(SensorNames.FindIndex(x => x == sensorName));
                errorMessage = "";
            }
            else
            {
                errorMessage = $"{sensorName} is does not exists";
            }
        }

        public void RemoveGroup(string groupName)
        {
            if (IsGroupExists(groupName))
            {
                GroupNames.RemoveAt(GroupNames.FindIndex(x => x == groupName));
            }
        }

        public bool IsGroupExists(string name)
        {
            return GroupNames.Find(x => x == name) != null;
        }

        public bool IsSensorNameExists(string name)
        {
            return SensorNames.Find(x => x == name) != null;
        }

        public PageTemplateChart GetChart(string chartName)
        {
            return Charts.Find(x => x.Name == chartName);
        }

        public void RemoveChart(string chartName)
        {
            if (GetChart(chartName) != null)
            {
                int index = Charts.FindIndex(x => x.Name == chartName);
                Charts.RemoveAt(index);
            }
        }
    }
}
