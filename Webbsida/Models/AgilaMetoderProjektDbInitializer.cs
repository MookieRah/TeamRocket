using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DatabaseObjects;
using FizzWare.NBuilder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Webbsida.Models
{
    //
    // -> Use DropCreateDatabaseIfModelChanges if you want db-data to persist between builds!
    //

    //public class AgilaMetoderProjektDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    public class AgilaMetoderProjektDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        private readonly RandomGenerator _randomGenerator = new RandomGenerator();
        private readonly Random _random = new Random();

        protected override void Seed(ApplicationDbContext context)
        {
            // USERS AND ROLES
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole(roleName));
                }
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

                manager.Create(user, "password");
                manager.AddToRole(user.Id, "Admin");
            }

            if (!(context.Users.Any(u => u.UserName == "user@user.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    UserName = "user@user.com",
                    Email = "user@user.com",
                    //TODO: profile-connection...
                    //Profile = xxx
                };
                userManager.Create(userToInsert, "password");
                userManager.AddToRole(userToInsert.Id, "User");
            }

            context.SaveChanges();


            // Profiles
            var profiles = Builder<Profile>.CreateListOfSize(20)
                .All()
                    .With(n => n.FirstName = Faker.Name.First())
                    .With(n => n.LastName = Faker.Name.Last())
                    .With(n => n.DateOfBirth = GenerateRandomBirthDate())
                    .With(n => n.IsPrivate = (Faker.RandomNumber.Next(0, 5) > 1))
                //.Random(15)
                //.With(n => n.FirstName = Faker.Name.First())
                .Build();

            foreach (var profile in profiles)
                context.Profiles.Add(profile);
            context.SaveChanges();


            // Events
            var events = Builder<Event>.CreateListOfSize(10)
                .All()
                    .With(n => n.Description = Faker.Lorem.Paragraph())
                    //.With(n => n.GetOwner = Pick<Profile>.RandomItemFrom(profiles))    //.Where(a => a.IsPrivate == true).ToList()
                    .With(n => n.Latitude = GenerateRandomLatitude())
                    .With(n => n.Longitude = GenerateRandomLongitude())
                    .With(n => n.MinSignups = _random.Next(0, 4))
                    .With(n => n.MaxSignups = _random.Next(1, 100))
                    .With(n => n.Price = GenerateRandomPrice())
                .Build();

            foreach (var @event in events)
                context.Events.Add(@event);
            context.SaveChanges();


            // EventProfiles
            var eventUsers = Builder<EventUser>.CreateListOfSize(20)

                .All()
                    .With(n => n.Profile = Pick<Profile>.RandomItemFrom(profiles))
                    .With(n => n.Event = Pick<Event>.RandomItemFrom(events))
                    .With(n => n.Status = (Faker.RandomNumber.Next(0, 2) == 1) ? "Pending" : "Confirmed")
                .Build();

            foreach (var @event in events)
            {
                var randomEventUser = eventUsers.ElementAt(_random.Next(0, eventUsers.Count));

                while (randomEventUser.EventId != @event.Id)
                {
                    randomEventUser = eventUsers.ElementAt(_random.Next(0, eventUsers.Count));
                }

                randomEventUser.IsOwner = true;
            }

            foreach (var eventUser in eventUsers)
                context.EventUsers.Add(eventUser);
            context.SaveChanges();


            base.Seed(context);
        }

        private decimal? GenerateRandomPrice()
        {
            if (_random.Next(0, 3) == 0)
                return _random.Next(10, 1000);

            return null;
        }

        private double GenerateRandomLatitude()
        {
            //63.000-64.000
            return Math.Round(63f + _random.NextDouble(), 3);
        }

        private double GenerateRandomLongitude()
        {
            //20.000-21.000
            return Math.Round(21f + _random.NextDouble(), 3);
        }

        private DateTime GenerateRandomBirthDate()
        {
            var randomYear = DateTime.Now.AddYears(-_randomGenerator.Next(10, 100));
            var randomYearAndDay = randomYear.AddDays(-_randomGenerator.Next(1, 365));

            return randomYearAndDay;
        }
    }
}