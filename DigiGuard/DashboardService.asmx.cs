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
            List<Cluster> list = new List<Cluster>();
            try
            {
                using (DGGuardEntities entities = new DGGuardEntities())
                {
                    var data = entities.FactReports.Where(x => x.StatusID != 2).ToList();
                    foreach (FactReport report in data)
                    {
                        Cluster c = list.FirstOrDefault(x => String.Equals(x.URL, report.URL));
                        if (c == null)
                        {
                            c = new Cluster
                            {
                                URL = report.URL,
                                status = new DimStatuPrivate(report.DimStatu.StatusID, report.DimStatu.StatusName)
                            };
                            if (report.TimeStamp > c.Time)
                                c.Time = report.TimeStamp;
                            list.Add(c);
                        }
                        c.Amount++;
                        c.Reports.Add(new FaceReportPrivate(report));

                    }

                }
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(list, settings);

            }
            catch (Exception e)
            {
                Logger.GetInstance.Log(LogType.Error, "GetRerpots Failed: " + e.Message);
                return "";
            }

        }
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string GetAllReports()
        {
            try
            {
                using (DGGuardEntities entities = new DGGuardEntities())
                {
                    var data = entities.FactReports.ToList();
                    List<FaceReportPrivate> list = data.Select(factReport => new FaceReportPrivate(factReport)).ToList();
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    return JsonConvert.SerializeObject(list, settings);
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance.Log(LogType.Error, "GetAllReports Failed: " + ex.Message);
                return JsonConvert.SerializeObject(false);
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
        public void changeStatus(List<int> sCluster, string user)
        {
            using (DGGuardEntities entities = new DGGuardEntities())
            {
                foreach (int reportID in sCluster)
                {
                    var data = entities.FactReports.FirstOrDefault(x => x.ReportID == reportID);
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
                                Data = "Status Changed From " + oldStat + " To " +
                                entities.DimStatus.First(x => x.StatusID == data.StatusID).StatusName,
                                Time = DateTime.Now
                            };
                            entities.Changes.Add(c);
                        }
                        try
                        {
                            entities.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
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
                    stat += "{\"Name\":\"" + category.Category +
                        "\",\"AmountOfReports\":" + category.FactReports.Count + "},";
                }
                stat = stat.Substring(0, stat.Length - 1) + "]";
                return stat + "}";
            }
        }
    }

    public class Cluster
    {
        public string URL;
        public int Amount = 0;
        public DimStatuPrivate status;
        public List<FaceReportPrivate> Reports = new List<FaceReportPrivate>();
        public DateTime Time = new DateTime(1900, 1, 1);
    }



    public class FaceReportPrivate
    {
        public int ReportID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string URL { get; set; }
        public string ScreenShot { get; set; }
        public int? CategoryID { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int? StatusID { get; set; }

        public FaceReportPrivate(int Reportid, DateTime Time, string url, int? catId, string location,
            string name, string lname, string phone, string desc, string email, int? statusID)
        {
            ReportID = Reportid;
            TimeStamp = Time;
            URL = url;
            CategoryID = catId;
            Location = location;
            Name = name;
            LastName = lname;
            Phone = phone;
            Description = desc;
            Email = email;
            StatusID = statusID;


        }

        public FaceReportPrivate(FactReport report)
        {
            ReportID = report.ReportID;
            TimeStamp = report.TimeStamp;
            URL = report.URL;
            CategoryID = report.CategoryID;
            Location = report.Location;
            Name = report.Name;
            LastName = report.LastName;
            Phone = report.Phone;
            Description = report.Description;
            Email = report.Description;
            StatusID = report.StatusID;
            ScreenShot = report.ScreenShot;

        }

    }

    public class DimStatuPrivate
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public DimStatuPrivate()
        {
            StatusID = 0;
            StatusName = "Pending";
        }
        public DimStatuPrivate(int statusID, string statusname)
        {
            StatusID = statusID;
            StatusName = statusname;
        }
    }
}

