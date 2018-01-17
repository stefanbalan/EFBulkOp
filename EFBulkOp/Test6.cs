using System;
using System.Collections.Generic;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test6 : Test5
    {
        /// <summary>
        /// Adds all  "count" children in 1 "session"
        /// No loading of results
        /// uses "batch" child collection insertion both for Child AND for ParentChildRel
        /// </summary>
        public Test6()
        {
            timer.SetTitle("Test 6");
        }

        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            var childBatch = new ChildBatch
            {
                DateTime = DateTime.Now,
                Children = new List<Child>()
            };
            ctx.SessionSet.Add(childBatch);
            ctx.SaveChanges();

            for (var i = 0; i < count; i++)
            {
                var child = new Child
                {
                    Name = Guid.NewGuid().ToString("N"),
                    BatchId = childBatch.Id
                };
                childBatch.Children.Add(child);
            }
            timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            timer.CheckPoint("Saved");

            using (var ctx2 = new Test2Context())
            {
                var relBatch = new ParentChildBatch()
                {
                    DateTime = DateTime.Now,
                    ParentChildSimpleRels = new List<ParentChildSimpleRel>()
                };
                ctx2.ParentChildBatchSet.Add(relBatch);

                foreach (var child in childBatch.Children)
                {
                    //ctx2.ParentChildRel2Set.Add(new ParentChildSimpleRel
                    relBatch.ParentChildSimpleRels.Add(new ParentChildSimpleRel
                    {
                        ParentId = parent.Id,
                        ChildId = child.Id
                    });
                }
                timer.CheckPoint($"Added rel {count}");

                ctx2.SaveChanges();
                timer.CheckPoint("Saved rel");
            }
        }
    }
}
