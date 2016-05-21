namespace JobSchedulerTests
{
    using System;
    using System.Threading.Tasks;
    using JobScheduler;

    internal class JobGenerator
    {
        public Job GenerateHighPriorityJob()
        {
            var thingDoer = new ThingDoer();

            return this.GenerateHighPriorityJob(thingDoer.DoThing);
        }

        public Job GenerateLowPriorityJob()
        {
            var thingDoer = new ThingDoer();

            return this.GenerateLowPriorityJob(thingDoer.DoThing);
        }

        public Job GenerateHighPriorityJob(Action action)
        {
            return this.GenerateJobWithPriority(1, action);
        }

        public Job GenerateLowPriorityJob(Action action)
        {
            return this.GenerateJobWithPriority(100, action);
        }

        public Job GenerateMediumPriorityJob(Action action)
        {
            return this.GenerateJobWithPriority(50, action);
        }

        private Job GenerateJobWithPriority(int priority, Action action)
        {
            return new Job(Guid.NewGuid().ToString(), priority, new Task(() => action()));
        }
    }
}
