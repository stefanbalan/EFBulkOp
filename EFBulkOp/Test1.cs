using System;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    /// <summary>
    /// 
    /// </summary>
    public class Test1 : Test
    {
        public Test1()
        {
            Timer.SetTitle("Test 1");
        }

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
            Timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            Timer.CheckPoint("Saved");
        }
    }
}
