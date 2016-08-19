using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

//namespace ScratchPadLibrary
//{
    // When it comes to generating the data, I personally make use of two 3rd party libraries,
    // namely NBuilder and Faker.NET. Both these libraries have not been updated for a long time,
    // but they still work just fine.
    //public class AgileDbInitializer : DropCreateDatabaseAlways<xxxOURCONTEXTxxx>
    //{
    //    protected override void Seed(xxxOURCONTEXTxxx context)
    //    {
    //        // Users (identity)

    //        // USERS AND ROLES
    //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
    //        string[] roleNames = { "Admin", "Owner", "User" };
    //        IdentityResult roleResult;
    //        foreach (var roleName in roleNames)
    //        {
    //            if (!roleManager.RoleExists(roleName))
    //            {
    //                roleResult = roleManager.Create(new IdentityRole(roleName));
    //            }
    //        }

    //        if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
    //        {
    //            var store = new UserStore<ApplicationUser>(context);
    //            var manager = new UserManager<ApplicationUser>(store);
    //            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

    //            manager.Create(user, "password");
    //            manager.AddToRole(user.Id, "Admin");
    //        }

    //        if (!context.Users.Any(u => u.UserName == "owner@owner.com"))
    //        {
    //            var store = new UserStore<ApplicationUser>(context);
    //            var manager = new UserManager<ApplicationUser>(store);
    //            var user = new ApplicationUser { UserName = "owner@owner.com", Email = "owner@owner.co" };

    //            manager.Create(user, "password");
    //            manager.AddToRole(user.Id, "Owner");
    //        }

    //        if (!(context.Users.Any(u => u.UserName == "user@user.com")))
    //        {
    //            var userStore = new UserStore<ApplicationUser>(context);
    //            var userManager = new UserManager<ApplicationUser>(userStore);
    //            var userToInsert = new ApplicationUser { UserName = "user@user.com", Email = "user@user.com" };
    //            userManager.Create(userToInsert, "password");
    //            userManager.AddToRole(userToInsert.Id, "User");
    //        }

    //        context.SaveChanges();

    //        // Profiles
    //        var profiles = new List<Profile>()
    //        {
    //            new Profile()
    //            {
    //            },
    //        };
    //        context.Profiles.AddOrUpdate(s => s.ID, profiles.ToArray());
    //        context.SaveChanges();


    //        // Events
    //        var events = new List<Event>()
    //        {
    //            new Event()
    //            {
    //            },
    //        };
    //        context.Events.AddOrUpdate(s => s.ID, events.ToArray());
    //        context.SaveChanges();


    //        // EventProfiles (Signups)
    //        var eventProfiles = new List<EventProfile>()
    //        {
    //            new EventProfile()
    //            {
    //            },
    //        };
    //        context.EventProfiles.AddOrUpdate(c => c.ID, eventProfiles.ToArray());
    //        context.SaveChanges();


    //        var courseEvents = new List<CourseEvent>()
    //        {
    //            new CourseEvent()
    //            {
    //                Course = courses.Where(c => c.Name == "Csharp").SingleOrDefault(),
    //                Teacher = teachers.Where(t => t.FirstName == "Lemmy").SingleOrDefault(),
    //                StartDate = DateTime.Now.Date.AddDays(10),
    //                EndDate = DateTime.Now.Date.AddDays(20)
    //            },
    //            new CourseEvent()
    //            {
    //                Course = courses.Where(c => c.Name == "Gardening").SingleOrDefault(),
    //                Teacher = teachers.Where(t => t.FirstName == "Lemmy").SingleOrDefault(),
    //                StartDate = DateTime.Now.Date.AddDays(10),
    //                EndDate = DateTime.Now.Date.AddDays(20)
    //            },
    //            new CourseEvent()
    //            {
    //                Course = courses.Where(c => c.Name == "Databases").SingleOrDefault(),
    //                Teacher = teachers.Where(t => t.FirstName == "Barbara").SingleOrDefault(),
    //                StartDate = DateTime.Now.Date.AddDays(20),
    //                EndDate = DateTime.Now.Date.AddDays(30)
    //            },
    //        };
    //        context.CourseEvents.AddOrUpdate(c => c.ID, courseEvents.ToArray());
    //        context.SaveChanges();


    //        base.Seed(context);
    //    }
//    }
//}
