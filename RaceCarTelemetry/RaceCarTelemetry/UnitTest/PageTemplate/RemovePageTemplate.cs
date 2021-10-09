using NUnit.Framework;
using UI.Managers;

namespace UnitTest.PageTemplate
{
    [TestFixture]
    public partial class PageTemplateTest
    {
        [Test]
        [TestCase(10, "template0")]
        [TestCase(11, " template1")]
        [TestCase(12, "template2 ")]
        [TestCase(13, " template3 ")]
        [TestCase(14, " template_3 ")]
        public void RemovePageTemplate(int id, string name)
        {
            PageTemplateManager.AddPageTemplate(new DataModel.PageTemplate(id, name), out string errorMessage);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            PageTemplateManager.RemovePageTemplate(id, out errorMessage);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));

            Assert.IsNull(PageTemplateManager.GetPageTemplate(id));
        }
    }
}
