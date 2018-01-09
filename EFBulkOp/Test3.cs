using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using EFBulkOp;

namespace EFTests
{
    public class Test3
    {
        private readonly ExecutionTimer timer = new ExecutionTimer("test");

        public void Run()
        {
            try
            {
                var parentId = Create();
                timer.Start();

                for (int i = 0; i < 10; i++)
                {
                    using (var ctx = new TestContext())
                    {
                        var ctx2 = new Test2Context();

                        LoadAndAddChildren(ctx, ctx2, parentId, 1000);

                        ctx2.Dispose();
                    }
                }
                using (var ctx = new TestContext())
                {
                    LoadParent(ctx, parentId);
                }
                var timertext = timer.Stop();
                Debug.WriteLine(timertext);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

        public void LoadAndAddChildren(TestContext ctx, Test2Context ctx2, int parentId, int count)
        {

            var parent = LoadParent(ctx, parentId);

            if (parent == null) return;

            Add(ctx, ctx2, parent, count);

        }

        private Parent LoadParent(TestContext ctx, int parentId)
        {
            var parent = ctx.ParentSet
                .Include(p => p.ChildRels)
                .Include(p => p.ChildRels.Select(rel => rel.Child))
                .FirstOrDefault(p => p.Id == parentId);

            timer.CheckPoint("\nLoaded", $"{parent?.ChildRels.Count} children");

            return parent;
        }

        private void Add(TestContext ctx, Test2Context ctx2, Parent parent, int count)
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
            timer.CheckPoint($"\nAdded {count}", $"{parent.ChildRels.Count} children");

            ctx.SaveChanges();
            timer.CheckPoint("\nSaved", $"{parent.ChildRels.Count} children");

            foreach (Child child in childrenAdded)
            {
                ctx2.ParentChildRels2.Add(new ParentChildRel2
                {
                    ParentId = parent.Id,
                    ChildId = child.Id
                });
            }
            timer.CheckPoint($"\nAdded rel {count}", $"{parent.ChildRels.Count} children");

            ctx2.SaveChanges();
            timer.CheckPoint("\nSaved rel", $"{parent.ChildRels.Count} children");
        }
    }
}
