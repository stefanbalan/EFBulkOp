using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;

namespace EFTest.OneManyOne
{
    public class TestOMOContext : DbContext
    {
        public TestOMOContext() : base("EfTests")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public DbSet<Parent> ParentSet { get; set; }
        public DbSet<Child> ChildSet { get; set; }
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

        public virtual ICollection<ParentChildRel> ParentsRel { get; set; } = new List<ParentChildRel>();
    }
}
