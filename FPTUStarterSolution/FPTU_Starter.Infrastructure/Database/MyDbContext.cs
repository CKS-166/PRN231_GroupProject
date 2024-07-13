using FPTU_Starter.Domain.Entity;
using FPTU_Starter.Infrastructure.ConfigEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Infrastructure.Database
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext()
        {
            
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPackage> Packages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }  
        public DbSet<Comment> Comments { get; set; }  
        public DbSet<Like> Likes { get; set; }  
        public DbSet<PackageBacker> PackageBackers { get; set; }  
        public DbSet<SystemWallet> SystemWallets { get; set; }  
        public DbSet<Wallet> Wallets { get; set; }  
        public DbSet<Transaction> Transactions { get; set; }  
        public DbSet<WithdrawRequest> WithdrawRequests { get; set; }  


        
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            return configuration.GetConnectionString("DefaultConnection");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PackageBacker>()
                .HasKey(ppu => new { ppu.ProjectPackageId, ppu.UserId });

            modelBuilder.Entity<PackageBacker>()
                .HasOne(ppu => ppu.ProjectPackage)
                .WithMany(pp => pp.ProjectPackageUsers)
                .HasForeignKey(ppu => ppu.ProjectPackageId);

            modelBuilder.Entity<PackageBacker>()
                .HasOne(ppu => ppu.ApplicationUser)
                .WithMany(au => au.ProjectPackageUsers)
                .HasForeignKey(ppu => ppu.UserId);
        }
    }
}
