using NUnit.Framework;
using UI.Managers;

namespace UnitTest.PageTemplate
{
    [TestFixture]
    public partial class PageTemplateTest
    {
        [Test]
        [TestCase(0, "template0", "template0")]
        [TestCase(1, " template1", "template1")]
        [TestCase(2, "template2 ", "template2")]
        [TestCase(3, " template3 ", "template3")]
        [TestCase(4, " template_3 ", "template_3")]
        public void AddPageTemplate_TestGood(int id, string name, string expectedName)
        {
            PageTemplateManager.AddPageTemplate(new DataModel.PageTemplate(id, name), out string errorMessage);

            Assert.IsTrue(string.IsNullOrEmpty(errorMessage)); // if there is a page template with the same id, the errorMessage will not be empty
            Assert.IsNotNull(PageTemplateManager.GetPageTemplate(id));
            Assert.AreEqual(expectedName, PageTemplateManager.GetPageTemplate(id).Name);
        }
    }
}
