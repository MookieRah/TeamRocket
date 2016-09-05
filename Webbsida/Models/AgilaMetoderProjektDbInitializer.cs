using System;
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
using System.Collections;

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
            // Ger information till Kontakta oss sidan
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

            // Ger information till olika events.

            var ec = new Event {
                Name = "Hubdpromenad",
                Description = "Vi träffas utanför Ålens och går runt och tittar på olika konstverk",
                StartDate = new DateTime(2016, 09, 10, 12, 00, 00),
                EndDate = new DateTime(2016, 09, 10, 16, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 20,
                MinSignups = 5,
                Price = 0,
                ImagePath ="Content/Eventimages/hund.png" 
            };
            var ec1 =  new Event
            {
                Name = "Svampplockning",
                Description = "Vi träffas på mariehemscentrum och åker ett par mil utanför Umeå",
                StartDate = new DateTime(2016, 09, 10, 09, 00, 00),
                EndDate = new DateTime(2016, 09, 10, 16, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 30,
                MinSignups = 5,
                Price = 40,
                ImagePath = "Content/Eventimages/images.png"
            };
            var ec2 = new Event
            {
                Name = "Lunchföreläsning",
                Description = "Vi träffas på vårt kontor mitt i stan på Skolgatan 38",
                StartDate = new DateTime(2016, 09, 12, 12, 00, 00),
                EndDate = new DateTime(2016, 09, 12, 13, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 50,
                MinSignups = 20,
                Price = 40,
                ImagePath = "Content/Eventimages/lunch.png"
            };
            var ec3 = new Event
            {
                Name = "Dockteater",
                Description = "Vi på Norrlandsooperan kommmer att hålla dockteatrar alla torsdagskvöllar under oktober månad, med start den 6/10 kl 19:00 ",
                StartDate = new DateTime(2016, 10, 6, 19, 00, 00),
                EndDate = new DateTime(2016, 10, 6, 20, 30, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 500,
                MinSignups = 100,
                Price = 50,
                ImagePath = "Content/Eventimages/dockteater.png"
            };
            var ec4 = new Event
            {
                Name = "Företagsjippo",
                Description = "Företaget Umeå intresseklubbs företagsjippo kommer i år att hålla till på Norrbyskör, där vi  kommer att ha både tält och karuseller och det kommer i år att vara öppen för allmänheten Vi kommer både att ha karuseller och hoppborg för de yngre.",
                StartDate = new DateTime(2016, 09, 17, 11, 00, 00),
                EndDate = new DateTime(2016, 09, 17, 17, 30, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 350,
                MinSignups = 35,
                Price = 20,
                ImagePath = "Content/Eventimages/Karusell.png"
            };
            var ec5 = new Event
            {
                Name = "Företagspresentation",
                Description = "Vårt företag, Svenska metaller och plaster, kommer att befinna oss på arbetsförmedlingen under veckosluten för att presentera oss och för att intresserade ska kunna komma och prata med oss, vi söker speciellt ingenjörer som är utbildade inom plast och metallindustrin.  ",
                StartDate = new DateTime(2016, 09, 23, 18, 00, 00),
                EndDate = new DateTime(2016, 09, 25, 12, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 500,
                MinSignups = 35,
                Price = 0,
                ImagePath = "Content/Eventimages/fore-img.png"
            };
            var ec6 = new Event
            {
                Name = "Älgkontakt",
                Description = "Vårt företag, Älgens hus, vill att barn och ungdomar ska få en närmare tillvaron i naturen så vi kommer att hålla helgförmiddagarna öppna med både besök i hagen och testa på med workshops med utbildade ledare och naturkännare, kom på en dag för att lära dig mer om den svenska naturen.   ",
                StartDate = new DateTime(2016, 10, 1, 09, 00, 00),
                EndDate = new DateTime(2016, 10, 1, 12, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 30,
                MinSignups = 10,
                Price = 40,
                ImagePath = "Content/Eventimages/älghus.png"
            };
            var ec7 = new Event
            {
                Name = "Språkskola",
                Description = "Vårt kompisgäng har ansökt om att lära oss engelska, men men det behövs minst 1n till så någon som är manat att läsa engelska får gärna anslta sig till oss. Då vi studerar på umu, så ser vi helst att kursen går på kvällen, och då medelåldern är 25 år, så ser vi gärna att din ålder är i närheten av denna.   ",
                StartDate = new DateTime(2017, 01, 9, 09, 00, 00),
                EndDate = new DateTime(2017, 01, 9, 12, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 1,
                MinSignups = 1,
                Price = 550,
                ImagePath = "Content/Eventimages/sprak.png"
            };
            var ec8 = new Event
            {
                Name = "pubafton",
                Description = "Jag är ensam, men lust att fsta loss i helgen, är ni några som vill festa loss med mig, så träffas vi på Allstar kl 21:00. ",
                StartDate = new DateTime(2017, 01, 9, 09, 00, 00),
                EndDate = new DateTime(2017, 01, 9, 12, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 10,
                MinSignups = 1,
                Price = 0,
                ImagePath = "Content/Eventimages/party.png"
            };
            var ec9 = new Event
            {
                Name = "Picknink med allsång",
                Description = "Vi kommer att hålla igång med allsång varje månddag kväll på vänortsparken, det är bara att dyka uppmed en picknickkorg och sjunga med oss.",
                StartDate = new DateTime(2016, 09, 12, 18, 00, 00),
                EndDate = new DateTime(2067, 09, 12, 20, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 85,
                MinSignups = 25,
                Price = 0,
                ImagePath = "Content/Eventimages/allsång.png"
            };
            var ec10 = new Event
            {
                Name = "Istid för barnen",
                Description = "Dagen innan säsonen börjar kommer det vara istid för barnen, där barn i åldrarna 3-7 år får komma ut på isen och lära sig åka skridskor, vi kommer även sponsra med besök av några av våra a-lagspelare. som visar sina konster.",
                StartDate = new DateTime(2016, 09, 14, 10, 00, 00),
                EndDate = new DateTime(2067, 09, 14, 10, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 50,
                MinSignups = 15,
                Price = 0,
                ImagePath = "Content/Eventimages/björklöven.png"
            };

            var ec11 = new Event
            {
                Name = "halltid för barnen",
                Description = "Dagen innan säsonen börjar kommer det vara istid för barnen, där barn i åldrarna 3-7 år får komma ut på isen och lära sig åka skridskor, vi kommer även sponsra med besök av några av våra a-lagspelare. som visar sina konster.",
                StartDate = new DateTime(2016, 09, 21, 10, 00, 00),
                EndDate = new DateTime(2067, 09, 21, 10, 00, 00),
                Latitude = 20.344535345,
                Longitude = 60.345345534,
                MaxSignups = 70,
                MinSignups = 25,
                Price = 0,
                ImagePath = "Content/Eventimages/nolia.png"
            };

            var listOfEvents = new List<Event>{
                ec,
                ec1,
                ec2,
                ec3,
                ec4,
                ec5,
                ec6,
                ec7,
                ec8,
                ec9,
                ec10,
                ec11
            };
            context.Events.AddRange(listOfEvents);
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