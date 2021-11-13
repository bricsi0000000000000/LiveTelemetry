using System.Collections.Generic;

namespace DataModel
{
    public class Group
    {
        public int Id { get; }
        public string Name { get; set; }
        public List<GroupAttribute> Attributes { get; }

        public int LastAttributeId;

        public Group(int id, string name)
        {
            Id = id;
            Name = name.Trim();
            Attributes = new List<GroupAttribute>();
            LastAttributeId = 0;
        }

        public void AddAttribute(string name, string color, int lineWidth)
        {
            if (!name.Equals(string.Empty))
            {
                Attributes.Add(new GroupAttribute(LastAttributeId++, Id, name, color, lineWidth));
            }
        }

        public void AddAttribute(GroupAttribute attribute, out string errorMessage)
        {
            if (IsAttributeExists(attribute.Id))
            {
                errorMessage = $"{attribute.Name} already exists";
                return;
            }

            Attributes.Add(attribute);
            errorMessage = string.Empty;
        }

        public bool IsAttributeExists(int attributeId)
        {
            return GetAttribute(attributeId) != null;
        }

        public bool IsAttributeExists(string name)
        {
            return GetAttribute(name) != null;
        }

        public void RemoveAttribute(int id)
        {
            Attributes.Remove(GetAttribute(id));
        }

        public GroupAttribute GetAttribute(int id)
        {
            return Attributes.Find(x => x.Id == id);
        }

        public GroupAttribute GetAttribute(string name)
        {
            return Attributes.Find(x => x.Name.Equals(name));
        }
    }
}
