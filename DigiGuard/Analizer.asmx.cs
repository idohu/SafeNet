using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace DigiGuard
{
    /// <summary>
    /// Summary description for Analizer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Analizer : System.Web.Services.WebService
    {

        [WebMethod]
        public void AnalyzePage(string source)
        {
            Thread t = new Thread(()=>Analyze(source));
            t.Start();
        }

        private void Analyze(string page)
        {
            string[] sSource = page.Split(' ', '\r', '\n', '>', '<');
            foreach (string s in sSource)
            {

            }
        }
    }
}
