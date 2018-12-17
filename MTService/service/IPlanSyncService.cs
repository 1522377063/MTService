using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTService.service
{
    public interface IPlanSyncService
    {
        string GetPlanWbsTaskInfos(string plan_code, Int64 proj_id);
    }
}