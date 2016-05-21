namespace JobScheduler
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Job
    {
        private readonly Task task;

        public Job(string id, int priority, Action action)
            : this(id, priority, new Task(() => action()))
        {
        }

        public Job(string id, int priority, Task task)
        {
            this.Id = id;
            this.Priority = priority;
            this.task = task;
        }

        public string Id { get; private set; }

        public int Priority { get; private set; }

        public bool IsCompleted
        {
            get
            {
                return this.task.IsCompleted;
            }
        }

        public Task Execute()
        {
            this.task.Start();

            return this.task;
        }
    }
}