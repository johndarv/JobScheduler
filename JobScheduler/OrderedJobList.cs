namespace JobScheduler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class OrderedJobList : IEnumerable<Job>
    {
        private readonly List<Job> jobList;

        public OrderedJobList()
        {
            this.jobList = new List<Job>();
        }

        public int Count
        {
            get
            {
                return this.jobList.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.jobList.Count == 0;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Job NextJob
        {
            get
            {
                return this.jobList.FirstOrDefault();
            }
        }

        public void Add(Job item)
        {
            var index = this.jobList.BinarySearch(item, new JobComparer());

            if (index < 0)
            {
                index = ~index;
            }

            this.jobList.Insert(index, item);
        }

        public IEnumerator<Job> GetEnumerator()
        {
            return this.jobList.GetEnumerator();
        }

        public Job Pop()
        {
            var job = this.jobList.FirstOrDefault();

            if (job != null)
            {
                this.jobList.Remove(job);
            }

            return job;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.jobList.GetEnumerator();
        }
    }
}
