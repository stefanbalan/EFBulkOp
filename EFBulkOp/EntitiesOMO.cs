using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;

namespace EFTest.OneManyOne
{
    public class TestContext : DbContext
    {
        public DbSet<Parent> ParentSet { get; set; }
        public DbSet<Child> ChildSet { get; set; }

        public DbSet<ParentChildRel> ParentChildRels { get; set; }
        public DbSet<ChildBatch> SessionSet { get; set; }


        public DbSet<ParentChildBatchTemplate> ParentChildBatcheTemplateSet { get; set; }

        public TestContext() : base("EFBulkOp") { }
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

        public int? ParentChildBatchId { get; set; }
    }

    [Table("Children")]
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<ParentChildRel> ParentRels { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey("BatchId")]
        public virtual ChildBatch AddedInBatch { get; set; }
    }

    //[Table("Batch")]
    public class ChildBatch
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<Child> Children { get; set; }
    }    
    
    [Table("ParentChildBatches")]
    public class ParentChildBatchTemplate
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
    }


    public class Test2Context : DbContext
    {
        public DbSet<ParentChildSimpleRel> ParentChildRel2Set { get; set; }
        public DbSet<ParentChildBatch> ParentChildBatchSet { get; set; }

        public Test2Context() : base("EFBulkOp") { }
    }

    [Table("ParentChildBatches")]
    public class ParentChildBatch
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<ParentChildSimpleRel> ParentChildSimpleRels { get; set; }
    }


    [Table("ParentChildRel")]
    public class ParentChildSimpleRel
    {
        [Key]
        [Column(Order = 1)]
        public int ParentId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ChildId { get; set; }

        public int? ParentChildBatchId { get; set; }
        [ForeignKey("ParentChildBatchId")]
        public virtual ParentChildBatch AddedInBatch { get; set; }
    }
}
