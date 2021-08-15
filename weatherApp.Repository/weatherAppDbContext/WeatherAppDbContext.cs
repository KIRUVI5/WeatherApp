using Microsoft.EntityFrameworkCore;
using weatherApp.DataAccess.Models;

namespace weatherApp.Repository.WeatherAppDbContext
{
    public partial class WeatherDbContext : DbContext
    {
        public WeatherDbContext()
        {
        }

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WeatherTbl> WeatherTbl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherTbl>(entity =>
            {
                entity.HasKey(e => e.WId);

                entity.ToTable("weather_tbl");

                entity.Property(e => e.WId).HasColumnName("W_ID");

                entity.Property(e => e.WDateTime)
                    .HasColumnName("W_DateTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WHumidity).HasColumnName("W_Humidity");

                entity.Property(e => e.WMaxTemperature).HasColumnName("W_max_temperature");

                entity.Property(e => e.WMinTemperature).HasColumnName("W_Min_Temperature");

                entity.Property(e => e.WTemperature).HasColumnName("W_Temperature");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
