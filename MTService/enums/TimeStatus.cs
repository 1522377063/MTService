using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MTService.enums
{
    public enum TimeStatus
    {
        [Description("日")]
        Day = 0,

        [Description("周")]
        Week = 1,

        [Description("月")]
        Month = 2
    }
}