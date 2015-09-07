using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;

namespace DigiGuard
{
    /// <summary>
    /// Summary description for PrioritizedDigiGuard
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PrioritizedDigiGuard : System.Web.Services.WebService
    {

        [WebMethod]
        [WebInvoke(Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
ResponseFormat = WebMessageFormat.Json)]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
