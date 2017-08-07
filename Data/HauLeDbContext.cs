using Data.Infrastructure.DomainEvents;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class HauLeDbContext : IdentityDbContext<AppUser>
    {
        //private readonly IDomainEventDispatcher _dispatcher;
        public HauLeDbContext(DbContextOptions<HauLeDbContext> options) : base(options)
        {
            //_dispatcher = dispatcher;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }

        public DbSet<AppRole> AppRoles { set; get; }

        //public override int SaveChanges()
        //{
        //    var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
        //        .Select(e => e.Entity)
        //        .Where(e => e.Events.Any())
        //        .ToArray();
        //    var result = base.SaveChanges();
        //    foreach (var entity in entitiesWithEvents)
        //    {
        //        var events = entity.Events.ToArray();
        //        entity.Events.Clear();
        //        foreach (var domainEvent in events)
        //        {
        //            _dispatcher.Dispatch(domainEvent);
        //        }
        //    }
        //    return result;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppUser>().ToTable("AppUsers");

            modelBuilder.Entity<IdentityRole>().ToTable("AppRoles").HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });
        }
    }
}
