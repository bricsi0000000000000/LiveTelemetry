using NUnit.Framework;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        [TestCase("attribute", "#fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase(" attribute", "#fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase("attribute", " #fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase(" attribute", " #fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase("attribute ", " #fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase(" attribute ", " #fc0505", 1, "attribute", "#fc0505", 1)]
        [TestCase("attribute", "#fc0505 ", 1, "attribute", "#fc0505", 1)]
        [TestCase("attribute", " #fc0505 ", 1, "attribute", "#fc0505", 1)]
        [TestCase(" attribute ", " #fc0505 ", 1, "attribute", "#fc0505", 1)]
        public void AddAttributeToGroup_Test(string name, string colorText, int lineWidth, string expectedName, string expectedColorCode, int expectedintLineWidth)
        {
            DataModel.Group group = new DataModel.Group(0, "group");
            group.AddAttribute(name, colorText, lineWidth);
            Assert.AreEqual(group.Attributes[0].Name, expectedName);
            Assert.AreEqual(group.Attributes[0].ColorCode, expectedColorCode);
            Assert.AreEqual(group.Attributes[0].LineWidth, expectedintLineWidth);
        }
    }
}
