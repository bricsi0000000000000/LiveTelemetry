using System.Collections.Generic;

namespace DataModel.Live
{
    public class LiveConfiguration
    {
        /// <summary>
        /// In milliseconds
        /// </summary>
        public int WaitBetweenCollectData { get; set; }
        public bool IsHttps { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// In minutes
        /// </summary>
        public int TimeOut { get; set; }
        public string Address => $"{(IsHttps ? "https" : "http")}://{Url}:{Port}/";

        public List<LiveSection> Sections { get; set; }

        public string GetApiCall(string sectionName)
        {
            return Sections.Find(x => x.Name.Equals(sectionName)).Value;
        }
    }
}
