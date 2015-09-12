using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;

namespace DigiGuard
{
    /// <summary>
    /// Summary description for DashboardService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DashboardService : System.Web.Services.WebService
    {

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string GetCategories()
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                var data = entities.DimCategories.ToList();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        //Clustered by URL
        public string GetReports()
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                var data = entities.FactReports.Where(x => x.StatusID != 2).ToList();
                List<Cluster> list = new List<Cluster>();
                foreach (FactReport report in data)
                {
                    Cluster c = list.FirstOrDefault(x => String.Equals(x.URL, report.URL));
                    if (c == null)
                    {
                        c = new Cluster() { URL = report.URL, status = report.DimStatu };
                        if (report.TimeStamp > c.Time)
                            c.Time = report.TimeStamp;
                        list.Add(c);
                    }
                    c.Amount++;
                    c.Reports.Add(report);

                }
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(list, settings);
            }


        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string GetSubReports(string url)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                var data = entities.FactReports.Where(x => x.URL == url);
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string AuthenticateUser(string username, string password)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                var data = entities.DimUsers.FirstOrDefault(x => x.UserName == username && x.Password == password);
                if (data != null)
                {
                    return JsonConvert.SerializeObject(true);
                }
                return JsonConvert.SerializeObject(false);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public void changeStatus(Cluster sCluster, string user)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                foreach (FactReport report in sCluster.Reports)
                {
                    var data = entities.FactReports.FirstOrDefault(x => x.ReportID == report.ReportID);
                    if (data != null)
                    {
                        if (data.StatusID < entities.DimStatus.Max(x => x.StatusID))
                        {
                            string oldStat = data.DimStatu.StatusName;
                            data.StatusID++;
                            Change c = new Change()
                            {
                                ReportID = data.ReportID,
                                UserName = user,
                                Data = "Status Changed From " + oldStat + " To " + entities.DimStatus.First(x=>x.StatusID==data.StatusID+1).StatusName,
                                Time = DateTime.Now
                            };
                            entities.Changes.Add(c);
                        }
                        entities.SaveChanges();
                    }
                }
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string GetUserActions(string user)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                var data = entities.Changes.Where(x => x.UserName == user).ToList();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string GetStatistics()
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                string stat = "{";
                var data = entities.FactReports.ToList();
                stat += "\"Reports\":{\"NumberofPending\":" + data.Count(x => x.StatusID == 0) + "," +
                        "\"NumberofInProgress\":" + data.Count(x => x.StatusID == 1) + "," +
                        "\"NumberofDone\":" + data.Count(x => x.StatusID == 2) + "}";
                stat += ",\"Categories\":[";
                foreach (DimCategory category in entities.DimCategories.ToList())
                {
                    stat+="{\"Name\":\""+category.Category+"\",\"AmountOfReports\":"+category.FactReports.Count+"},";
                }
                stat = stat.Substring(0,stat.Length - 1) + "]";
                return stat+"}";
            }
        }
    }

    public class Cluster
    {
        public string URL;
        public int Amount = 0;
        public DimStatu status;
        public List<FactReport> Reports = new List<FactReport>();
        public DateTime Time = new DateTime(1900,1,1);
    }
}
