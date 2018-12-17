using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string[] plan_codes = new string[]
            {
                "TJ5101",
                "TJ5102",
                "TJ5103",
                "TJ5104",
                "TJ5108",
            };
            string address = null;
            string result = null;
            foreach (string planCode in plan_codes)
            {
                address = "http://dev.p3china.com:9580/Plan/GetPlanWbsTaskInfos?project_code=L5P1&plan_code=" + planCode;
                result = wc.DownloadString(address);
                JObject jo = JsonConvert.DeserializeObject(result) as JObject;
                JArray array = jo["data"]["value"] as JArray;
                for (int index1 = 0; index1 < array.Count; index1++)
                {
                    if (array[index1]["task_type"].ToString().CompareTo("wbs") != 0)
                    {
                        using (FileStream fs = new FileStream(@"d:\test.txt", FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                            {
                                sw.BaseStream.Seek(0, SeekOrigin.End);
                                sw.WriteLine("{0}\r\n", array[index1]["task_type"].ToString());
                                sw.Flush();
                            }
                        }
                    }
                }
            }

        }

        [TestMethod]
        public void TestMethod2()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string address = null;
            string result = null;
            address = "http://dev.p3china.com:9580/Plan/GetPlanWbsTaskInfos?project_code=L5P1&plan_code=BIMTest";
            result = wc.DownloadString(address);
            JObject jo = JsonConvert.DeserializeObject(result) as JObject;
            JArray array = jo["data"]["value"] as JArray;
            for (int index1 = 0; index1 < array.Count; index1++)
            {
                using (FileStream fs = new FileStream(@"d:\BIMTest.txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine("{0}", array[index1]["Name"].ToString());
                        sw.Flush();
                    }
                }
            }
        }
    }
}
