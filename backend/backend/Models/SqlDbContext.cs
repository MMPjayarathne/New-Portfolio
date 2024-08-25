using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options) { }

        // DbSet for PersonalInfo entity
        public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

