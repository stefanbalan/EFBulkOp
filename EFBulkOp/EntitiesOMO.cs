using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;

namespace EFTest.OneManyOne
{
    public class TestOMOContext : DbContext
    {
        public TestOMOContext() : base("EFBulkOp")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public DbSet<Parent> ParentSet { get; set; }
        public DbSet<Child> ChildSet { get; set; }
    }

    public class Test2Context : DbContext
    {
        public DbSet<ParentChildRel2> ParentChildRel2Set { get; set; }

        public Test2Context() : base("EfTests")
        {

        }
    }

    [Table("Parents")]
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ParentChildRel> ChildrenRel { get; set; } = new List<ParentChildRel>();
    }

    [Table("ParentChildRel")]
    public class ParentChildRel
    {
        [Key]
        [Column(Order = 1)]
        public int ParentId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ChildId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }

        [ForeignKey("ChildId")]
        public virtual Child Child { get; set; }
    }

    [Table("Children")]
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SessionId { get; set; }

        public ICollection<ParentChildRel> ParentRels { get; set; }

        [ForeignKey("SessionId")]
        public virtual ContextSession AddedInSession { get; set; }
    }


    public class ContextSession
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<Child> Children { get; set; }
    }

    public class TestContext : DbContext
    {
        public DbSet<Parent> ParentSet { get; set; }
        public DbSet<Child> ChildSet { get; set; }

        public DbSet<ParentChildRel> ParentChildRels { get; set; }
        public DbSet<ContextSession> SessionSet { get; set; }

        public TestContext() : base("EfTests")
        {

        }
    }

    [Table("ParentChildRels")]
    public class ParentChildRel2
    {
        [Key]
        [Column(Order = 1)]
        public int ParentId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ChildId { get; set; }
    }


  
}
