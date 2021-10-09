using BusinessLogic;
using NUnit.Framework;
using UI.Managers;

namespace UnitTest.PageTemplate
{
    [TestFixture]
    public partial class PageTemplateTest
    {
        [Test]
        [TestCase(GOOD_PAGE_TEMPLATES_PATH + "page_templates.json")]
        public void DeserializeJson_AllGood(string fileName)
        {
            _ = new PageTemplateBusinessLogic().LoadPageTemplate(fileName, out string errorMessage, ref PageTemplateManager.LastTemplateId);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        [TestCase(GOOD_PAGE_TEMPLATES_PATH + "page_temmmmplates.json")]
        public void DeserializeJson_FileNotFound(string fileName)
        {
            _ = new PageTemplateBusinessLogic().LoadPageTemplate(fileName, out string errorMessage, ref PageTemplateManager.LastTemplateId);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        [TestCase(WRONG_PAGE_TEMPLATES_PATH + "missing_curly_bracket.json")]
        [TestCase(WRONG_PAGE_TEMPLATES_PATH + "null_name.json")]
        [TestCase(WRONG_PAGE_TEMPLATES_PATH + "empty_name.json")]
        [TestCase(WRONG_PAGE_TEMPLATES_PATH + "empty.json")]
        public void DeserializeJson_Missing(string fileName)
        {
            _ = new PageTemplateBusinessLogic().LoadPageTemplate(fileName, out string errorMessage, ref PageTemplateManager.LastTemplateId);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }
    }
}
