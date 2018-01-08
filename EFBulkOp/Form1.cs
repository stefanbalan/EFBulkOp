using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using EFTest.ManyToMany;
using EFTest.OneManyOne;
using Child = EFTest.ManyToMany.Child;
using Parent = EFTest.ManyToMany.Parent;

namespace EFBulkOp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new TestMMContext())
                {
                    var p = new Parent { Id = 1, Name = "1" };
                    ctx.ParentSet.Add(p);

                    for (int i = 0; i < 6000; i++)
                    {
                        var c = new Child
                        {
                            Name = Guid.NewGuid().ToString("N")
                        };
                        ctx.ChildSet.Add(c);
                        p.Children.Add(c);
                        if (i % 1000 == 0)
                            SetStatus($"{i} rows added");
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                SetStatus(ex.Message);
            }
        }


        private void SetStatus(string s)
        {
            statusText.Text = s;
        }

        public class TestAddChildren
        {
            private readonly Action<string> _progress;
            private Action _done;
            public TestAddChildren(Action<string> progress, Action done)
            {
                _progress = progress;
                _done = done;
            }
            public void Run()
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new TestMMContext())
                {
                    var query = ctx.ParentSet
                        .Include(p => p.Children);
                    var parent = query
                        .First(p => p.Id == 1);

                    var cCount = parent.Children.Count;

                    SetStatus($"{cCount} children loaded");

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                SetStatus(ex.Message);
            }
        } // 2,481 sec for 120k rows

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var ctx = new TestOMOContext())
            {
                var query = ctx.ParentSet
                    .Include(p => p.ChildrenRel)
                    .Include(p => p.ChildrenRel.Select(rel => rel.Child));

                var cls = query
                    .First(p => p.Id == 1);

                var cCount = cls.ChildrenRel.Count;

                SetStatus($"{cCount} children loaded");

                ctx.SaveChanges();
            }

        } 

    }
}
