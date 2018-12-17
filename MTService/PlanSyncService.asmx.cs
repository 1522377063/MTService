using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using MTService.enums;
using MTService.service;
using MTService.service.impl;
using TPCService.enums;
using TPCService.utils;

namespace MTService
{
    /// <summary>
    /// planSync 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PlanSync : System.Web.Services.WebService
    {
        private IPlanSyncService planSyncService = new PlanSyncService();
        private IProgressModelSyncService progressModelSyncService = new ProgressModelSyncService();
        /// <summary>
        /// 获取代办消息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void GetPlanWbsTaskInfos(string plan_code,Int64 proj_id )
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            
            Context.Response.Write(planSyncService.GetPlanWbsTaskInfos(plan_code, proj_id));

            Context.Response.End();
        }

        /// <summary>
        /// 获取进度模型信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void GetProgressModelInfos(string date, TimeStatus status,Int64 projId)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(progressModelSyncService.GetProgressModelInfos(date,status,projId));

            Context.Response.End();
        }
    }
}
