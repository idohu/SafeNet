using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Image = System.Drawing.Image;

namespace DigiGuard
{
    /// <summary>
    /// Summary description for ClientService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ClientService : System.Web.Services.WebService
    {
        private string genPath = "";

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string PostReport(string url, string img, string dom = "", int category = -1,
            string name = "", string lName = "", string phone = "", string email = "",
            string description = "", string location = "")
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                genPath = Server.MapPath("/UploadImage/");;
                string fileName = (DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".png").Replace(" ", "");
                try
                {
                    byte[] data = Convert.FromBase64String(img);
                    var fs =
                        new BinaryWriter(new FileStream(Path.Combine(genPath,fileName), FileMode.Append,
                            FileAccess.Write));
                    fs.Write(data);
                    fs.Close();
                }
                catch (Exception ex)
                {

                }
                FactReport f = new FactReport
                    {
                        URL = url,
                        TimeStamp = DateTime.Now,
                        Location = location,
                        Description = description,
                        Name = name,
                        LastName = lName,
                        Email = email,
                        Phone = phone,
                        ScreenShot = genPath + fileName,
                        DimCategory = entities.DimCategories.FirstOrDefault(x=>x.CategoryId==category),
                        DimStatu = entities.DimStatus.FirstOrDefault(x=>x.StatusID == 0)
                    };
                entities.FactReports.Add(f);
                entities.SaveChanges();
            }
            return "Success";
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string PostReportImage(string img)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                string fileName = DateTime.Now + ".png";
                byte[] data = Convert.FromBase64String(img);
                var fs =
                    new BinaryWriter(new FileStream(genPath + fileName, FileMode.Append,
                        FileAccess.Write));
                fs.Write(data);
                fs.Close();
            }
            return "sucess";
        }
    }
}
