using DataModel;
using DataModel.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace DataAccess
{
    public class GroupDataAccess : BaseDataAccess
    {
        public List<Group> LoadGroups(string fileName, out string errorMessage, ref int lastGroupId, out int temporaryGroupIndex)
        {
            temporaryGroupIndex = 0;
            List<Group> groups = new List<Group>();

            if (CheckFile(fileName, out errorMessage))
            {
                using StreamReader reader = new StreamReader(fileName);

                try
                {
                    dynamic groupsJson = JsonConvert.DeserializeObject(reader.ReadToEnd());

                    for (int groupIndex = 0; groupIndex < groupsJson.Count; groupIndex++)
                    {
                        if (groupsJson[groupIndex].Name == null)
                        {
                            errorMessage = "Can't add group, because 'name' is null!";
                            continue;
                        }

                        if (groupsJson[groupIndex].Name.ToString().Equals(string.Empty))
                        {
                            errorMessage = "Can't add group, because 'name' is empty!";
                            continue;
                        }

                        if (groupsJson[groupIndex].Attributes == null)
                        {
                            errorMessage = "Can't add group, because 'attributes' are null!";
                            continue;
                        }
                        else
                        {
                            Group group = new Group(lastGroupId++, groupsJson[groupIndex].Name.ToString());

                            for (int attributeIndex = 0; attributeIndex < groupsJson[groupIndex].Attributes.Count; attributeIndex++)
                            {
                                string attributeName = "";
                                string attributeColor = "";
                                int attributeLineWidth = 0;

                                if (groupsJson[groupIndex].Attributes[attributeIndex].Name == null)
                                {
                                    errorMessage = "Can't add attribute, because 'name' is null!";
                                    continue;
                                }

                                if (groupsJson[groupIndex].Attributes[attributeIndex].ColorCode == null)
                                {
                                    errorMessage = "Can't add attribute, because 'color' is null!";
                                    continue;
                                }

                                if (groupsJson[groupIndex].Attributes[attributeIndex].LineWidth == null)
                                {
                                    errorMessage = "Can't add attribute, because 'line width' is null!";
                                    continue;
                                }

                                attributeName = groupsJson[groupIndex].Attributes[attributeIndex].Name.ToString();
                                attributeColor = groupsJson[groupIndex].Attributes[attributeIndex].ColorCode.ToString();
                                attributeLineWidth = int.Parse(groupsJson[groupIndex].Attributes[attributeIndex].LineWidth.ToString());

                                if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(attributeColor))
                                {
                                    group.AddAttribute(attributeName, attributeColor, attributeLineWidth);
                                }
                                else
                                {
                                    errorMessage = "Can't add attribute, because 'name' or/and 'color' are empty!";
                                    continue;
                                }
                            }

                            groups.Add(group);
                        }

                    }

                    int tempGroupIndex = 0;
                    Group temporaryGroup = groups.Find(x => x.Name.Equals($"Temporary{tempGroupIndex}"));

                    while (temporaryGroup != null)
                    {
                        tempGroupIndex++;
                        temporaryGroup = groups.Find(x => x.Name.Equals($"Temporary{tempGroupIndex}"));
                    }

                    temporaryGroupIndex = tempGroupIndex;
                }
                catch (JsonReaderException)
                {
                    errorMessage = $"There was a problem reading '{TextManager.GROUPS_FILE}'";
                }
            }

            return groups;
        }

        public void SaveGroups(List<Group> groups, out string errorMessage)
        {
            if (CheckFile(FilePathManager.GroupFilePath, out errorMessage))
            {
                using StreamWriter writer = new StreamWriter(FilePathManager.GroupFilePath);

                JsonSerializer serializer = new JsonSerializer();
                try
                {
                    serializer.Serialize(writer, groups);
                }
                catch (Exception)
                {
                    errorMessage = "Can't save file!";
                }
            }
        }
    }
}
