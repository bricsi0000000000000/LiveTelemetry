using DataAccess;
using DataModel;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class GroupBusinessLogic
    {
        public List<Group> LoadGroups(string fileName, out string errorMessage, ref int lastGroupId, out int temporaryGroupIndex)
        {
            return new GroupDataAccess().LoadGroups(fileName, out errorMessage, ref lastGroupId, out temporaryGroupIndex);
        }

        public void SaveGroups(List<Group> groups, out string errorMessage)
        {
            new GroupDataAccess().SaveGroups(groups, out errorMessage);
        }
    }
}
