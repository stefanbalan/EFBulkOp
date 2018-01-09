using System;
using System.Collections.Generic;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test3 : Test
    {
        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            var childrenAdded = new List<Child>(count);

            for (var i = 0; i < count; i++)
            {
                var child = new Child
                {
                    Name = Guid.NewGuid().ToString("N")
                };
                childrenAdded.Add(child);

                ctx.ChildSet.Add(child);
            }
            timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            timer.CheckPoint("Saved");

            using (var ctx2 = new Test2Context())
            {
                foreach (var child in childrenAdded)
                {
                    ctx2.ParentChildRel2Set.Add(new ParentChildRel2
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
