using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using myapp.auth.Models;
using System.Collections.Generic;
using System.Linq;

namespace myapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanFeature> PlanFeatures { get; set; }
        public DbSet<PlanOption> PlanOption { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlanOption>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configure owned types for Profile
            modelBuilder.Entity<Profile>(profile =>
            {
                profile.OwnsMany(p => p.Certificates, a =>
                {
                    a.WithOwner().HasForeignKey("ProfileId");
                    a.Property<int>("Id");
                    a.HasKey("Id");
                });
                profile.OwnsMany(p => p.TrainingHistory, a =>
                {
                    a.WithOwner().HasForeignKey("ProfileId");
                    a.Property<int>("Id");
                    a.HasKey("Id");
                });

                // Store Achievements as a delimited string with value comparer
                var achievementsConverter = new ValueConverter<List<string>, string>(
                    v => v != null ? string.Join(';', v) : null,
                    v => v != null ? new List<string>(v.Split(';', System.StringSplitOptions.RemoveEmptyEntries)) : new List<string>()
                );
                var achievementsComparer = new ValueComparer<List<string>>(
                    (c1, c2) => c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
                    c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                    c => c != null ? c.ToList() : new List<string>()
                );

                profile.Property(p => p.Achievements)
                    .HasConversion(achievementsConverter)
                    .Metadata.SetValueComparer(achievementsComparer);
            });

            // Seed Plans
            modelBuilder.Entity<Plan>().HasData(
                new Plan
                {
                    Id = 1,
                    Name = "Get Basic",
                    Description = "Remove Add & Unlock All Location",
                    Icon = "1"
                },
                new Plan
                {
                    Id = 2,
                    Name = "Get Premium",
                    Description = "Remove Add & Unlock All Location",
                    Icon = "crown"
                },
                new Plan
                {
                    Id = 3,
                    Name = "Get Basic",
                    Description = "Remove Add & Unlock All Location",
                    Icon = "2"
                }
            );

            // Seed PlanOptions
            modelBuilder.Entity<PlanOption>().HasData(
                // Plan 1 Options
                new PlanOption { Id = 1, PlanId = 1, DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 2, PlanId = 1, DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 3, PlanId = 1, DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan", PriceUnit = "EGP" },
                // Plan 2 Options
                new PlanOption { Id = 4, PlanId = 2, DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 5, PlanId = 2, DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 6, PlanId = 2, DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan", PriceUnit = "EGP" },
                // Plan 3 Options
                new PlanOption { Id = 7, PlanId = 3, DurationMonths = 1, Label = "1 Month", Price = 0, Note = "that's Basic Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 8, PlanId = 3, DurationMonths = 6, Label = "6 Month", Price = 310, Note = "that's Premium Plan", PriceUnit = "EGP" },
                new PlanOption { Id = 9, PlanId = 3, DurationMonths = 12, Label = "1 Year", Price = 620, Note = "that's Premium Plan", PriceUnit = "EGP" }
            );

            // Seed PlanFeatures
            modelBuilder.Entity<PlanFeature>().HasData(
                // Plan 1 Features
                new PlanFeature { Id = 1, PlanOptionId = 1, Description = "Limited VR Training" },
                new PlanFeature { Id = 2, PlanOptionId = 1, Description = "Access to Home Scenario" },
                new PlanFeature { Id = 3, PlanOptionId = 4, Description = "Limited VR Training" },
                new PlanFeature { Id = 4, PlanOptionId = 4, Description = "Access to Home Scenario" },
                new PlanFeature { Id = 5, PlanOptionId = 7, Description = "Limited VR Training" },
                new PlanFeature { Id = 6, PlanOptionId = 7, Description = "Access to Home Scenario" },
                // Plan 2 Features
                new PlanFeature { Id = 7, PlanOptionId = 2, Description = "AI Chatbot Support" },
                new PlanFeature { Id = 8, PlanOptionId = 2, Description = "Home & Factory Scenarios" },
                new PlanFeature { Id = 9, PlanOptionId = 2, Description = "Full VR Training Access" },
                new PlanFeature { Id = 10, PlanOptionId = 2, Description = "Certification" },
                new PlanFeature { Id = 11, PlanOptionId = 5, Description = "AI Chatbot Support" },
                new PlanFeature { Id = 12, PlanOptionId = 5, Description = "Home & Factory Scenarios" },
                new PlanFeature { Id = 13, PlanOptionId = 5, Description = "Full VR Training Access" },
                new PlanFeature { Id = 14, PlanOptionId = 5, Description = "Certification" },
                new PlanFeature { Id = 15, PlanOptionId = 8, Description = "AI Chatbot Support" },
                new PlanFeature { Id = 16, PlanOptionId = 8, Description = "Home & Factory Scenarios" },
                new PlanFeature { Id = 17, PlanOptionId = 8, Description = "Full VR Training Access" },
                new PlanFeature { Id = 18, PlanOptionId = 8, Description = "Certification" },
                // Plan 3 Features
                new PlanFeature { Id = 19, PlanOptionId = 3, Description = "Unlimited VR Training" },
                new PlanFeature { Id = 20, PlanOptionId = 3, Description = "All Scenarios (Home, Factory, Vehicle)" },
                new PlanFeature { Id = 21, PlanOptionId = 3, Description = "AI Chatbot + Burn Detection" },
                new PlanFeature { Id = 22, PlanOptionId = 3, Description = "Multi-User & Custom Reports" },
                new PlanFeature { Id = 23, PlanOptionId = 6, Description = "Unlimited VR Training" },
                new PlanFeature { Id = 24, PlanOptionId = 6, Description = "All Scenarios (Home, Factory, Vehicle)" },
                new PlanFeature { Id = 25, PlanOptionId = 6, Description = "AI Chatbot + Burn Detection" },
                new PlanFeature { Id = 26, PlanOptionId = 6, Description = "Multi-User & Custom Reports" },
                new PlanFeature { Id = 27, PlanOptionId = 9, Description = "Unlimited VR Training" },
                new PlanFeature { Id = 28, PlanOptionId = 9, Description = "All Scenarios (Home, Factory, Vehicle)" },
                new PlanFeature { Id = 29, PlanOptionId = 9, Description = "AI Chatbot + Burn Detection" },
                new PlanFeature { Id = 30, PlanOptionId = 9, Description = "Multi-User & Custom Reports" }
            );
        }
    }
}
