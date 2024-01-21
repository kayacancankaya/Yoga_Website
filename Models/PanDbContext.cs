using Microsoft.EntityFrameworkCore;
using Pan.Models.DbClasses;

namespace Pan.Models
{
    public class PanDbContext : DbContext
    {
        
        public PanDbContext(DbContextOptions<PanDbContext> options) : base(options)
        {

        }
        public PanDbContext()
        {
        }
        public DbSet<Instructors> Instructors { get; set; }
        public DbSet<ClassTypes> ClassTypes { get; set; }
        public DbSet<ClassesMaster> ClassesMaster { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ChallengesMaster> ChallengesMaster { get; set; }
        public DbSet<Challenges> Challanges { get; set; }
        public DbSet<Registrations> Registrations { get; set; }
        public DbSet<Attendees> Attendees { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<ClassPayments> ClassPayments { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<LocationTypes> LocationTypes { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<EventClasses> EventClasses { get; set; }
        public DbSet<EventPayments> EventPayments { get; set; }
        //public DbSet<Suppliers> Suppliers { get; set; }
        //public DbSet<Customers> Customers { get; set; }
        //public DbSet<SalesChannels> SalesChannels { get; set; }
        //public DbSet<Products> Products { get; set; }
        //public DbSet<Orders> Orders { get; set; }
        //public DbSet<OrderDetails> OrderDetails { get; set; }
        //public DbSet<CouponCodes> CouponCodes { get; set; }
        //public DbSet<GiftCards> GiftCards { get; set; }
        //public DbSet<Subscribers> Subscribers { get; set; }
        //public DbSet<ProductDiscountsCoupon> ProductDiscountsCoupon { get; set; }
        //public DbSet<ProductDiscountsGiftCard> ProductDiscountsGiftCard { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classes>()
              .HasMany(c => c.Attendances)  
              .WithOne(a => a.Class)
              .HasForeignKey(a => a.ClassID);

            modelBuilder.Entity<Attendees>()
             .HasMany(a => a.Attendaces)  
             .WithOne(a => a.Attendee)
             .HasForeignKey(a => a.ClassID);


            base.OnModelCreating(modelBuilder);
        }

    }
}
