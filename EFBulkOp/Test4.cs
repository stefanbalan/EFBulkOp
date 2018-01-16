﻿using System;
using System.Collections.Generic;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    public class Test4 : Test
    {
        public Test4()
        {
            timer.SetTitle("Test 4");
        }

        protected override void Add(TestContext ctx, Parent parent, int count)
        {
            var batch = new ChildBatch
            {
                DateTime = DateTime.Now,
                Children = new List<Child>()
            };
            ctx.SessionSet.Add(batch);
            ctx.SaveChanges();

            for (var i = 0; i < count; i++)
            {
                var child = new Child
                {
                    Name = Guid.NewGuid().ToString("N"),
                    BatchId = batch.Id
                };
                batch.Children.Add(child);
            }
            timer.CheckPoint($"Added {count}");

            ctx.SaveChanges();
            timer.CheckPoint("Saved" );

            using (var ctx2 = new Test2Context())
            {
                foreach (var child in batch.Children)
                {
                    ctx2.ParentChildRel2Set.Add(new ParentChildSimpleRel
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
