using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTService.enums;

namespace MTService.service
{
    public interface IProgressModelSyncService
    {
        string GetProgressModelInfos(string date, TimeStatus status, Int64 projId);
    }
}