using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoWebApis.Models;

namespace ProyectoWebApis.DataBase
{
    public class ApplicationDbContext : IdentityDbContext
    {   
        public DbSet<User> Users { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Record> Records { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Operation>()
                .HasKey(o => o.Id);
            builder.Entity<Record>()
                .HasKey(r => r.Id);
            builder.Entity<Record>()
                .HasOne(r => r.Operation)
                .WithMany()
                .HasForeignKey(r => r.Operation_Id);  
            builder.Entity<Record>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.User_Id);

        }
    }
}


