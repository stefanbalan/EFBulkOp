﻿using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using EFTest.OneManyOne;

namespace EFBulkOp
{
    internal interface ITest
    {
        void Run(int count, int iter, bool loadAfter);
    }

    public abstract class Test : ITest
    {
        protected ExecutionTimer Timer = new ExecutionTimer("test");

        public virtual void Run(int count = 10_000, int iter = 1, bool loadAfter = false)
        {
            var parentId = Create();
            Timer.Start();

            for (var i = 0; i < iter; i++)
            {
                using (var ctx = new TestContext())
                {
                    LoadAndAddChildren(ctx, parentId, count);
                }
            }
            if (loadAfter)
            {
                using (var ctx = new TestContext())
                {
                    LoadParent(ctx, parentId);
                }
            }
            var timertext = Timer.Stop();
            Debug.WriteLine(timertext);
            FileLogger.Info(timertext);
        }

        protected int Create()
        {
            using (var ctx = new TestContext())
            {
                var parent = new Parent { Name = Guid.NewGuid().ToString("N") };
                ctx.ParentSet.Add(parent);
                ctx.SaveChanges();

                return parent.Id;
            }
        }

        protected void LoadAndAddChildren(TestContext ctx, int parentId, int count)
        {
            var parent = LoadParent(ctx, parentId);
            if (parent == null) return;
            Add(ctx, parent, count);
        }

        protected virtual Parent LoadParent(TestContext ctx, int parentId)
        {
            var parent = ctx.ParentSet
                .Include(p => p.ChildrenRel)
                .Include(p => p.ChildrenRel.Select(rel => rel.Child))
                .FirstOrDefault(p => p.Id == parentId);

            Timer.CheckPoint("\r\nLoaded", $"{parent?.ChildrenRel.Count} children");

            return parent;
        }

        protected abstract void Add(TestContext ctx, Parent parent, int count);
    }
}
