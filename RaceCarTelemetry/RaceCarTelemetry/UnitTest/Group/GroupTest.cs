using NUnit.Framework;
using System.IO;

namespace UnitTest.Group
{
    [TestFixture]
    public partial class AddGroup
    {
        private const string GOOD_GROUPS_PATH = "../../../Group/good_groups/";
        private const string WRONG_GROUPS_PATH = "../../../Group/wrong_groups/";
        private const string TEST_RESULT_LOG_PATH = "../../../TestResults/test_results.log";


        [TearDown]
        public void TearDownAddGroup()
        {
            using StreamWriter writer = new StreamWriter(TEST_RESULT_LOG_PATH, append: true);
            writer.WriteLine($"{TestContext.CurrentContext.Result.Outcome}\t{TestContext.CurrentContext.Test.FullName}");
        }
    }
}
