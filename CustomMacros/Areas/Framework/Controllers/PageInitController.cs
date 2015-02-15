using System;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Commons;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Commons;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace CustomMacros.Areas.Framework.Controllers
{
    [PluginController("Framework")]
    public class PageInitController : SurfaceController
    {
        private readonly IPageService PageService;

        public PageInitController(IPageService pageService)
        {
            Contract.Requires<ArgumentNullException>(pageService != null, "pageService");
            PageService = pageService;
        }

        [ChildActionOnly]
        public void CultureInfo()
        {
            var _currentPage = getRootPage();
            var cultureProp = _currentPage.GetProperty(Constants.ID_LANGUAGE);
            if (cultureProp != null)
            {
                PageService.SetCultureInfo(Request.Url.AbsolutePath, cultureProp.DataValue as string);
            }
        }

        [ChildActionOnly]
        public void PageViewStyle(string viewStyle = "")
        {
            if (viewStyle != null)
            {
                PageService.SetViewStyle(Request.Url.AbsolutePath, viewStyle);
            }
        }

        private IPublishedContent getRootPage()
        {
            var _currentPage = CurrentPage;
            while (_currentPage != null && _currentPage.Parent != null)
                _currentPage = _currentPage.Parent;
            return _currentPage;
        }
    }
}
