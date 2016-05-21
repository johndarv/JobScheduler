namespace JobScheduler
{
    using System.Collections.Generic;

    internal class JobComparer : IComparer<Job>
    {
        public int Compare(Job x, Job y)
        {
            return x.Priority.CompareTo(y.Priority);
        }
    }
}
