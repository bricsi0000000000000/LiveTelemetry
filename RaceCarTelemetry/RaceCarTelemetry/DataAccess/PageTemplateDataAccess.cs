using DataModel;
using DataModel.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class PageTemplateDataAccess : BaseDataAccess
    {
        public List<PageTemplate> LoadPageTemplates(string fileName, out string errorMessage, ref int lastPageTemplateId)
        {
            List<PageTemplate> templates = new List<PageTemplate>();

            if (CheckFile(fileName, out errorMessage))
            {
                using StreamReader reader = new StreamReader(fileName);

                try
                {
                    dynamic templatesJson = JsonConvert.DeserializeObject(reader.ReadToEnd());

                    for (int templateIndex = 0; templateIndex < templatesJson.Count; templateIndex++)
                    {
                        if (templatesJson[templateIndex].Name == null)
                        {
                            errorMessage = "Can't add page template, because 'name' is null!";
                            continue;
                        }

                        if (templatesJson[templateIndex].Name.ToString().Equals(string.Empty))
                        {
                            errorMessage = "Can't add page template, because 'name' is empty!";
                            continue;
                        }

                        PageTemplate template = new PageTemplate(lastPageTemplateId++, templatesJson[templateIndex].Name.ToString());

                        if (templatesJson[templateIndex].SensorNames != null)
                        {
                            for (int sensorNameIndex = 0; sensorNameIndex < templatesJson[templateIndex].SensorNames.Count; sensorNameIndex++)
                            {
                                template.SensorNames.Add(templatesJson[templateIndex].SensorNames[sensorNameIndex].ToString());
                            }
                        }

                        if (templatesJson[templateIndex].GroupNames != null)
                        {
                            for (int groupNameIndex = 0; groupNameIndex < templatesJson[templateIndex].GroupNames.Count; groupNameIndex++)
                            {
                                template.GroupNames.Add(templatesJson[templateIndex].GroupNames[groupNameIndex].ToString());
                            }
                        }

                        if (templatesJson[templateIndex].Charts != null)
                        {
                            for (int chartIndex = 0; chartIndex < templatesJson[templateIndex].Charts.Count; chartIndex++)
                            {
                                template.Charts.Add(new PageTemplateChart { Name = templatesJson[templateIndex].Charts[chartIndex].Name, Index = templatesJson[templateIndex].Charts[chartIndex].Index });
                            }
                        }

                        templates.Add(template);
                    }
                }
                catch (JsonReaderException)
                {
                    errorMessage = $"There was a problem reading '{TextManager.PAGE_TEMPLATES_FILE}'";
                }
            }

            return templates;
        }

        public void SavePageTemplates(List<PageTemplate> templates, out string errorMessage)
        {
            if (IsFileExists(FilePathManager.PageTemplateFilePath, out errorMessage))
            {
                using StreamWriter writer = new StreamWriter(FilePathManager.PageTemplateFilePath);

                JsonSerializer serializer = new JsonSerializer();
                try
                {
                    serializer.Serialize(writer, templates);
                }
                catch (Exception)
                {
                    errorMessage = "Can't save file!";
                }
            }
        }
    }
}
