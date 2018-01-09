using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

namespace EFTest.ManyToMany
{
    public class TestMMContext : DbContext
    {
        public TestMMContext() : base("EFBulkOp")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public DbSet<Parent> ParentSet { get; set; }
        public DbSet<Child> ChildSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Children)
                .WithMany(p => p.Parents)
                .Map(cf => cf
                    .ToTable("ParentChildRel")
                    .MapLeftKey("ParentId")
                    .MapRightKey("ChildId"));
        }
    }


    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Child> Children { get; set; } = new List<Child>();
    }

    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
    }
}