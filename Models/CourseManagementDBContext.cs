using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CourseManagementSystem.Models
{
    public partial class CourseManagementDBContext : DbContext
    {
        public CourseManagementDBContext()
        {
        }

        public CourseManagementDBContext(DbContextOptions<CourseManagementDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Take> Takes { get; set; }
        public virtual DbSet<Teach> Teaches { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              //  optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database='CourseManagementDB';Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DeptIdFk).HasColumnName("deptId_fk");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Place)
                    .HasMaxLength(10)
                    .HasColumnName("place")
                    .IsFixedLength(true);

                entity.HasOne(d => d.DeptIdFkNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DeptIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("deptId_fk");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Dean)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dean");

                entity.Property(e => e.DeptName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dept_name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.DeptIdFk).HasColumnName("dept_Id_fk");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.HasOne(d => d.DeptIdFkNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.DeptIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dept_Id_fk");
            });

            modelBuilder.Entity<Take>(entity =>
            {
                entity.Property(e => e.CourseIdFk).HasColumnName("courseId_fk");

                entity.Property(e => e.Grade).HasColumnName("grade");

                entity.Property(e => e.Semester)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("semester");

                entity.Property(e => e.StudentIdFk).HasColumnName("studentId_fk");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.CourseIdFkNavigation)
                    .WithMany(p => p.Takes)
                    .HasForeignKey(d => d.CourseIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("courseId_fk");

                entity.HasOne(d => d.StudentIdFkNavigation)
                    .WithMany(p => p.Takes)
                    .HasForeignKey(d => d.StudentIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("studentId_fk");
            });

            modelBuilder.Entity<Teach>(entity =>
            {
                entity.Property(e => e.CourseIdFkk).HasColumnName("courseId_fkk");

                entity.Property(e => e.Semester)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("semester");

                entity.Property(e => e.TeacherIdFk).HasColumnName("teacherId_fk");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.CourseIdFkkNavigation)
                    .WithMany(p => p.Teaches)
                    .HasForeignKey(d => d.CourseIdFkk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("courseId_fkk");

                entity.HasOne(d => d.TeacherIdFkNavigation)
                    .WithMany(p => p.Teaches)
                    .HasForeignKey(d => d.TeacherIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("teacherId_fk");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.DeptId).HasColumnName("deptId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.DeptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("deptId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
