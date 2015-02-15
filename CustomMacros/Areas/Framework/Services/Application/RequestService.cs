using System.Collections.Specialized;
using System.Web;

namespace CustomMacros.Areas.Framework.Services.Application
{
    public class RequestService : IRequestService
    {
        private HybridDictionary hdRequest = new HybridDictionary();

        public string GetRequest(string RequestKey)
        {
            string myReqVal = string.Empty;
            if (HttpContext.Current.Request[RequestKey] != null)
                myReqVal = HttpContext.Current.Request[RequestKey];
            else if (hdRequest[RequestKey] != null)
                myReqVal = hdRequest[RequestKey].ToString();
            return myReqVal;
        }

    }
}