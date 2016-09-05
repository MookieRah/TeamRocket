﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
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

        private UserStore<ApplicationUser> _userStore;
        private UserManager<ApplicationUser> _userManager;


        protected override void Seed(ApplicationDbContext context)
        {
            //Setup userstore&manager
            _userStore = new UserStore<ApplicationUser>(context);
            _userManager = new UserManager<ApplicationUser>(_userStore);


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


            // ROLES
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole(roleName));
                }
            }


            var uniqueUserList = profiles.ToArray();

            // ADMINUSER
            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Profile = uniqueUserList[0],
                    PhoneNumber = Faker.Phone.Number(),
                    //Pick<Profile>.RandomItemFrom(profiles)
                };

                _userManager.Create(user, "password");
                _userManager.AddToRole(user.Id, "Admin");
            }

            // USERUSER
            if (!context.Users.Any(u => u.UserName == "user@user.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "user@user.com",
                    Email = "user@user.com",
                    Profile = uniqueUserList[1],
                    PhoneNumber = Faker.Phone.Number(),
                };

                _userManager.Create(user, "password");
                _userManager.AddToRole(user.Id, "User");
            }


            // THE REST OF THE USERS
            for (int index = 2; index < uniqueUserList.Length; index++)
            {
                var profile = uniqueUserList[index];
                var tempEmail = Faker.Internet.Email();
                var userToInsert = new ApplicationUser
                {
                    UserName = tempEmail,
                    Email = tempEmail,
                    Profile = profile,
                    PhoneNumber = Faker.Phone.Number(),
                };
                _userManager.Create(userToInsert, "password");
                _userManager.AddToRole(userToInsert.Id, "User");
            }
            context.SaveChanges();


            //Tags
            var tags = new List<Tag>
            {
                new Tag {Name = "gratis"},
                new Tag {Name = "schack"},
                new Tag {Name = "hund"},
                new Tag {Name = "djur"},
                new Tag {Name = "fest"},
                new Tag {Name = "barn"},
                new Tag {Name = "vuxen"},
                new Tag {Name = "tonåring"},
                new Tag {Name = "musik"},
                new Tag {Name = "mat"},
                new Tag {Name = "friluftsliv"},
                new Tag {Name = "dator"},
                new Tag {Name = "nya vänner"},
                new Tag {Name = "festival"}
            };

            foreach (var tag in tags)
            {
                context.Tags.Add(tag);
            }
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

            //EventTags
            var eventTags = Builder<EventTag>.CreateListOfSize(20)

                .All()
                    .With(n => n.Tag = Pick<Tag>.RandomItemFrom(tags))
                    .With(n => n.Event = Pick<Event>.RandomItemFrom(events))
                .Build();

            foreach (var eventTag in eventTags)
                context.EventTags.Add(eventTag);
            context.SaveChanges();

            // EventProfiles
            var eventUsers = Builder<EventUser>.CreateListOfSize(200)

                .All()
                    .With(n => n.Profile = Pick<Profile>.RandomItemFrom(profiles))
                    .With(n => n.Event = Pick<Event>.RandomItemFrom(events))
                    .With(n => n.Status = (Faker.RandomNumber.Next(0, 2) == 1) ? "Pending" : "Confirmed")
                .Build();

            var pathToFile = @"\Content\EventImages\"; 

            var dummyImages = new List<string>
            {
                pathToFile + "pic1.jpg",
                pathToFile + "pic2.jpg",
                pathToFile + "pic3.jpg",
                pathToFile + "pic4.jpg",
                pathToFile + "pic5.jpg",
                pathToFile + "pic6.jpg",
                pathToFile + "pic7.jpg",
                pathToFile + "pic8.jpg",
                pathToFile + "pic9.jpg",
                pathToFile + "pic10.jpg",
                pathToFile + "pic11.jpg",
                pathToFile + "pic12.jpg",
                pathToFile + "pic13.jpg",
                pathToFile + "pic14.jpg",
                pathToFile + "pic15.jpg",
                pathToFile + "pic16.jpg",
                pathToFile + "pic17.jpg",
                pathToFile + "pic18.jpg",
                pathToFile + "pic19.jpg",
                pathToFile + "png1.png",
                pathToFile + "png2.png"
            };

            //var gratis = context.Tags.FirstOrDefault(x => x.Name == "gratis");

            foreach (var @event in events)
            {
                var randomEventUser = eventUsers.ElementAt(_random.Next(0, eventUsers.Count));

                while (randomEventUser.EventId != @event.Id)
                {
                    randomEventUser = eventUsers.ElementAt(_random.Next(0, eventUsers.Count));
                }

                randomEventUser.IsOwner = true;

                @event.ImagePath = dummyImages.ElementAt(_random.Next(0, dummyImages.Count()));
            }

            //foreach (var @event in events)
            //{
            //    @event.EventTags.Add(new EventTag
            //    {
            //        Event = @event,
            //        Tag = tags.FirstOrDefault(x => x.Name == "test")
            //    });
            //}

            foreach (var eventUser in eventUsers)
                context.EventUsers.Add(eventUser);
            context.SaveChanges();

            var companyInfo = new CompanyInformation
            {
                Address = "Tvistevägen 48",
                Description = "Vi är ett företag som arbetar för att människor ska kunna knyta nya kontakter via egengjorda event. ",
                Phonenumber = "090.232424",
                Latitude = 63.817444,
                Longitude = 20.316255
            };

            context.CompanyInformations.Add(companyInfo);
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
            //63.710-63.900
            return Math.Round(63f + _random.Next(710,901) / 1000f, 3);
        }

        private double GenerateRandomLongitude()
        {
            //20.000-20.550
            return Math.Round(20f + _random.Next(0, 551) / 1000f, 3);
        }

        private DateTime GenerateRandomBirthDate()
        {
            var randomYear = DateTime.Now.AddYears(-_randomGenerator.Next(10, 100));
            var randomYearAndDay = randomYear.AddDays(-_randomGenerator.Next(1, 365));

            return randomYearAndDay;
        }
    }
}