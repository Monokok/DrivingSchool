using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DomainModel
{
    public partial class DSModel : DbContext
    {
        public DSModel()
            : base("name=DSModel")
        {
        }

        public virtual DbSet<car> car { get; set; }
        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<course> course { get; set; }
        public virtual DbSet<invite_course> invite_course { get; set; }
        public virtual DbSet<lesson> lesson { get; set; }
        public virtual DbSet<lesson_type> lesson_type { get; set; }
        public virtual DbSet<payment> payment { get; set; }
        public virtual DbSet<payment_type> payment_type { get; set; }
        public virtual DbSet<student> student { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<teacher> teacher { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category>()
                .HasMany(e => e.course)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.category_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<course>()
                .HasMany(e => e.invite_course)
                .WithRequired(e => e.course)
                .HasForeignKey(e => e.group_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<lesson>()
                .Property(e => e.cost)
                .IsFixedLength();

            modelBuilder.Entity<lesson_type>()
                .HasMany(e => e.lesson)
                .WithRequired(e => e.lesson_type)
                .HasForeignKey(e => e.type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<payment_type>()
                .HasMany(e => e.lesson)
                .WithRequired(e => e.payment_type)
                .HasForeignKey(e => e.payment_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<student>()
                .HasMany(e => e.invite_course)
                .WithRequired(e => e.student)
                .HasForeignKey(e => e.student_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<student>()
                .HasMany(e => e.lesson)
                .WithRequired(e => e.student)
                .HasForeignKey(e => e.student_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<teacher>()
                .HasMany(e => e.car)
                .WithOptional(e => e.teacher)
                .HasForeignKey(e => e.teacher_id);

            modelBuilder.Entity<teacher>()
                .HasMany(e => e.course)
                .WithRequired(e => e.teacher)
                .HasForeignKey(e => e.teacher_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<teacher>()
                .HasMany(e => e.lesson)
                .WithRequired(e => e.teacher)
                .HasForeignKey(e => e.teacher_id)
                .WillCascadeOnDelete(false);
        }
    }
}
