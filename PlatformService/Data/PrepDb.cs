using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb // static class doesn't need an instance to run
    {
        // pass the IApplicationBuilder interface
        public static void PrepPolution(IApplicationBuilder app)
        {
            // get the AppDbContext from the app
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            // check if there are any platforms
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data...");

                // add the platforms
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" }, // we don't need to specify the Id because it's auto-generated
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" }, // we are creating a new instance of the Platform class
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

                // save the changes
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
    }

    }
}