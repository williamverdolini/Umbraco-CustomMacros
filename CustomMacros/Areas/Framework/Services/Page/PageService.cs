using System.Collections.Generic;
using System.Globalization;

namespace CustomMacros.Areas.Framework.Services.Page
{
    public class PageService : IPageService
    {
        private CultureInfo lastCulture;
        private IDictionary<string, CultureInfo> cultures;
        private IDictionary<string, string> viewStyles;

        public PageService()
        {
            viewStyles = new Dictionary<string, string>();
            cultures = new Dictionary<string, CultureInfo>();
        }

        //public void SetCultureInfo(string language)
        //{
        //    _currentCulture = string.IsNullOrEmpty(language) ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(language);
        //}

        public CultureInfo GetLastCultureInfo()
        {
            return lastCulture ?? CultureInfo.InvariantCulture;
        }

        public void SetCultureInfo(string pageAbsolutePath, string language)
        {
            if (!cultures.ContainsKey(pageAbsolutePath))
            {
                lastCulture = string.IsNullOrEmpty(language) ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(language);
                cultures.Add(pageAbsolutePath, lastCulture);
            }
        }

        public CultureInfo GetCultureInfo(string pageAbsolutePath)
        {
            return cultures.ContainsKey(pageAbsolutePath) ? cultures[pageAbsolutePath] : CultureInfo.InvariantCulture;
        }

        public void SetViewStyle(string pageAbsolutePath, string viewStyle)
        {
            if (!viewStyles.ContainsKey(pageAbsolutePath))
                viewStyles.Add(pageAbsolutePath, viewStyle);
        }

        public string GetViewStyle(string pageAbsolutePath)
        {
            return viewStyles[pageAbsolutePath];
        }
    }
}