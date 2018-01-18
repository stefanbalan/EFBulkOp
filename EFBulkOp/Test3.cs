using System;
using System.Collections.Generic;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test3 : Test
    {
        public Test3()
        {
            Timer.SetTitle("Test 3");
        }

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
            Timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            Timer.CheckPoint("Saved");

            using (var ctx2 = new Test2Context())
            {
                foreach (var child in childrenAdded)
                {
                    ctx2.ParentChildRel2Set.Add(new ParentChildSimpleRel
                    {
                        ParentId = parent.Id,
                        ChildId = child.Id
                    });
                }
                Timer.CheckPoint($"Added rel {count}");

                ctx2.SaveChanges();
                Timer.CheckPoint("Saved rel");
            }
        }
    }
}
