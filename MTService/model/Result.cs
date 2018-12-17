/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    结果信息实体类
** Ver.:     V1.0.0
**************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace TPCService.model
{
    #region 结果实体类
    /// <summary>
    /// 结果实体类
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 结果
        /// </summary>
        public object result { set; get; }
    }
    #endregion

    #region layui table实体类
    /// <summary>
    /// layui table实体类
    /// </summary>
    public class LayTableResult<T>
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }

        /// <summary>
        /// 数量总数
        /// </summary>
        public int count { set; get; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> data { set; get; }
    }
    #endregion

    #region layui table实体类
    /// <summary>
    /// layui table实体类
    /// </summary>
    public class projSim
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int status { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { set; get; }


        /// <summary>
        /// 数据集合
        /// </summary>
        public JArray result { set; get; }
    }
    #endregion

    public class T_tree : Plan
    {
        //internal string lGuid;

        public string GUID { get; set; }
    }
    public class T
    {
        public int F_id { set; get; }
        public string id { set; get; }

        public Int64 treeID { set; get; }

        public long key_project_id { set; get; }
        public string name { set; get; }

        public int progress { set; get; }
        public sbyte progressbyworking { set; get; }

        public sbyte relevance { set; get; }
        public string type { set; get; }
        public string typeId { set; get; }
        public string description { set; get; }
        public string code { set; get; }
        public string level { set; get; }
        public string status { set; get; }
        public string depends { set; get; }
        public bool canwrite { set; get; }
        public string start { set; get; }
        public string duration { set; get; }
        public string end { set; get; }
        public bool startIsMilestone { set; get; }
        public bool endIsMilestone { set; get; }
        public string collapsed { set; get; }
        public string hasChild { set; get; }

    }
    public class Plan
    {
        public long id { get; set; }
        public long order { get; set; }
        public long pid { get; set; }
        public bool Checked { get; set; }
        public long proj_id { get; set; }
        public string name { get; set; }
        public int progress { get; set; }
        public bool progressbyworking { get; set; }
        public bool relevance { get; set; }
        public string type { get; set; }
        public string typeid { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public int level { get; set; }
        public string status { get; set; }
        public string depends { get; set; }
        public bool canwrite { get; set; }
        public string start { get; set; }
        public string duration { get; set; }
        public string end { get; set; }
        public bool startismilestone { get; set; }
        public bool endismilestone { get; set; }
        public bool collapsed { get; set; }
        public bool haschild { get; set; }
        public bool isdelete { get; set; }  
    }

    public class guid
    {
        public string guidd { get; set; }
    }

    }