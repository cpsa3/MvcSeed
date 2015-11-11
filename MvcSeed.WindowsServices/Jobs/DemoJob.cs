using MvcSeed.Component.Helpers;
using Quartz;
using System;

namespace MvcSeed.WindowsServices.Jobs
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
