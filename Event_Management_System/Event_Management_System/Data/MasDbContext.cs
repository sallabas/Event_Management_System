using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models.Base;       

namespace Event_Management_System.Data
{
    public class MasDbContext : DbContext
    {
        // for database configuration (mas.db was appearing in bin folder)
        public MasDbContext() : base() { }
        public MasDbContext(DbContextOptions<MasDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Organizer> Organizers => Set<Organizer>();
        public DbSet<RegularUser> RegularUsers => Set<RegularUser>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventCategory> EventCategories => Set<EventCategory>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<PaymentDetail> PaymentDetails => Set<PaymentDetail>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<DiscussionComment> DiscussionComments => Set<DiscussionComment>();
        public DbSet<PromotedRequest> PromotedRequests => Set<PromotedRequest>();


        // mandatory working path -> related to "GUI -> .db" connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                var dbPath = @"C:\Users\kysal\RiderProjects\Event_Management_System\Event_Management_System\mas.db";

        
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }*/
        }

        
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 1) Çalışma dizininden yukarı doğru çık
                var dir = new DirectoryInfo(AppContext.BaseDirectory);

                while (dir != null && dir.Name != "Event_Management_System")
                {
                    dir = dir.Parent;
                }

                if (dir == null)
                {
                    throw new Exception("Project root (Event_Management_System) could not be found.");
                }

                // 2) DB dosyasının kesin doğru konumu
                var dbPath = Path.Combine(dir.FullName, "mas.db");

                Console.WriteLine("EF REAL DB PATH => " + dbPath);

                // 3) EF Core bağlantıyı yap
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
        */
        
        
        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Organizer>("Organizer")
                .HasValue<RegularUser>("Regular");

            // Enrollment
            b.Entity<Enrollment>()
                .HasKey(e => new { e.UserId, e.EventId });

            b.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Event 
            b.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<Event>()
                .HasMany(e => e.Enrollments)
                .WithOne(en => en.Event)
                .HasForeignKey(en => en.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<Event>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.TargetEvent)
                .HasForeignKey(c => c.TargetEventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<Event>()
                .HasMany(e => e.Categories)
                .WithMany(c => c.Events)
                .UsingEntity(j => j.ToTable("EventCategoryLink"));
            
            /*
            // Event-Tag relation 
            b.Entity<Event>()
                .OwnsMany(e => e.Tags, t =>
                {
                    t.ToTable("EventTags");
                    t.WithOwner().HasForeignKey("EventId");

                    t.Property<string>("Tag")
                        .HasColumnName("Tag")
                        .IsRequired();

                    t.HasKey("EventId", "Tag");
                });
                */
            

            //Discussion Comment
            b.Entity<DiscussionComment>()
                .HasOne(dc => dc.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(dc => dc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<DiscussionComment>()
                .HasOne(dc => dc.TargetEvent)
                .WithMany(e => e.Comments)
                .HasForeignKey(dc => dc.TargetEventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Location
            b.Entity<Location>()
                .HasMany(l => l.Venues)
                .WithOne(v => v.Location)
                .HasForeignKey(v => v.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message 
            b.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            b.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            // Promoted Request 
            b.Entity<PromotedRequest>()
                .HasOne(pr => pr.TargetEvent)
                .WithMany(e => e.PromotedRequests)
                .HasForeignKey(pr => pr.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<PromotedRequest>()
                .HasOne(pr => pr.Organizer)
                .WithMany(o => o.PromotedRequests)
                .HasForeignKey(pr => pr.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // User 
            b.Entity<User>()
                .HasOne(u => u.PaymentDetail)
                .WithOne(p => p.OwnerUser)
                .HasForeignKey<PaymentDetail>(p => p.OwnerUserId)
                .IsRequired(false)   
                .OnDelete(DeleteBehavior.Cascade);
            
            // User follow mechanism - self referencing beacuse of following system (w join table name)
            b.Entity<User>()
                .HasMany(u => u.Following)
                .WithMany(u => u.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFollow",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict),

                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict)
                )
                .HasKey("FollowerId", "FollowingId");

            // Transaction 
            b.Entity<Transaction>()
                .HasOne(t => t.PaymentDetail)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PaymentDetailId)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.Entity<Transaction>()
                .HasOne(t => t.PromotedRequest)
                .WithMany(pr => pr.Transactions)
                .HasForeignKey(t => t.PromotedRequestId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
