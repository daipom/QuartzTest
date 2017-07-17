using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Job;
using Quartz.Listener;
using Quartz.Util;
using Quartz.Core;
using Quartz.Collection;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.Start();

            JobDetail jobDetail = new JobDetail("myJob", null, typeof(TestJob));

            var list = new List<string>();
            list.Add("test");
            jobDetail.JobDataMap["testData"] = list;
            
            Trigger trigger = TriggerUtils.MakeSecondlyTrigger();
            trigger.StartTimeUtc = TriggerUtils.GetEvenSecondDate(DateTime.UtcNow);
            trigger.Name = "myTrigger";

            var testJobListener = new TestJobListener();
            scheduler.AddJobListener(testJobListener);
            jobDetail.AddJobListener("testListener");

            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }

    public class TestJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            Console.WriteLine("hello quartz!!");
            //throw new NotImplementedException();
        }
    }

    public class TestJobListener : IJobListener
    {
        public string Name
        {
            get
            {
                return "testListener";
                //throw new NotImplementedException();
            }
        }

        public void JobExecutionVetoed(JobExecutionContext context)
        {
            //throw new NotImplementedException();
        }

        public void JobToBeExecuted(JobExecutionContext context)
        {
            var list = (List<string>) context.JobDetail.JobDataMap["testData"];
            Console.WriteLine("start:" + list[0]);
            //throw new NotImplementedException();
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            var list = (List<string>)context.JobDetail.JobDataMap["testData"];
            list[0] = "after";
            Console.WriteLine("finished:" + list[0]);
            //throw new NotImplementedException();
        }
    }

}
