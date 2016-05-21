namespace JobSchedulerTests
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ThingDoer
    {
        private int numberOfDoneThings = 0;

        public ThingDoer()
        {
        }

        public void DoThing()
        {
            this.DoThing(TimeSpan.FromMilliseconds(10));
        }

        public void DoThing(TimeSpan timeSpan)
        {
            Thread.Sleep(timeSpan);

            this.numberOfDoneThings++;
        }

        public Task<bool> FinishedNumberOfThings(int expectedNumberOfThings, TimeSpan timeout)
        {
            var task = Task.Factory.StartNew(
                () =>
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (this.numberOfDoneThings < expectedNumberOfThings && stopwatch.Elapsed < timeout)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(10));
                    }

                    return this.numberOfDoneThings == expectedNumberOfThings;
                });

            return task;
        }
    }
}
