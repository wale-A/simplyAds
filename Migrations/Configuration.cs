namespace SimplyAds.Migrations
{
    using SimplyAds.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SimplyAds.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SimplyAds.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Car.AddOrUpdate(
                c => c.NumberOfCars,
                new Car { NumberOfCars = 1, Amount = 20000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new Car { NumberOfCars = 2, Amount = 39000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new Car { NumberOfCars = 3, Amount = 58000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new Car { NumberOfCars = 5, Amount = 96000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new Car { NumberOfCars = 10, Amount = 1800000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" }
                //new Car { NumberOfCars = 1, Amount = 20000 },
            );

            context.DurationPricing.AddOrUpdate(
                d => d.Time,
                new DurationPricing { Time = "1 day", Amount = 3000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "3 days", Amount = 8500, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "5 days", Amount = 14000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "1 week", Amount = 20000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "2 weeks", Amount = 38000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "1 month", Amount = 75000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "3 months", Amount = 210000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "6 months", Amount = 400000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new DurationPricing { Time = "1 year", Amount = 1100000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" }
            );

            context.MiscChargePricing.AddOrUpdate(
                m => m.Property,
                new MiscChargePricing { Property = "Urgent", Amount = 15000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" }
            );

            context.ContentPricing.AddOrUpdate(
                c => c.Type,
                new ContentPricing { Type = "Video", Amount = 40000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" },
                new ContentPricing { Type = "Image", Amount = 20000, DateCreated = DateTime.UtcNow.AddHours(1), CreatedBy = "admin" }
            );
        }
    }
}
