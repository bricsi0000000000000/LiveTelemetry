using NUnit.Framework;
using System.IO;

namespace UnitTest.PageTemplate
{
    [TestFixture]
    public partial class PageTemplateTest
    {
        private const string GOOD_PAGE_TEMPLATES_PATH = "../../../PageTemplate/good_page_templates/";
        private const string WRONG_PAGE_TEMPLATES_PATH = "../../../PageTemplate/wrong_page_templates/";
        private const string TEST_RESULT_LOG_PATH = "../../../test_results.txt";


        [TearDown]
        public void TearDownAddGroup()
        {
            using StreamWriter writer = new StreamWriter(TEST_RESULT_LOG_PATH, append: true);
            writer.WriteLine($"{TestContext.CurrentContext.Result.Outcome}\t{TestContext.CurrentContext.Test.FullName}");
        }
    }
}
