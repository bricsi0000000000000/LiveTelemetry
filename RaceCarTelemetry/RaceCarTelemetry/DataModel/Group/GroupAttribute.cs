namespace DataModel
{
    public class GroupAttribute
    {
        public int sensorId;

        public int Id { get; private set; }
        public int GroupId { get; private set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public int LineWidth { get; set; }
        public bool IsActive { get; set; }

        public GroupAttribute(int id, int groupId, string name, string colorCode, int lineWidth)
        {
            Id = id;
            GroupId = groupId;
            Name = name.Trim();
            ColorCode = colorCode.Trim();
            LineWidth = lineWidth;
            IsActive = true;
        }
    }
}
