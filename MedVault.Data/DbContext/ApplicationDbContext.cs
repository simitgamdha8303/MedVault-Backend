using Microsoft.EntityFrameworkCore;
using MedVault.Models.Entities;

namespace MedVault.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets

        public DbSet<User> Users => Set<User>();
        public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();
        public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();
        public DbSet<Hospital> Hospitals => Set<Hospital>();
        public DbSet<Reminder> Reminders => Set<Reminder>();
        public DbSet<ReminderType> ReminderTypes => Set<ReminderType>();
        public DbSet<MedicalTimeline> MedicalTimelines => Set<MedicalTimeline>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<QrShare> QrShares => Set<QrShare>();
        public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Appointment> Appointments => Set<Appointment>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReminderType>().HasData(
                new ReminderType { Id = 1, Name = "Lab Test" },
                new ReminderType { Id = 2, Name = "Appointment" },
                new ReminderType { Id = 3, Name = "Medicine" }
            );

            modelBuilder.Entity<Hospital>().HasData(
                new Hospital { Id = 1, Name = "HCG" },
                new Hospital { Id = 2, Name = "Svastik" },
                new Hospital { Id = 3, Name = "Sterling" },
                new Hospital { Id = 4, Name = "Apollo" },
                new Hospital { Id = 5, Name = "Synergy" },
                new Hospital { Id = 6, Name = "Wockhardt" },
                new Hospital { Id = 7, Name = "AIIMS" }
            );


            // User indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Mobile)
                .IsUnique();

            // User - PatientProfile (1–1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.PatientProfile)
                .WithOne(p => p.User)
                .HasForeignKey<PatientProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - DoctorProfile (1–1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.DoctorProfile)
                .WithOne(d => d.User)
                .HasForeignKey<DoctorProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Hospital - DoctorProfile (1–Many)
            modelBuilder.Entity<DoctorProfile>()
                .HasOne(d => d.Hospital)
                .WithMany(h => h.Doctors)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserRole (ENUM-based Many–Many)
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.Role });

            modelBuilder.Entity<UserRole>()
                .Property(ur => ur.Role)
                .HasConversion<int>();

            // PatientProfile - Reminder (1–Many)
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.PatientProfile)
                .WithMany(p => p.Reminder)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reminder - ReminderType (Many–1)
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.ReminderType)
                .WithMany()
                .HasForeignKey(r => r.ReminderTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicalTimeline relations
            modelBuilder.Entity<MedicalTimeline>()
                .HasOne(m => m.PatientProfile)
                .WithMany(p => p.MedicalTimelines)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalTimeline>()
                .HasOne(m => m.DoctorProfile)
                .WithMany()
                .HasForeignKey(m => m.DoctorProfileId)
                .OnDelete(DeleteBehavior.SetNull);

            // Document relations
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.MedicalTimeline)
                .WithMany(m => m.Documents)
                .HasForeignKey(d => d.MedicalTimelineId)
                .OnDelete(DeleteBehavior.Cascade);


            // QR Share
            modelBuilder.Entity<QrShare>()
                .HasOne(q => q.PatientProfile)
                .WithMany()
                .HasForeignKey(q => q.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QrShare>()
                .HasOne(q => q.DoctorProfile)
                .WithMany()
                .HasForeignKey(q => q.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.PatientProfile)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.DoctorProfile)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // OTP Verification
            modelBuilder.Entity<OtpVerification>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);


        }
    }
}
