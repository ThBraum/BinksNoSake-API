using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Domain.Models
{
    public class BinksNoSakeContext : DbContext
    {
        public BinksNoSakeContext(DbContextOptions<BinksNoSakeContext> options) : base(options) { }

        public DbSet<PirataModel> Piratas { get; set; }
        public DbSet<CapitaoModel> Capitaes { get; set; }
        public DbSet<NavioModel> Navios { get; set; }
        public DbSet<TimoneiroModel> Timoneiros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // N:1 PirataModel e CapitaoModel
            modelBuilder.Entity<PirataModel>()
                        .HasOne(p => p.Capitao)
                        .WithMany(c => c.Piratas)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:N PirataModel e NavioModel
            modelBuilder.Entity<PirataModel>()
                        .HasMany(p => p.Navios)
                        .WithOne(n => n.Pirata)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:N CapitaoModel e PirataModel
            modelBuilder.Entity<CapitaoModel>()
                        .HasMany(c => c.Piratas)
                        .WithOne(p => p.Capitao)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:1 CapitaoModel e TimoneiroModel
            modelBuilder.Entity<CapitaoModel>()
                        .HasOne(c => c.Timoneiro)
                        .WithOne(t => t.Capitao)
                        .HasForeignKey<TimoneiroModel>(t => t.CapitaoId)
                        .OnDelete(DeleteBehavior.SetNull);

            // N:1 NavioModel e PirataModel
            modelBuilder.Entity<NavioModel>()
                        .HasOne(n => n.Pirata)
                        .WithMany(p => p.Navios)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:1 TimoneiroModel e CapitaoModel
            modelBuilder.Entity<TimoneiroModel>()
                        .HasOne(t => t.Capitao)
                        .WithOne(c => c.Timoneiro)
                        .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
