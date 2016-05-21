namespace JobSchedulerTests
{
    using System.Linq;
    using JobScheduler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OrderedJobListTests
    {
        private JobGenerator jobGenerator;

        public OrderedJobListTests()
        {
            this.jobGenerator = new JobGenerator();
        }

        [TestMethod]
        public void OrderedJobList_Add_WhenTwoJobsAreAdded_HighestPriorityJobIsFirstInTheList()
        {
            var highPriorityJob = this.jobGenerator.GenerateHighPriorityJob();
            var lowPriorityJob = this.jobGenerator.GenerateLowPriorityJob();

            var orderedJobList = new OrderedJobList();

            orderedJobList.Add(lowPriorityJob);
            orderedJobList.Add(highPriorityJob);

            var firstJobInList = orderedJobList.First();

            Assert.AreEqual(highPriorityJob.Id, firstJobInList.Id, "The first job in the list was not the highest priority job provided to the list.");
        }
    }
}
