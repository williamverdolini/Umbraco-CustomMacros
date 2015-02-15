using System.Globalization;

namespace CustomMacros.Areas.Framework.Services.Page
{
    public interface IPageService
    {
        void SetCultureInfo(string pageAbsolutePath, string language);
        void SetViewStyle(string pageAbsolutePath, string viewStyle);
        CultureInfo GetCultureInfo(string pageAbsolutePath);
        //TODO: Da eliminare. Baco di design: non esiste una CurrentCultureInfo, ma l'ultima impostata.
        CultureInfo GetLastCultureInfo();
        string GetViewStyle(string pageAbsolutePath);
    }
}
