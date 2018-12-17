using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTService.entity
{
    public class ProgressModel
    {
        public int id { get; set; }
        public int revitid { get; set; }
        public string guid { get; set; }
        public int pgs_id { get; set; }
        public Int64 proj_id { get; set; }
        public string name { get; set; }
    }
}