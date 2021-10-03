using NUnit.Framework;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase(0, "proba_group", 0, "proba_group")]
        [TestCase(1, "proba group", 1, "proba group")]
        [TestCase(2, " proba group", 2, "proba group")]
        [TestCase(3, "proba group ", 3, "proba group")]
        [TestCase(4, " proba group ", 4, "proba group")]
        public void CreateGroup_TestGood(int id, string groupName, int expectedId, string expectedGroupName)
        {
            DataModel.Group group = new DataModel.Group(id, groupName);
            Assert.AreEqual(group.Name, expectedGroupName);
            Assert.AreEqual(group.Id, expectedId);
        }

        [Test]
        [TestCase("proba_group", "proba group")]
        [TestCase("proba group", "proba_group")]
        [TestCase(" proba group", " proba group")]
        [TestCase("proba group ", "proba group ")]
        [TestCase(" proba group ", " proba group ")]
        public void CreateGroup_TestBad(string groupName, string expectedGroupName)
        {
            DataModel.Group group = new DataModel.Group(0, groupName);
            Assert.AreNotEqual(group.Name, expectedGroupName);
        }
    }
}
