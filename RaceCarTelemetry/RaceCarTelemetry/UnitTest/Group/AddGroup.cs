using NUnit.Framework;
using System.IO;
using UI.Managers;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase(0, "group0", "group0")]
        [TestCase(1, " group1", "group1")]
        [TestCase(2, "group2 ", "group2")]
        [TestCase(3, " group3 ", "group3")]
        [TestCase(4, " group_3 ", "group_3")]
        public void AddGroup_TestGood(int id, string name, string expectedName)
        {
            GroupManager.AddGroup(new DataModel.Group(id, name), out string errorMessage);

            Assert.IsTrue(string.IsNullOrEmpty(errorMessage)); // if there is a group with the same id, the errorMessage will not be empty
            Assert.IsNotNull(GroupManager.GetGroup(id));
            Assert.AreEqual(expectedName, GroupManager.GetGroup(id).Name);
        }
    }
}