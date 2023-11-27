using BinksNoSake.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BinksNoSake.Domain.Models
{
    public class BinksNoSakeContext : IdentityDbContext<Account, Role, int, 
                                                    IdentityUserClaim<int>, AccountRole, IdentityUserLogin<int>, 
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public BinksNoSakeContext(DbContextOptions<BinksNoSakeContext> options) : base(options) { }

        public DbSet<PirataModel> Piratas { get; set; }
        public DbSet<CapitaoModel> Capitaes { get; set; }
        public DbSet<NavioModel> Navios { get; set; }
        public DbSet<TimoneiroModel> Timoneiros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountRole>(accountRole =>
            {
                accountRole.HasKey(ar => new { ar.UserId, ar.RoleId });
                accountRole.HasOne(ar => ar.Account).WithMany(a => a.AccountRoles).HasForeignKey(ar => ar.UserId).IsRequired();
                accountRole.HasOne(ar => ar.Role).WithMany(r => r.AccountRole).HasForeignKey(ar => ar.RoleId).IsRequired();
            });

            // N:1 PirataModel e CapitaoModel
            modelBuilder.Entity<PirataModel>()
                        .HasOne(p => p.Capitao)
                        .WithMany(c => c.Piratas)
                        .HasForeignKey(p => p.CapitaoId)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:N PirataModel e NavioModel
            modelBuilder.Entity<PirataModel>()
                        .HasMany(p => p.Navios)
                        .WithOne(n => n.Pirata)
                        .HasForeignKey(p => p.PirataId)
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
                        .HasForeignKey(n => n.PirataId)
                        .OnDelete(DeleteBehavior.SetNull);

            // 1:1 TimoneiroModel e CapitaoModel
            modelBuilder.Entity<TimoneiroModel>()
                        .HasOne(t => t.Capitao)
                        .WithOne(c => c.Timoneiro)
                        .HasForeignKey<CapitaoModel>(c => c.TimoneiroId)
                        .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
