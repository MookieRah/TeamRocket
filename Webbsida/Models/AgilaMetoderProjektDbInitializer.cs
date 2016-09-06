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
        private const string PathToEventImages = @"\Content\EventImages\";
        private readonly RandomGenerator _randomGenerator = new RandomGenerator();
        private readonly Random _random = new Random();


        private UserStore<ApplicationUser> _userStore;
        private UserManager<ApplicationUser> _userManager;


        protected override void Seed(ApplicationDbContext context)
        {
            //Setup userstore&manager
            _userStore = new UserStore<ApplicationUser>(context);
            _userManager = new UserManager<ApplicationUser>(_userStore);


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

            // ADMINUSER
            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Profile = new Profile()
                    {
                        DateOfBirth = GenerateRandomBirthDate(),
                        FirstName = "Admin",
                        LastName = "Adminsson",
                        IsPrivate = true
                    },
                    PhoneNumber = GenerateRandomPhoneNr(),
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
                    Profile = new Profile()
                    {
                        DateOfBirth = GenerateRandomBirthDate(),
                        FirstName = "User",
                        LastName = "Usersson",
                        IsPrivate = true
                    },
                    PhoneNumber = GenerateRandomPhoneNr(),
                };

                _userManager.Create(user, "password");
                _userManager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "Chris@wizzerd.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "Chris@wizzerd.com",
                    Email = "Chris@wizzerd.com",
                    Profile = new Profile()
                    {
                        DateOfBirth = GenerateRandomBirthDate(),
                        FirstName = "Christopher",
                        LastName = "Guest",
                        IsPrivate = true
                    },
                    PhoneNumber = GenerateRandomPhoneNr(),
                };

                _userManager.Create(user, "123123");
                _userManager.AddToRole(user.Id, "User");
            }

            
            // JoinUser
            if (!context.Users.Any(u => u.UserName == "joiner@joiner.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "joiner@joiner.com",
                    Email = "joiner@joiner.com",
                    Profile = new Profile()
                    {
                        DateOfBirth = GenerateRandomBirthDate(),
                        FirstName = "Joiner",
                        LastName = "Joinersson",
                        IsPrivate = true
                    },
                    PhoneNumber = GenerateRandomPhoneNr(),
                };

                _userManager.Create(user, "password");
                _userManager.AddToRole(user.Id, "User");
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
                new Tag {Name = "festival"},
                new Tag {Name ="rollspel" },
                new Tag {Name ="bord spel" },
                new Tag {Name ="kvällspel" },
                new Tag {Name ="öl" },
                new Tag {Name ="tärningar" },
                new Tag {Name ="lanparty" },
                new Tag {Name ="datorspel" },
                new Tag {Name ="spel" },
                new Tag {Name ="party" },
            };

            foreach (var tag in tags)
                context.Tags.Add(tag);

            context.SaveChanges();


            // Events
            var ec = new Event
            {
                Name = "Hundpromenad",
                Description = "Vi träffas utanför Åhlens och går runt och tittar på olika konstverk",
                StartDate = new DateTime(2016, 09, 10, 12, 00, 00),
                EndDate = new DateTime(2016, 09, 10, 16, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 20,
                MinSignups = 5,
                Price = 0,
                ImagePath = PathToEventImages + "hund.png",
            };
            ec.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "hund"),
                    Event = ec,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "djur"),
                    Event = ec,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "gratis"),
                    Event = ec,
                }
            };

            var ec1 = new Event
            {
                Name = "Svampplockning",
                Description = "Vi träffas vid mariehemscentrum och åker ett par mil utanför Umeå",
                StartDate = new DateTime(2016, 09, 10, 09, 00, 00),
                EndDate = new DateTime(2016, 09, 10, 16, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 30,
                MinSignups = 5,
                Price = 40,
                ImagePath = PathToEventImages + "images.png"
            };
            ec1.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "friluftsliv"),
                    Event = ec1,
                },
            };

            var ec2 = new Event
            {
                Name = "Lunchföreläsning",
                Description = "Vi träffas på vårt kontor mitt i stan på Skolgatan 38",
                StartDate = new DateTime(2016, 09, 12, 12, 00, 00),
                EndDate = new DateTime(2016, 09, 12, 13, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 50,
                MinSignups = 20,
                Price = 40,
                ImagePath = PathToEventImages + "lunch.png"
            };
            ec2.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "mat"),
                    Event = ec2,
                },
            };

            var ec3 = new Event
            {
                Name = "Dockteater",
                Description = "Vi på Norrlandsoperan kommmer att hålla dockteatrar alla torsdagskvällar under oktober månad, med start den 6/10 kl 19:00 ",
                StartDate = new DateTime(2016, 10, 6, 19, 00, 00),
                EndDate = new DateTime(2016, 10, 6, 20, 30, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 500,
                MinSignups = 100,
                Price = 50,
                ImagePath = PathToEventImages + "dockteater.png"
            };
            ec3.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "barn"),
                    Event = ec3,
                },
            };

            var ec4 = new Event
            {
                Name = "Företagsjippo",
                Description = "Företaget Umeå intresseklubbs företagsjippo kommer i år att hålla till på Norrbyskär, där vi kommer att ha både tält och karuseller och det kommer i år att vara öppen för allmänheten. Vi kommer både att ha karuseller och hoppborg för de yngre.",
                StartDate = new DateTime(2016, 09, 17, 11, 00, 00),
                EndDate = new DateTime(2016, 09, 17, 17, 30, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 350,
                MinSignups = 35,
                Price = 20,
                ImagePath = PathToEventImages + "Karusell.png"
            };
            ec4.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "barn"),
                    Event = ec4,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "tonåring"),
                    Event = ec4,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "vuxen"),
                    Event = ec4,
                },
            };

            var ec5 = new Event
            {
                Name = "Företagspresentation",
                Description = "Vårt företag, Svenska metaller och plaster, kommer att befinna oss på arbetsförmedlingen under veckosluten för att presentera oss och för att intresserade ska kunna komma och prata med oss. Vi söker speciellt ingenjörer som är utbildade inom plast och metallindustrin.",
                StartDate = new DateTime(2016, 09, 23, 18, 00, 00),
                EndDate = new DateTime(2016, 09, 25, 12, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 500,
                MinSignups = 35,
                Price = 0,
                ImagePath = PathToEventImages + "fore-img.png"
            };
            ec5.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "vuxen"),
                    Event = ec5,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "gratis"),
                    Event = ec5,
                },
            };

            var ec6 = new Event
            {
                Name = "Älgkontakt",
                Description = "Vårt företag, Älgens hus, vill att barn och ungdomar ska få en närmare tillvaron till naturen så vi kommer att hålla helgförmiddagarna öppna med både besök i hagen och testa på med workshops med utbildade ledare och naturkännare. Kom på en dag för att lära dig mer om den svenska naturen.   ",
                StartDate = new DateTime(2016, 10, 1, 09, 00, 00),
                EndDate = new DateTime(2016, 10, 1, 12, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 30,
                MinSignups = 10,
                Price = 40,
                ImagePath = PathToEventImages + "älghus.png"
            };
            ec6.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "djur"),
                    Event = ec6,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "barn"),
                    Event = ec6,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "tonåring"),
                    Event = ec6,
                },
            };

            var ec7 = new Event
            {
                Name = "Språkskola",
                Description = "Vårt kompisgäng har ansökt om att lära oss spanska, men men det behövs minst 1 till, så någon som är manat att lära sig spanska får gärna ansluta sig till oss. Då vi studerar på umu, så ser vi helst att kursen går på kvällen, och då medelåldern är 25 år, så ser vi gärna att din ålder är i närheten av detta.   ",
                StartDate = new DateTime(2017, 01, 9, 09, 00, 00),
                EndDate = new DateTime(2017, 01, 9, 12, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 1,
                MinSignups = 1,
                Price = 550,
                ImagePath = PathToEventImages + "sprak.png"
            };
            ec7.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "vuxen"),
                    Event = ec7,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "nya vänner"),
                    Event = ec7,
                },
            };

            var ec8 = new Event
            {
                Name = "pubafton",
                Description = "Jag är ensam, men lust att festa loss i helgen, är ni några som vill festa loss med mig, så träffas vi på Allstar kl 21:00. ",
                StartDate = new DateTime(2017, 01, 9, 09, 00, 00),
                EndDate = new DateTime(2017, 01, 9, 12, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 10,
                MinSignups = 1,
                Price = 0,
                ImagePath = PathToEventImages + "party.png"
            };
            ec8.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "vuxen"),
                    Event = ec8,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "nya vänner"),
                    Event = ec8,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "fest"),
                    Event = ec8,
                },
            };

            var ec9 = new Event
            {
                Name = "Picknink med allsång",
                Description = "Vi kommer att hålla igång med allsång varje måndag kväll på vänortsparken, det är bara att dyka upp med en picknickkorg och sjunga med oss.",
                StartDate = new DateTime(2016, 09, 12, 18, 00, 00),
                EndDate = new DateTime(2067, 09, 12, 20, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 85,
                MinSignups = 25,
                Price = 0,
                ImagePath = PathToEventImages + "allsång.png"
            };
            ec9.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "mat"),
                    Event = ec9,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "festival"),
                    Event = ec9,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "musik"),
                    Event = ec9,
                },
            };

            var ec10 = new Event
            {
                Name = "Istid för barnen",
                Description = "Dagen innan säsongen börjar kommer det vara istid för barnen, där barn i åldrarna 3-7 år får komma ut på isen och lära sig åka skridskor, vi kommer även sponsra med besök av några av våra a-lagspelare. som visar sina konster.",
                StartDate = new DateTime(2016, 09, 14, 10, 00, 00),
                EndDate = new DateTime(2067, 09, 14, 10, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 50,
                MinSignups = 15,
                Price = 0,
                ImagePath = PathToEventImages + "björklöven.png"
            };
            ec10.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "barn"),
                    Event = ec10,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "nya vänner"),
                    Event = ec10,
                },
            };

            var ec11 = new Event
            {
                Name = "halltid för barnen",
                Description = "Dagen innan säsonen börjar kommer det vara full ös i Noliahallarna av barn mellan 3-7 år som får komma och lära sig olika bollsporter.",
                StartDate = new DateTime(2016, 09, 21, 10, 00, 00),
                EndDate = new DateTime(2067, 09, 21, 10, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 70,
                MinSignups = 25,
                Price = 0,
                ImagePath = PathToEventImages + "nolia.png"
            };
            ec11.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "barn"),
                    Event = ec11,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "nya vänner"),
                    Event = ec11,
                },
            };

            var ec12 = new Event
            {
                Name = "Rollspels kväll",
                Description = "Jag ska hålla en Rollspels sektion, men jag behöver andra spellare, skal hållas i en uthyres lokal i Umeå, vill hälst att vi kan vara med minst en gång i väkan",
                StartDate = new DateTime(2016, 08, 14, 10, 00, 00),
                EndDate = new DateTime(2016, 08, 14, 10, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 4,
                MinSignups = 4,
                Price = 0,
                ImagePath = PathToEventImages +"Five_ivory_dice.jpg"
            };
            ec12.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "rollspel"),
                    Event = ec12,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "bord spel"),
                    Event = ec12,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "kvällspel"),
                    Event = ec12,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "öl"),
                    Event = ec12,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "tärningar"),
                    Event = ec12,
                },
            };
            var ec13 = new Event
            {
                Name = "Lanparty",
                Description = "Har hyrt en sall i umeå, och nu ska ha en Lan party där alla får komma, det ända du måste ta med dig är en PC eller en spelconsol ",
                StartDate = new DateTime(2016, 08, 14, 10, 00, 00),
                EndDate = new DateTime(2016, 08, 16, 10, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                Price = 0,
                ImagePath = PathToEventImages +"lanparty.png"
            };
            ec13.EventTags = new List<EventTag>()
            {
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "kvällspel"),
                    Event = ec13,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "öl"),
                    Event = ec13,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "lanparty"),
                    Event = ec13,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "datorspel"),
                    Event = ec13,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "spel"),
                    Event = ec13,
                },
                new EventTag()
                {
                    Tag = tags.First(n => n.Name == "party"),
                    Event = ec13,
                },

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
                ec11,
                ec12,
                ec13
            };
            context.Events.AddRange(listOfEvents);



            // EventProfiles
            var eventUsers = new List<EventUser>
            {
                new EventUser()
                {
                    Event = ec,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec,
                    Profile = context.Profiles.First(n => n.FirstName == "Joiner"),
                    IsOwner = false
                },


                new EventUser()
                {
                    Event = ec1,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec1,
                    Profile = context.Profiles.First(n => n.FirstName == "Joiner"),
                    IsOwner = false
                },
                new EventUser()
                {
                    Event = ec1,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = false
                },


                new EventUser()
                {
                    Event = ec2,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec2,
                    Profile = context.Profiles.First(n => n.FirstName == "Joiner"),
                    IsOwner = false
                },


                new EventUser()
                {
                    Event = ec3,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec3,
                    Profile = context.Profiles.First(n => n.FirstName == "Joiner"),
                    IsOwner = false
                },

                new EventUser()
                {
                    Event = ec4,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec4,
                    Profile = context.Profiles.First(n => n.FirstName == "Joiner"),
                    IsOwner = false
                },


                new EventUser()
                {
                    Event = ec5,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec6,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec7,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec8,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec9,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec10,
                    Profile = context.Profiles.First(n => n.FirstName == "Admin"),
                    IsOwner = true
                },


                new EventUser()
                {
                    Event = ec11,
                    Profile = context.Profiles.First(n => n.FirstName == "User"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec12,
                    Profile = context.Profiles.First(n => n.FirstName == "Christopher"),
                    IsOwner = true
                },
                new EventUser()
                {
                    Event = ec13,
                    Profile = context.Profiles.First(n => n.FirstName == "Christopher"),
                    IsOwner = true
                },

            };


            context.EventUsers.AddRange(eventUsers);
            context.SaveChanges();


            // Ger information till Kontakta oss sidan
            var companyInfo = new CompanyInformation
            {
                Address = "Tvistevägen 48",
                Description = "Vi är ett företag som arbetar för att människor ska kunna knyta nya kontakter via egengjorda event. ",
                Phonenumber = "090.232424",
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude()
            };

            context.CompanyInformations.Add(companyInfo);
            context.SaveChanges();


            base.Seed(context);
        }

        private string GenerateRandomPhoneNr()
        {
            return "070" + _random.Next(0, 10) + _random.Next(0, 10) + _random.Next(0, 10) + _random.Next(0, 10) +
                   _random.Next(0, 10) + _random.Next(0, 10) + _random.Next(0, 10);
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
            return Math.Round(63f + _random.Next(710, 901) / 1000f, 3);
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