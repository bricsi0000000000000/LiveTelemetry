using NUnit.Framework;
using UI.Managers;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase(10, "group00")]
        [TestCase(11, "group01")]
        [TestCase(12, "group02")]
        [TestCase(13, "group03")]
        [TestCase(14, "group04")]
        public void RemoveGroup_TestGood(int id, string name)
        {
            GroupManager.AddGroup(new DataModel.Group(id, name), out string errorMessage);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            GroupManager.RemoveGroup(id, out errorMessage);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            Assert.IsNull(GroupManager.GetGroup(id));
        }
    }
}
