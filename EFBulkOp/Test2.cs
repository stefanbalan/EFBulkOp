using System;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test2 : Test
    {
        public Test2()
        {
            timer.SetTitle("Test 2");
        }

        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            for (var i = 0; i < count; i++) // 300ms per 1 row
            {
                ctx.ParentChildRels.Add(new ParentChildRel
                {
                    ParentId = parent.Id,
                    Parent = parent,
                    Child = new Child
                    {
                        Name = Guid.NewGuid().ToString("N")
                    }
                });
            }
            timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            timer.CheckPoint("Saved");
        }
    }
}
