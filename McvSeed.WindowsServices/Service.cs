using MvcSeed.Component.Helpers;
using Quartz;
using Quartz.Impl;
using System.ServiceProcess;

namespace McvSeed.WindowsServices
{
    public partial class Service : ServiceBase
    {
        private IScheduler scheduler;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            LogHelper.Error("Service.OnStart");
        }

        protected override void OnStop()
        {
            scheduler.Shutdown();
            LogHelper.Error("Service.OnStop");
        }
    }
}
