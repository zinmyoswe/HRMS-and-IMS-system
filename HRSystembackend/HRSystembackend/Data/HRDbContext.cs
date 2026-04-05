using System;
using Microsoft.EntityFrameworkCore;
using HRSystembackend.Models;

namespace HRSystembackend.Data
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options) { }

        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<StaffRole> StaffRoles => Set<StaffRole>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Site> Sites => Set<Site>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<AttendanceLog> AttendanceLogs => Set<AttendanceLog>();
        public DbSet<SalaryStructure> SalaryStructures => Set<SalaryStructure>();
        public DbSet<Payroll> Payrolls => Set<Payroll>();
        public DbSet<AllowanceType> AllowanceTypes => Set<AllowanceType>();
        public DbSet<StaffAllowance> StaffAllowances => Set<StaffAllowance>();
        public DbSet<DeductionType> DeductionTypes => Set<DeductionType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Staff
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff"); // ✅ THIS FIXES YOUR ERROR
                entity.HasKey(e => e.StaffId);
                entity.HasIndex(e => e.StaffCode).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.StaffCode).IsRequired().HasMaxLength(32);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.StaffName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Staffs)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Position)
                      .WithMany(p => p.Staffs)
                      .HasForeignKey(e => e.PositionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Manager)
                      .WithMany(m => m.Subordinates)
                      .HasForeignKey(e => e.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(100);
            });

            // StaffRole junction
            modelBuilder.Entity<StaffRole>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.RoleId });
                entity.HasOne(sr => sr.Staff).WithMany(s => s.StaffRoles).HasForeignKey(sr => sr.StaffId);
                entity.HasOne(sr => sr.Role).WithMany(r => r.StaffRoles).HasForeignKey(sr => sr.RoleId);
            });

            // Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.Property(e => e.DepartmentName).IsRequired().HasMaxLength(200);
            });

            // Position
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId);
                entity.Property(e => e.PositionName).IsRequired().HasMaxLength(200);
            });

            // Site
            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(e => e.SiteId);
                entity.Property(e => e.SiteName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SiteType).IsRequired().HasMaxLength(50);
            });

            // Attendance & logs
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendanceId);
                entity.HasIndex(e => new { e.StaffId, e.WorkDate });
                entity.Property(e => e.WorkDate).HasColumnType("date");
                entity.HasOne(a => a.Staff).WithMany(s => s.Attendances).HasForeignKey(a => a.StaffId);
                entity.HasOne(a => a.Site).WithMany(s => s.Attendances).HasForeignKey(a => a.SiteId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AttendanceLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogType).IsRequired().HasMaxLength(10);
                entity.Property(e => e.LogTime).IsRequired();
                entity.HasOne(l => l.Attendance).WithMany(a => a.Logs).HasForeignKey(l => l.AttendanceId);
            });

            // SalaryStructure
            modelBuilder.Entity<SalaryStructure>(entity =>
            {
                entity.HasKey(e => e.SalaryStructureId);
                entity.Property(e => e.BasicSalary).HasColumnType("decimal(12,2)");
                entity.Property(e => e.PhoneAllowance).HasColumnType("decimal(12,2)");
                entity.Property(e => e.MealAllowance).HasColumnType("decimal(12,2)");
                entity.Property(e => e.HouseAllowance).HasColumnType("decimal(12,2)");
                entity.Property(e => e.TaxRate).HasColumnType("decimal(5,2)");
                entity.Property(e => e.SocialSecurity).HasColumnType("decimal(12,2)");
                entity.HasOne(s => s.Staff).WithMany(st => st.SalaryStructures).HasForeignKey(s => s.StaffId);
            });

            // Payroll
            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.HasKey(e => e.PayrollId);
                entity.Property(e => e.GrossSalary).HasColumnType("decimal(12,2)");
                entity.Property(e => e.NetSalary).HasColumnType("decimal(12,2)");
                entity.Property(e => e.TotalAllowance).HasColumnType("decimal(12,2)");
                entity.Property(e => e.TotalDeduction).HasColumnType("decimal(12,2)");
                entity.HasIndex(e => new { e.StaffId, e.Year, e.Month });
                entity.HasOne(p => p.Staff).WithMany(s => s.Payrolls).HasForeignKey(p => p.StaffId);
            });

            // AllowanceType
            modelBuilder.Entity<AllowanceType>(entity =>
            {
                entity.HasKey(e => e.AllowanceTypeId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // StaffAllowance
            modelBuilder.Entity<StaffAllowance>(entity =>
            {
                entity.HasKey(e => e.StaffAllowanceId);
                entity.Property(e => e.Amount).HasColumnType("decimal(12,2)");
                entity.HasOne(sa => sa.Staff).WithMany(s => s.StaffAllowances).HasForeignKey(sa => sa.StaffId);
                entity.HasOne(sa => sa.AllowanceType).WithMany(at => at.StaffAllowances).HasForeignKey(sa => sa.AllowanceTypeId);
            });

            // DeductionType
            modelBuilder.Entity<DeductionType>(entity =>
            {
                entity.HasKey(e => e.DeductionTypeId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // Seed sample data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Human Resources" },
                new Department { DepartmentId = 2, DepartmentName = "Engineering" }
            );

            modelBuilder.Entity<Position>().HasData(
                new Position { PositionId = 1, PositionName = "Software Engineer" },
                new Position { PositionId = 2, PositionName = "HR Specialist" }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Senior" },
                new Role { RoleId = 2, RoleName = "Middle" },
                new Role { RoleId = 3, RoleName = "Junior" },
                new Role { RoleId = 4, RoleName = "Intern" },
                new Role { RoleId = 5, RoleName = "Manager" },
                new Role { RoleId = 6, RoleName = "Senior Manager" },
                new Role { RoleId = 7, RoleName = "Director" },
                new Role { RoleId = 8, RoleName = "Admin" }
            );

            modelBuilder.Entity<Site>().HasData(
                new Site { SiteId = 1, SiteName = "Head Office", SiteType = "Office", Latitude = null, Longitude = null },
                new Site { SiteId = 2, SiteName = "Factory A", SiteType = "Factory", Latitude = null, Longitude = null }
            );

            modelBuilder.Entity<Staff>().HasData(
                new Staff
                {
                    StaffId = 1,
                    StaffCode = "STF001",
                    StaffName = "Alice Johnson",
                    Email = "alice.johnson@example.com",
                    Phone = "+1234567890",
                    Address = "123 Main St",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinedDate = new DateTime(2020, 6, 1),
                    DepartmentId = 2,
                    PositionId = 1,
                    ManagerId = null,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<StaffRole>().HasData(
                new StaffRole { StaffId = 1, RoleId = 1 },
                new StaffRole { StaffId = 1, RoleId = 8 }
            );

            modelBuilder.Entity<SalaryStructure>().HasData(
                new SalaryStructure
                {
                    SalaryStructureId = 1,
                    StaffId = 1,
                    BasicSalary = 8000m,
                    PhoneAllowance = 50m,
                    MealAllowance = 150m,
                    HouseAllowance = 500m,
                    TaxRate = 3m,
                    SocialSecurity = 200m,
                    EffectiveDate = DateTime.UtcNow.Date
                }
            );
        }
    }
}
