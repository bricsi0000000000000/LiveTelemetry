using BusinessLogic;
using NUnit.Framework;
using System.Collections.Generic;
using UI.Managers;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase("Gearbox")]
        [TestCase("EngineBase")]
        public void GetGroup_TestGood(string name)
        {
            List<DataModel.Group> groups = new GroupBusinessLogic().LoadGroups(GOOD_GROUPS_PATH + "groups.json", out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
            Assert.IsNotNull(groups.Find(x => x.Name.Equals(name)));
        }
    }
}
