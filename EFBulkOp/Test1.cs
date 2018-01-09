using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using EFBulkOp;

namespace EFTests
{
    public class Test1
    {
        private readonly ExecutionTimer timer = new ExecutionTimer("test");

        public void Run()
        {
            var parentId = Create();
            timer.Start();

            for (int i = 0; i < 10; i++)
            {
                using (var ctx = new TestContext())
                {
                    LoadAndAddChildren(ctx, parentId);
                }
            }
            using (var ctx = new TestContext())
            {
                LoadParent(ctx, parentId);
            }
            var timertext = timer.Stop();
            Debug.WriteLine(timertext);
        }

        private int Create()
        {
            using (var ctx = new TestContext())
            {
                var parent = new Parent { Name = Guid.NewGuid().ToString("N") };
                ctx.ParentSet.Add(parent);
                ctx.SaveChanges();

                return parent.Id;
            }
        }

        public void LoadAndAddChildren(TestContext ctx, int parentId)
        {

            var parent = LoadParent(ctx, parentId);

            if (parent == null) return;


            Add(ctx, parent, 10_000);

        }

        private Parent LoadParent(TestContext ctx, int parentId)
        {
            var parent = ctx.ParentSet
                .Include(p => p.ChildRels)
                .Include(p => p.ChildRels.Select(rel => rel.Child))
                .FirstOrDefault(p => p.Id == parentId);

            timer.CheckPoint("Loaded \n", $"{parent?.ChildRels.Count} children");

            return parent;
        }

        private void Add(TestContext ctx, Parent parent, int count)
        {
            for (var i = 0; i < count; i++)
            {
                parent.ChildRels.Add(new ParentChildRel
                {
                    Parent = parent,
                    Child = new Child
                    {
                        Name = Guid.NewGuid().ToString("N")
                    }
                });
            }
            timer.CheckPoint($"Added {count}\n", $"{parent.ChildRels.Count} children");

            ctx.SaveChanges();
            timer.CheckPoint("Saved \n", $"{parent.ChildRels.Count} children");
        }
    }
}
