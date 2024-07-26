using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Saas.Entities;

namespace Saas.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; } = default!;
    public DbSet<Secret> Secrets { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionSecret> CollectionSecrets { get; set; }
    public DbSet<OrganizationUser> OrganizationUsers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentPlan> PaymentPlans { get; set; }
    public DbSet<OneTimeShare> OneTimeShares { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted })
            .EnableSensitiveDataLogging();
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrganizationUser>()
            .HasKey(ou => new { ou.OrganizationId, ou.CognitoUserId });

        modelBuilder.Entity<OrganizationUser>()
            .HasOne(ou => ou.Organization)
            .WithMany(o => o.OrganizationUsers)
            .HasForeignKey(ou => ou.OrganizationId);

        modelBuilder.Entity<OrganizationUser>()
            .HasOne(ou => ou.CognitoUser)
            .WithMany()
            .HasForeignKey(ou => ou.CognitoUserId);

        modelBuilder.Entity<SharedSecret>()
            .HasOne(ss => ss.User)
            .WithMany(ss => ss.SharedSecrets)
            .HasForeignKey(ss => ss.SharedTo);

        modelBuilder.Entity<SharedSecret>()
            .HasOne(ss => ss.Organization)
            .WithMany(ss => ss.SharedSecrets)
            .HasForeignKey(ss => ss.SharedTo);

        modelBuilder.Entity<Collection>()
            .HasOne(sf => sf.Organization)
            .WithMany(o => o.SecretCollections)
            .HasForeignKey(sf => sf.OrganizationId);

        modelBuilder.Entity<Collection>()
            .HasOne(sf => sf.User)
            .WithMany()
            .HasForeignKey(sf => sf.UserId);

        modelBuilder.Entity<Secret>()
            .HasOne(s => s.CreatingUser)
            .WithMany(u => u.Secrets)
            .HasForeignKey(s => s.CreatedBy);

        modelBuilder.Entity<CollectionSecret>()
           .HasKey(fs => new { fs.CollectionId, fs.SecretId });

        modelBuilder.Entity<CollectionSecret>()
           .HasOne(fs => fs.Collection)
           .WithMany(f => f.CollectionSecrets)
           .HasForeignKey(fs => fs.CollectionId);

        modelBuilder.Entity<CollectionSecret>()
            .HasOne(fs => fs.Secret)
            .WithMany(s => s.CollectionSecrets)
            .HasForeignKey(fs => fs.SecretId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Plan)
            .WithMany(pp => pp.Payments)
            .HasForeignKey(p => p.PlanId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.InitiatingUser)
            .WithMany(u => u.PaymentsInitiated)
            .HasForeignKey(p => p.InitiatingUserId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.VerifyingUser)
            .WithMany(u => u.PaymentsVerified)
            .HasForeignKey(p => p.VerifyingUserId);

        modelBuilder.Entity<PaymentPlan>()
            .HasMany(pp => pp.Users)
            .WithOne(u => u.PaymentPlan)
            .HasForeignKey(u => u.PaymentPlanId);

        modelBuilder.Entity<PaymentPlan>()
            .HasMany(pp => pp.Organizations)
            .WithOne(o => o.PaymentPlan)
            .HasForeignKey(o => o.PaymentPlanId);

        // For enums
        modelBuilder.Entity<Secret>().Property(s => s.Type).HasConversion<string>();
        modelBuilder.Entity<SharedSecret>().Property(ss => ss.AccessLevel).HasConversion<string>();
        modelBuilder.Entity<Collection>().Property(sf => sf.Type).HasConversion<string>();
        modelBuilder.Entity<PaymentPlan>().Property(pp => pp.Type).HasConversion<string>();
        modelBuilder.Entity<Payment>().Property(p => p.Mode).HasConversion<string>();
        modelBuilder.Entity<Payment>().Property(p => p.Status).HasConversion<string>();
    }
}