using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSeed.Business.Models;

namespace McvSeed.Web.Security
{
    public class CurrentUser : ICurrentUser
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string OpenId { get; set; }
    }

    /// <summary>
    /// 功能权限
    /// </summary>
    public enum FuncAuthority
    {
        今日统计_明细 = 0x1,
        今日统计_咨询 = 0x2,
        今日统计_报名 = 0x4,

        经营日报 = 0x8,
        开班结班 = 0x10,
        查看电脑版 = 0x20,
    }

    /// <summary>
    /// 数据权限
    /// </summary>
    public enum DataAuthority
    {
        全部校区 = 0x1,
        指定多校区 = 0x2,
        指定单校区 = 0x4,
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum Level
    {
        最高 = 1,
        财务 = 2,
        人事 = 8,
        教务 = 3,
        教师 = 7,
        多校主管 = 0,
        单校主管 = 4,
        前台 = 5,
        市场 = 6,
        销售员 = 9,
    }

    /// <summary>
    /// 系统版本
    /// </summary>
    public enum Crmver
    {
        免费版 = 1,
        简易版 = 2,
        专业版 = 3
    }
}