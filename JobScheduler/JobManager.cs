namespace JobScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class JobManager
    {
        private readonly OrderedJobList orderedJobQueue;

        private Job currentJob;

        public JobManager()
        {
            this.orderedJobQueue = new OrderedJobList();
            this.CompletedJobs = new List<Job>();
        }

        public IList<Job> CompletedJobs { get; internal set; }

        public void Register(Job job)
        {
            if (this.currentJob == null)
            {
                this.ExecuteJob(job);
            }
            else
            {
                this.orderedJobQueue.Add(job);
            }
        }

        public Job Peek()
        {
            return this.orderedJobQueue.FirstOrDefault();
        }

        private void ExecuteJob(Job job)
        {
            this.currentJob = job;
            job.Execute();

            Task.Factory.StartNew(() => this.WaitForCurrentJobToFinish());
        }

        private void ExecuteNextJob()
        {
            var nextJob = this.orderedJobQueue.Pop();

            this.ExecuteJob(nextJob);
        }

        private void WaitForCurrentJobToFinish()
        {
            while (!this.currentJob.IsCompleted)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            this.CompletedJobs.Add(this.currentJob);
            this.currentJob = null;

            if (!this.orderedJobQueue.IsEmpty)
            {
                this.ExecuteNextJob();
            }
        }
    }
}