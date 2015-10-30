using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSeed.Business.Util;
using MvcSeed.Component.Data;
using MvcSeed.Component.Helpers;
using Quartz;
using Microsoft.Practices.Unity;

namespace McvSeed.WindowsServices.Jobs
{
    public class DemoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                LogHelper.Error("DemoJob 开始\r\n");

                //Bootstrapper.Instance.Initialise();
                //using (var unitOfWork = Bootstrapper.Instance.UnityContainer.Resolve<IUnitOfWork>("ReadUnitOfWork"))
                //{
                //    //TODO 添加处理逻辑
                //    LogHelper.Error("DemoJob 结束\r\n");
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("DemoJob 异常", ex);
            }
        }
    }
}
