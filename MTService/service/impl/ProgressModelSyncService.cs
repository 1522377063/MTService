using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTService.entity;
using MTService.enums;
using MTService.utils;
using MySql.Data.MySqlClient;
using TPCService.enums;
using TPCService.utils;

namespace MTService.service.impl
{
    public class ProgressModelSyncService : IProgressModelSyncService
    {
        public string GetProgressModelInfos(string date, TimeStatus status, Int64 projId)
        {
            Int64 startTime = 0;
            Int64 endTime = 0;
            switch (status)
            {
                case TimeStatus.Day:
                {
                    string startDate = date + " 00:00:00";
                    string endDate = date + " 23:59:59";
                    startTime = Convert.ToInt64((Convert.ToDateTime(startDate) - new DateTime(1970, 1, 1, 8, 0, 0)).TotalMilliseconds);
                    endTime = Convert.ToInt64((Convert.ToDateTime(endDate) - new DateTime(1970, 1, 1, 8, 0, 0)).TotalMilliseconds);
                    
                    break;
                }

                case TimeStatus.Week:
                {
                    string tmpDate = date + " 00:00:00";
                    DateTime dt = Convert.ToDateTime(tmpDate);
                    DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
                    DateTime endWeek = startWeek.AddDays(7);
                    startTime = Convert.ToInt64((startWeek - new DateTime(1970, 1, 1, 8, 0, 0)).TotalMilliseconds);
                    endTime = Convert.ToInt64((endWeek - new DateTime(1970, 1, 1, 8, 0, 1)).TotalMilliseconds);
                    break;
                }
                case TimeStatus.Month:
                {
                    string tmpDate = date + " 00:00:00";
                    DateTime dt = Convert.ToDateTime(tmpDate);
                    DateTime startMonth = dt.AddDays(1 - dt.Day);
                    DateTime endMonth = startMonth.AddMonths(1);
                    startTime = Convert.ToInt64((startMonth - new DateTime(1970, 1, 1, 8, 0, 0)).TotalMilliseconds);
                    endTime = Convert.ToInt64((endMonth - new DateTime(1970, 1, 1, 8, 0, 1)).TotalMilliseconds);
                    break;
                }
                    
                default:
                    break;
            }

            try
            {
                string strSql =
                    "SELECT prmm.*, ma.attrvalue AS `name` FROM ( SELECT prm.* FROM ( SELECT pm.* FROM ( SELECT p.`id` AS pid FROM `t_progress` p WHERE p.`proj_id` =@proj_id AND p.`startTime` >=@startTime AND p.`endTime` <=@endTime) r LEFT JOIN `t_progress_model` pm ON r.`pid` = pm.`pgs_id`) prm LEFT JOIN `t_model` m ON prm.`revitid` = m.`revitid`) prmm LEFT JOIN t_model_attr ma ON prmm.`guid` = ma.`guid` WHERE ma.`attrname`='族与类型'";
                MySqlParameter[] mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@proj_id",MySqlDbType.Int64) {Value = projId},
                    new MySqlParameter("@startTime",MySqlDbType.VarChar,50) {Value = startTime.ToString()},
                    new MySqlParameter("@endTime",MySqlDbType.VarChar,50) {Value = endTime.ToString()},
                };
                List<ProgressModel> listProgressModels = ResultUtil.getResultList<ProgressModel>(strSql, mySqlParameters);
                return ResultUtil.getStandardResult((int)Status.Normal,EnumUtil.getMessageStr((int)Message.Query), listProgressModels);
            }
            catch (Exception e)
            {
                return ResultUtil.getStandardResult((int)Status.Error,EnumUtil.getMessageStr((int)Message.QueryFailure), null);
                throw;
            }
            
        }
    }
}