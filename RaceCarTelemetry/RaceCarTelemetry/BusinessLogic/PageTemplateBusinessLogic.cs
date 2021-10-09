using DataAccess;
using DataModel;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class PageTemplateBusinessLogic
    {
        public List<PageTemplate> LoadPageTemplate(string fileName, out string errorMessage, ref int lastPageTemplateId)
        {
            return new PageTemplateDataAccess().LoadPageTemplates(fileName, out errorMessage, ref lastPageTemplateId);
        }

        public void SavePageTemplates(List<PageTemplate> pageTemplates, out string errorMessage)
        {
            new PageTemplateDataAccess().SavePageTemplates(pageTemplates, out errorMessage);
        }
    }
}
