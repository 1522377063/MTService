using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using TPCService.enums;
using TPCService.utils;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;
using log4net.DateFormatter;
using MTService.ServiceReference1;

namespace MTService.service.impl
{
    public class PlanSyncService : IPlanSyncService
    {
        public string GetPlanWbsTaskInfos(string plan_code, long proj_id)
        {
            try
            {
                Dictionary<string, string> keyvalues = new Dictionary<string, string>();
                keyvalues.Add("project_code", "L5P1");//以实际为准//L5P1
                keyvalues.Add("plan_code", plan_code);//以实际为准
                string msgBody = Post("http://dev.p3china.com:9580/Plan/GetPlanWbsTaskInfos", keyvalues);
                JObject jo = Newtonsoft.Json.JsonConvert.DeserializeObject(msgBody) as JObject;

                JToken isSuccess = jo["success"] as JToken;
                if (!(bool)isSuccess)
                {
                    return ResultUtil.getStandardResult((int)Status.Error, "接收json数据状态:", "失败");
                }
                else
                {
                    deleteDataByProjectID(plan_code);
                }
                JArray array = jo["data"]["value"] as JArray;
                //===========================初始化开始====================================================
                int[] levelArray = new int[array.Count];
                bool[] hasChildArray = new bool[array.Count];
                string[] idArray = new string[array.Count];
                string[] pidArray = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    levelArray[index] = 0;
                    hasChildArray[index] = false;
                    idArray[index] = (string)array[index]["UID"];
                    pidArray[index] = (string)array[index]["ParentTaskUID"];
                }
                //=========================初始化结束======================================================
                //=========================计算level和haschild开始======================================================
                for (int indexOfPid = 1; indexOfPid < array.Count; indexOfPid++)
                {
                    for (int indexOfId = 0; indexOfId < array.Count; indexOfId++)
                    {
                        if (pidArray[indexOfPid] == idArray[indexOfId])
                        {
                            levelArray[indexOfPid] = levelArray[indexOfId] + 1;
                            hasChildArray[indexOfId] = true;
                            break;
                        }
                    }
                }
                //=========================计算level和haschild结束======================================================
                array[0]["ParentTaskUID"] = "0";
                StringBuilder sb = new StringBuilder("INSERT INTO t_plan(id,pid,`name`,progress,`start`,duration,`end`,startismilestone,endismilestone,haschild,proj_id,`order`,level,IsWBSTask) VALUES");
                for (int index1 = 0; index1 < array.Count; index1++)
                {
                    String start = array[index1]["Start"].ToString().Replace("/", "-");
                    String finish = array[index1]["Finish"].ToString().Replace("/", "-");
                    DateTime date = Convert.ToDateTime(start);
                    DateTime date1 = Convert.ToDateTime(finish);
                    DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
                    DateTime radom = new DateTime(2000, 1, 1, 0, 0, 0);
                    Int64 startTimeOri = Convert.ToInt64((date - dateStart).TotalMilliseconds);
                    Int64 finishTimeOri = Convert.ToInt64((date1 - dateStart).TotalMilliseconds);
                    Int64 randomTime = Convert.ToInt64((radom - dateStart).TotalMilliseconds);
                    string startTime = ((startTimeOri > 0) ? startTimeOri : randomTime).ToString();
                    string finishTime = ((finishTimeOri > 0) ? finishTimeOri : randomTime).ToString();

                    sb.Append("('");
                    sb.Append(array[index1]["UID"]);
                    sb.Append("','");
                    sb.Append(array[index1]["ParentTaskUID"]);
                    sb.Append("','");
                    sb.Append(array[index1]["Name"]);
                    sb.Append("',");
                    sb.Append(array[index1]["PercentComplete"]);
                    sb.Append(",'");
                    sb.Append(startTime);
                    sb.Append("','");
                    sb.Append(array[index1]["Duration"]);
                    sb.Append("','");
                    sb.Append(finishTime);
                    sb.Append("',");
                    sb.Append(array[index1]["Milestone"]);
                    sb.Append(",");
                    sb.Append(array[index1]["Milestone"]);
                    sb.Append(",");
                    sb.Append(hasChildArray[index1]);
                    sb.Append(",");
                    sb.Append(proj_id);
                    sb.Append(",'");
                    sb.Append(array[index1]["sequ"]);
                    sb.Append("',");
                    sb.Append(levelArray[index1]);
                    sb.Append(",");
                    sb.Append(array[index1]["IsWBSTask"]);
                    sb.Append(") ,");
                }
                string strTempSql = sb.ToString();
                string strSql = strTempSql.Substring(0, strTempSql.Length - 1);

                ResultUtil.insOrUpdOrDel(strSql);
                updateProject(array[0]["UID"], proj_id);

                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                String address = "http://cnanzhen.com:8047/nservice/TPCService.asmx/getTree?projid=" + proj_id +
                                 "&&type=计划";
                string result = wc.DownloadString(
                    address);

                JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JObject;
                JArray arr = jobj["result"] as JArray;
                if ((string)jobj["status"] == "200")
                {
                    return ResultUtil.getStandardResult((int)Status.Normal, "同步了" + array.Count + "条数据", arr);
                }
                else
                {
                    return ResultUtil.getStandardResult((int)Status.Error, "调用服务错误", null);
                }
            }
            catch (Exception e)
            {
                return ResultUtil.getStandardResult((int) Status.Error, "调用服务错误", null);
            }
            
        }

        private void updateProject(JToken F_planid, Int64 proj_id)
        {
            string strSql2 = "UPDATE w_t_project SET F_planid=@F_planid WHERE ID=@id";
            MySqlParameter[] mySqlParameters2 = new MySqlParameter[]
            {
                new MySqlParameter("@F_planid",MySqlDbType.VarChar,50) {Value =F_planid },
                new MySqlParameter("@id",MySqlDbType.Int64) {Value = proj_id},
            };
            ResultUtil.insOrUpdOrDel(strSql2, mySqlParameters2);
        }

        private void deleteDataByProjectID(string plan_code)
        {
            string strSql1 = "SELECT w_t_project.ID FROM w_t_project WHERE F_projName=@F_projName";
            MySqlParameter[] mySqlParameters1 = new MySqlParameter[]
            {
                new MySqlParameter("@F_projName",MySqlDbType.VarChar,30) {Value = plan_code},
            };
            Int64 proj_id = Int64.Parse(ResultUtil.getResultString(strSql1, mySqlParameters1));

            string strSql7 = "DELETE FROM t_plan WHERE t_plan.proj_id=@proj_id";
            MySqlParameter[] mySqlParameters7 = new MySqlParameter[]
            {
                new MySqlParameter("@proj_id",MySqlDbType.Int64) {Value = proj_id},
            };
            ResultUtil.insOrUpdOrDel(strSql7, mySqlParameters7);
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}