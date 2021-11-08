using BusinessLogic;
using DataModel;
using DataModel.Constants;
using System.Collections.Generic;

namespace UI.Managers
{
    public static class PageTemplateManager
    {
        public static int LastTemplateId = 0;

        private static readonly PageTemplateBusinessLogic pageTemplateBusinessLogic = new PageTemplateBusinessLogic();

        public static List<PageTemplate> PageTemplates { get; private set; } = new List<PageTemplate>();

        public static void LoadPageTemplates(out string errorMessage)
        {
            PageTemplates = pageTemplateBusinessLogic.LoadPageTemplate(FilePathManager.PageTemplateFilePath, out errorMessage, ref LastTemplateId);
        }

        public static PageTemplate GetPageTemplate(int id)
        {
            return PageTemplates.Find(x => x.Id == id);
        }

        public static PageTemplate GetPageTemplate(string name)
        {
            return PageTemplates.Find(x => x.Name == name);
        }

        private static bool IsPageTemplateExists(int id)
        {
            return GetPageTemplate(id) != null;
        }

        public static bool IsPageTemplateExists(string name)
        {
            return PageTemplates.Find(x => x.Name.Equals(name)) != null;
        }

        public static void AddPageTemplate(PageTemplate template, out string errorMessage)
        {
            if (IsPageTemplateExists(template.Id))
            {
                errorMessage = $"{template.Name} already exists";
                return;
            }

            PageTemplates.Add(template);

            SavePageTemplates(out errorMessage);
        }

        public static void RemovePageTemplate(int id, out string errorMessage)
        {
            PageTemplates.RemoveAt(PageTemplates.FindIndex(x => x.Id == id));

            SavePageTemplates(out errorMessage);
        }

        public static void ChangePageTemplateName(int id, string name, out string errorMessage)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name can not be empty";
                return;
            }

            if (IsPageTemplateExists(name))
            {
                errorMessage = $"{name} already exists";
                return;
            }

            PageTemplate template = GetPageTemplate(id);
            if (!template.Name.Equals(name))
            {
                template.Name = name;
                SavePageTemplates(out errorMessage);
            }
            else
            {
                errorMessage = string.Empty;
            }
        }

        public static void SavePageTemplates(out string errorMessage)
        {
            pageTemplateBusinessLogic.SavePageTemplates(PageTemplates, out errorMessage);
        }

        public static void DeleteAllChartFromTemplates(out string errorMessage)
        {
            foreach (PageTemplate template in PageTemplates)
            {
                template.Charts.Clear();
            }

            SavePageTemplates(out errorMessage);
        }
    }
}
