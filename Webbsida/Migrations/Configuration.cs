using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DatabaseObjects;
using FizzWare.NBuilder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Webbsida.Models;

namespace Webbsida.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private const string PathToEventImages = @"\Content\EventImages\";
        private readonly
        RandomGenerator _randomGenerator = new RandomGenerator();
        private readonly
        Random _random = new Random();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Webbsida.Models.ApplicationDbContext";
        }

        private UserStore<ApplicationUser> _userStore;
        private UserManager<ApplicationUser> _userManager;

        protected override void Seed(Webbsida.Models.ApplicationDbContext context)
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
                    new Tag {Name = "ton�ring"},
                    new Tag {Name = "musik"},
                    new Tag {Name = "mat"},
                    new Tag {Name = "friluftsliv"},
                    new Tag {Name = "dator"},
                    new Tag {Name = "nya v�nner"},
                    new Tag {Name = "festival"}
                };

            foreach (var tag in tags)
                context.Tags.Add(tag);

            context.SaveChanges();


            // Events
            var ec = new Event
            {
                Name = "Hundpromenad",
                Description = "Vi tr�ffas utanf�r �hlens och g�r runt och tittar p� olika konstverk",
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
                Description = "Vi tr�ffas vid mariehemscentrum och �ker ett par mil utanf�r Ume�",
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
                Name = "Lunchf�rel�sning",
                Description = "Vi tr�ffas p� v�rt kontor mitt i stan p� Skolgatan 38",
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
                Description =
                    "Vi p� Norrlandsoperan kommmer att h�lla dockteatrar alla torsdagskv�llar under oktober m�nad, med start den 6/10 kl 19:00 ",
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
                Name = "F�retagsjippo",
                Description =
                    "F�retaget Ume� intresseklubbs f�retagsjippo kommer i �r att h�lla till p� Norrbysk�r, d�r vi kommer att ha b�de t�lt och karuseller och det kommer i �r att vara �ppen f�r allm�nheten. Vi kommer b�de att ha karuseller och hoppborg f�r de yngre.",
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
                        Tag = tags.First(n => n.Name == "ton�ring"),
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
                Name = "F�retagspresentation",
                Description =
                    "V�rt f�retag, Svenska metaller och plaster, kommer att befinna oss p� arbetsf�rmedlingen under veckosluten f�r att presentera oss och f�r att intresserade ska kunna komma och prata med oss. Vi s�ker speciellt ingenj�rer som �r utbildade inom plast och metallindustrin.",
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
                Name = "�lgkontakt",
                Description =
                    "V�rt f�retag, �lgens hus, vill att barn och ungdomar ska f� en n�rmare tillvaron till naturen s� vi kommer att h�lla helgf�rmiddagarna �ppna med b�de bes�k i hagen och testa p� med workshops med utbildade ledare och naturk�nnare. Kom p� en dag f�r att l�ra dig mer om den svenska naturen.   ",
                StartDate = new DateTime(2016, 10, 1, 09, 00, 00),
                EndDate = new DateTime(2016, 10, 1, 12, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 30,
                MinSignups = 10,
                Price = 40,
                ImagePath = PathToEventImages + "�lghus.png"
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
                        Tag = tags.First(n => n.Name == "ton�ring"),
                        Event = ec6,
                    },
                };

            var ec7 = new Event
            {
                Name = "Spr�kskola",
                Description =
                    "V�rt kompisg�ng har ans�kt om att l�ra oss spanska, men men det beh�vs minst 1 till, s� n�gon som �r manat att l�ra sig spanska f�r g�rna ansluta sig till oss. D� vi studerar p� umu, s� ser vi helst att kursen g�r p� kv�llen, och d� medel�ldern �r 25 �r, s� ser vi g�rna att din �lder �r i n�rheten av detta.   ",
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
                        Tag = tags.First(n => n.Name == "nya v�nner"),
                        Event = ec7,
                    },
                };

            var ec8 = new Event
            {
                Name = "pubafton",
                Description =
                    "Jag �r ensam, men lust att festa loss i helgen, �r ni n�gra som vill festa loss med mig, s� tr�ffas vi p� Allstar kl 21:00. ",
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
                        Tag = tags.First(n => n.Name == "nya v�nner"),
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
                Name = "Picknink med alls�ng",
                Description =
                    "Vi kommer att h�lla ig�ng med alls�ng varje m�ndag kv�ll p� v�nortsparken, det �r bara att dyka upp med en picknickkorg och sjunga med oss.",
                StartDate = new DateTime(2016, 09, 12, 18, 00, 00),
                EndDate = new DateTime(2067, 09, 12, 20, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 85,
                MinSignups = 25,
                Price = 0,
                ImagePath = PathToEventImages + "alls�ng.png"
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
                Name = "Istid f�r barnen",
                Description =
                    "Dagen innan s�songen b�rjar kommer det vara istid f�r barnen, d�r barn i �ldrarna 3-7 �r f�r komma ut p� isen och l�ra sig �ka skridskor, vi kommer �ven sponsra med bes�k av n�gra av v�ra a-lagspelare. som visar sina konster.",
                StartDate = new DateTime(2016, 09, 14, 10, 00, 00),
                EndDate = new DateTime(2067, 09, 14, 10, 00, 00),
                Latitude = GenerateRandomLatitude(),
                Longitude = GenerateRandomLongitude(),
                MaxSignups = 50,
                MinSignups = 15,
                Price = 0,
                ImagePath = PathToEventImages + "bj�rkl�ven.png"
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
                        Tag = tags.First(n => n.Name == "nya v�nner"),
                        Event = ec10,
                    },
                };

            var ec11 = new Event
            {
                Name = "halltid f�r barnen",
                Description =
                    "Dagen innan s�sonen b�rjar kommer det vara full �s i Noliahallarna av barn mellan 3-7 �r som f�r komma och l�ra sig olika bollsporter.",
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
                        Tag = tags.First(n => n.Name == "nya v�nner"),
                        Event = ec11,
                    },
                };

            var listOfEvents = new List<Event>
                {
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
                };


            context.EventUsers.AddRange(eventUsers);
            context.SaveChanges();


            // Ger information till Kontakta oss sidan
            var companyInfo = new CompanyInformation
            {
                Address = "Tvistev�gen 48",
                Description =
                    "Vi �r ett f�retag som arbetar f�r att m�nniskor ska kunna knyta nya kontakter via egengjorda event. ",
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

