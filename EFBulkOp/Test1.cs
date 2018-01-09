using System;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test1 : Test
    {
        private readonly ExecutionTimer timer = new ExecutionTimer("test");


        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            for (var i = 0; i < count; i++)
            {
                parent.ChildrenRel.Add(new ParentChildRel
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
