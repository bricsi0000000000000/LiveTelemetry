using BusinessLogic;
using DataModel;
using DataModel.Constants;
using System.Collections.Generic;

namespace UI.Managers
{
    public static class GroupManager
    {
        public static int LastGroupId = 0;
        public static int TemporaryGroupIndex = 0;

        private static readonly GroupBusinessLogic groupBusinessLogic = new GroupBusinessLogic();

        public static List<Group> Groups { get; private set; } = new List<Group>();

        public static void LoadGroups(out string errorMessage)
        {
            Groups = groupBusinessLogic.LoadGroups(FilePathManager.GroupFilePath, out errorMessage, ref LastGroupId, out TemporaryGroupIndex);
        }

        public static Group GetGroup(int id)
        {
            return Groups.Find(x => x.Id == id);
        }

        private static bool IsGroupExists(int groupId)
        {
            return GetGroup(groupId) != null;
        }

        public static bool IsGroupExists(string name)
        {
            return Groups.Find(x => x.Name.Equals(name)) != null;
        }

        public static void AddGroup(Group group, out string errorMessage)
        {
            if (IsGroupExists(group.Id))
            {
                errorMessage = $"{group.Name} already exists";
                return;
            }

            Groups.Add(group);

            SaveGroups(out errorMessage);
        }

        public static void AddAttributeToGroup(int groupId, GroupAttribute attribute, out string errorMessage)
        {
            GetGroup(groupId).AddAttribute(attribute, out errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                SaveGroups(out errorMessage);
            }
        }

        public static void RemoveGroup(int groupId, out string errorMessage)
        {
            Groups.RemoveAt(Groups.FindIndex(x => x.Id == groupId));

            SaveGroups(out errorMessage);
        }

        public static void RemoveAttributeFromGroup(int groupId, int attributeId, out string errorMessage)
        {
            GetGroup(groupId).RemoveAttribute(attributeId);

            SaveGroups(out errorMessage);
        }

        public static void ChangeGroupName(int groupId, string name, out string errorMessage)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name can not be empty";
                return;
            }

            if (IsGroupExists(name))
            {
                errorMessage = $"{name} already exists";
                return;
            }

            Group group = GetGroup(groupId);
            if (!group.Name.Equals(name))
            {
                group.Name = name;
                SaveGroups(out errorMessage);
            }
            else
            {
                errorMessage = string.Empty;
            }
        }

        public static void ChangeAttributeNameInGroup(int groupId, int attributeId, string name, out string errorMessage)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name can not be empty";
                return;
            }

            if (GetGroup(groupId).IsAttributeExists(name))
            {
                errorMessage = $"{name} already exists";
                return;
            }

            GroupAttribute attribute = GetGroup(groupId).GetAttribute(attributeId);
            if (!attribute.Name.Equals(name))
            {
                attribute.Name = name;
                SaveGroups(out errorMessage);
            }
            else
            {
                errorMessage = string.Empty;
            }
        }

        public static void ChangeAttributeLineWidthInGroup(int groupId, int attributeId, int lineWidth, out string errorMessage)
        {
            GroupAttribute attribute = GetGroup(groupId).GetAttribute(attributeId);
            if (attribute.LineWidth != lineWidth)
            {
                attribute.LineWidth = lineWidth;
                SaveGroups(out errorMessage);
            }
            else
            {
                errorMessage = string.Empty;
            }
        }

        public static void ChangeAttributeColorInGroup(int groupId, int attributeId, string colorCode, out string errorMessage)
        {
            GroupAttribute attribute = GetGroup(groupId).GetAttribute(attributeId);
            if (!attribute.ColorCode.Equals(colorCode))
            {
                attribute.ColorCode = colorCode;
                SaveGroups(out errorMessage);
            }
            else
            {
                errorMessage = string.Empty;
            }
        }

        private static void SaveGroups(out string errorMessage)
        {
            groupBusinessLogic.SaveGroups(Groups, out errorMessage);
        }
    }
}
