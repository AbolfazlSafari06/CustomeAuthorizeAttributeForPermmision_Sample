using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Database;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // UserMapping
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasKey(x=>x.Id);
        modelBuilder.Entity<User>().Property(x=>x.Password).IsRequired();
        modelBuilder.Entity<User>().Property(x=>x.Name).IsRequired();
        //modelBuilder.Entity<User>().HasMany(x => x.Permissions).WithOne(x => x.User).HasForeignKey(x => x.UserId);

        modelBuilder.Entity<User>().HasData(
            new User() { Id =1,Name = "admin",Password= "123_Hashed" }
        );
        

        // Permission Mapping
        modelBuilder.Entity<Permission>().ToTable("Permission");
        modelBuilder.Entity<Permission>().HasKey(x => x.Id);
        modelBuilder.Entity<Permission>().Property(x => x.Name);
        modelBuilder.Entity<Permission>().HasData(
            new Permission(){Id = 1,Name = "strign1" },
            new Permission(){Id = 2,Name = "strign2" },
            new Permission(){Id = 3,Name = "strign3" }
        );



        // UserPermission Mapping
        modelBuilder.Entity<UserPermission>().ToTable("UserPermissions");
        modelBuilder.Entity<UserPermission>().HasKey(x => x.Id);
        
        modelBuilder.Entity<UserPermission>()
            .HasOne(x => x.User)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<UserPermission>()
            .HasOne(x => x.Permission)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.PermissionId);

        modelBuilder.Entity<UserPermission>()
            .HasData(
                new UserPermission { Id = 1, PermissionId = 1, UserId = 1 },
                new UserPermission { Id = 2, PermissionId = 2, UserId = 1 },
                new UserPermission { Id = 3, PermissionId = 3, UserId = 1 }
            );
    }
}
