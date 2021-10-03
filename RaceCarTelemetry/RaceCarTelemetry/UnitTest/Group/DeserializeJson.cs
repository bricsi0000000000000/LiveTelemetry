using BusinessLogic;
using NUnit.Framework;
using UI.Managers;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase(GOOD_GROUPS_PATH + "groups.json")]
        public void DeserializeJson_AllGood(string fileName)
        {
            _ = new GroupBusinessLogic().LoadGroups(fileName, out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        [TestCase(GOOD_GROUPS_PATH + "grouops.json")]
        public void DeserializeJson_FileNotFound(string fileName)
        {
            _ = new GroupBusinessLogic().LoadGroups(fileName, out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        [TestCase(WRONG_GROUPS_PATH + "missing_curly_bracket.json")]
        [TestCase(WRONG_GROUPS_PATH + "empty_name.json")]
        [TestCase(WRONG_GROUPS_PATH + "null_name.json")]
        [TestCase(WRONG_GROUPS_PATH + "null_customizable.json")]
        [TestCase(WRONG_GROUPS_PATH + "null_driverless.json")]
        [TestCase(WRONG_GROUPS_PATH + "null_attribute_name.json")]
        [TestCase(WRONG_GROUPS_PATH + "null_attribute_color.json")]
        [TestCase(WRONG_GROUPS_PATH + "empty.json")]
        public void DeserializeJson_MissingCurlyBrackets(string fileName)
        {
            _ = new GroupBusinessLogic().LoadGroups(fileName, out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }
    }
}
