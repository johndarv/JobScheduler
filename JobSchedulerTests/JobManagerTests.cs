namespace JobSchedulerTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JobScheduler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JobManagerTests
    {
        private JobGenerator jobGenerator;

        public JobManagerTests()
        {
            this.jobGenerator = new JobGenerator();
        }

        [TestMethod]
        public void JobManager_Peek_ReturnsHighestPriorityJobInQueue()
        {
            var highPriorityJob = this.jobGenerator.GenerateHighPriorityJob();
            var lowPriorityJob = this.jobGenerator.GenerateLowPriorityJob();

            var jobManager = new JobManager();

            jobManager.Register(lowPriorityJob);
            jobManager.Register(highPriorityJob);

            Job nextJob = jobManager.Peek();

            Assert.AreEqual(highPriorityJob.Id, nextJob.Id, "The next job was not the highest priority job provided to the JobManager.");
        }

        [TestMethod]
        public async Task JobManager_ExecutesHighestPriorityJob()
        {
            var jobManager = new JobManager();
            var thingDoer = new ThingDoer();

            var mediumPriorityJob = this.jobGenerator.GenerateMediumPriorityJob(thingDoer.DoThing);

            jobManager.Register(mediumPriorityJob);

            var result = await thingDoer.FinishedNumberOfThings(1, TimeSpan.FromMilliseconds(100));

            Assert.IsTrue(result, "Thing doer did not do the thing.");
        }

        [TestMethod]
        public async Task JobManager_ExecutesJobsInOrderOfPriority()
        {
            var jobManager = new JobManager();
            var thingDoer = new ThingDoer();

            var lowPriorityJob1 = this.jobGenerator.GenerateLowPriorityJob(thingDoer.DoThing);
            var lowPriorityJob2 = this.jobGenerator.GenerateLowPriorityJob(thingDoer.DoThing);
            var highPriorityJob = this.jobGenerator.GenerateHighPriorityJob(thingDoer.DoThing);
            var mediumPriorityJob = this.jobGenerator.GenerateMediumPriorityJob(thingDoer.DoThing);

            jobManager.Register(lowPriorityJob1);
            jobManager.Register(lowPriorityJob2);
            jobManager.Register(highPriorityJob);
            jobManager.Register(mediumPriorityJob);

            var result = await thingDoer.FinishedNumberOfThings(4, TimeSpan.FromSeconds(2));

            // Wait for the completed jobs to be added to the list in the job manager
            Thread.Sleep(TimeSpan.FromMilliseconds(10));

            Assert.IsTrue(result, "Thing doer did not do the 4 things.");

            Assert.AreEqual(lowPriorityJob1.Id, jobManager.CompletedJobs.First().Id, "First job completed was not correct.");
            Assert.AreEqual(highPriorityJob.Id, jobManager.CompletedJobs[1].Id, "Second job completed was not correct.");
            Assert.AreEqual(mediumPriorityJob.Id, jobManager.CompletedJobs[2].Id, "Third job completed was not correct.");
            Assert.AreEqual(lowPriorityJob2.Id, jobManager.CompletedJobs[3].Id, "Fourth job completed was not correct.");
        }

        [TestMethod]
        public async Task JobManager_CanQueueJobsWithParameters()
        {
            var jobManager = new JobManager();
            var thingDoer = new ThingDoer();

            var job = new Job(Guid.NewGuid().ToString(), 1, new Task(() => thingDoer.DoThing(TimeSpan.FromMilliseconds(1))));

            jobManager.Register(job);

            var result = await thingDoer.FinishedNumberOfThings(1, TimeSpan.FromMilliseconds(100));

            Assert.IsTrue(result, "Thing doer did not do the thing.");
        }

        [TestMethod]
        public async Task JobManager_CanQueueJobViaAction()
        {
            var jobManager = new JobManager();
            var thingDoer = new ThingDoer();

            var job = new Job(Guid.NewGuid().ToString(), 1, thingDoer.DoThing);

            jobManager.Register(job);

            var result = await thingDoer.FinishedNumberOfThings(1, TimeSpan.FromMilliseconds(100));

            Assert.IsTrue(result, "Thing doer did not do the thing.");
        }
    }
}
