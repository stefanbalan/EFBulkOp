using System;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test2 : Test
    {
        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            for (var i = 0; i < count; i++)
            {
                ctx.ParentChildRels.Add(new ParentChildRel
                {
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
