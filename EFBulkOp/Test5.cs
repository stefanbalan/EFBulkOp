using System.Diagnostics;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test5 : Test4
    {
        /// <summary>
        /// Adds all <paramref name="count"/> children in 1 "session"
        /// No loading of results
        /// </summary>
        public Test5()
        {
            timer.SetTitle("Test 5");
        }

        public override void Run(int count = 10_000)
        {
            var parentId = Create();
            timer.Start();


            using (var ctx = new TestContext())
            {
                var parent = LoadParent(ctx, parentId);

                Add(ctx, parent, count);
            }
            var timertext = timer.Stop();
            Debug.WriteLine(timertext);
            FileLogger.Info(timertext);
        }

    }
}
