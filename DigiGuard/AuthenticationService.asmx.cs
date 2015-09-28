using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace DigiGuard
{
    /// <summary>
    /// Summary description for Analizer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AuthenticationService : System.Web.Services.WebService
    {
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string AuthenticateUser(string username, string password)
        {
            try
            {
                using (DGGuardEntities entities = new DGGuardEntities())
                {
                    var data = entities.DimUsers.FirstOrDefault(x => x.UserName == username && x.Password == password);
                    if (data != null)
                        return JsonConvert.SerializeObject(true);
                    return JsonConvert.SerializeObject(false);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance.Log(LogType.Error, "Authentication Failed: "+ ex.Message);
                return JsonConvert.SerializeObject(false);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string RegisterUser(string username, string password, string permissions)
        {
            try
            {
                using (DGGuardEntities entities = new DGGuardEntities())
                {
                    var data = entities.DimUsers.FirstOrDefault(x => x.UserName == username);
                        //check for exisiting username
                    if (data != null)
                        return JsonConvert.SerializeObject(false);
                    string s = StringCipher.CreateHash(password);
                    DimUser u = new DimUser
                    {
                        UserName = username,
                        Password = s.Split(':')[1],
                        Salt = s.Split(':')[0],
                        Permisssions = permissions
                    };
                    entities.DimUsers.Add(u);
                    entities.SaveChanges();
                    Logger.GetInstance.Log(LogType.Info, "New User Registerd");
                    return JsonConvert.SerializeObject(true);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance.Log(LogType.Error, "Registration Failed: "+ ex.Message);
                return JsonConvert.SerializeObject(false);
            }
        }
    }
}
