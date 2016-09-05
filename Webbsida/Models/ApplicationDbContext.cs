using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DatabaseObjects;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Webbsida.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Webbsida", throwIfV1Schema: false)
        {
            Database.SetInitializer(new AgilaMetoderProjektDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: Remove ALL FORMS of cascade-deletes??
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
    }
}