using BusinessLogic;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UI.Managers;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        [Test]
        public void InitGroup_TestGood()
        {
            List<DataModel.Group> groups = new GroupBusinessLogic().LoadGroups(GOOD_GROUPS_PATH + "groups.json", out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            DataModel.Group group1 = new DataModel.Group(groups.Last().Id + 1, "Gearbox");
            group1.AddAttribute("rev", "#FFFC0505", 1);
            group1.AddAttribute("gear", "#FFFC7C05", 1);
            group1.AddAttribute("upInput", "#FFFCE705", 1);
            group1.AddAttribute("upRqe", "#FF80FC05", 1);
            group1.AddAttribute("upGoing", "#FF05FCF4", 1);
            group1.AddAttribute("shiftCnt", "#FF8005FC", 1);
            group1.AddAttribute("neutral_In", "#FFFC0505", 1);

            DataModel.Group group2 = groups.Find(x => x.Name.Equals("Gearbox"));
            Assert.AreEqual(group1.Name, group2.Name);
            for (int i = 0; i < group1.Attributes.Count; i++)
            {
                Assert.AreEqual(group1.Attributes[i].Name, group2.Attributes[i].Name);
                Assert.AreEqual(group1.Attributes[i].ColorCode, group2.Attributes[i].ColorCode);
            }
        }

        [Test]
        public void InitGroup_TestBad()
        {
            List<DataModel.Group> groups = new GroupBusinessLogic().LoadGroups(GOOD_GROUPS_PATH + "groups.json", out string errorMessage, ref GroupManager.LastGroupId, out GroupManager.TemporaryGroupIndex);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            DataModel.Group group1 = new DataModel.Group(groups.Last().Id + 1, "Gearbox");
            group1.AddAttribute("rer", "#FFFC505", 1);
            group1.AddAttribute("ger", "#FFFC705", 1);
            group1.AddAttribute("upnput", "#FFFE705", 1);
            group1.AddAttribute("upRe", "#FF80FC5", 1);
            group1.AddAttribute("upoing", "#FF05FF4", 1);
            group1.AddAttribute("shftCnt", "#FF800FC", 1);
            group1.AddAttribute("netral_In", "#FFFC005", 1);

            DataModel.Group group2 = groups.Find(x => x.Name.Equals("Gearbox"));
            Assert.AreEqual(group1.Name, group2.Name);
            for (int i = 0; i < group1.Attributes.Count; i++)
            {
                Assert.AreNotEqual(group1.Attributes[i].Name, group2.Attributes[i].Name);
                Assert.AreNotEqual(group1.Attributes[i].ColorCode, group2.Attributes[i].ColorCode);
            }
        }
    }
}
