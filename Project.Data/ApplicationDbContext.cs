using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, AppRole, string>
    {
        public DbSet<Part> Parts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<UserRepairTask> UsersRepairsTasks { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<ExpertReceipt> ExpertsReceipts { get; set; }

        public DbSet<RepairTask> RepairTasks { get; set; } //TODO: When initial testing is performed, name this property appropriately.

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public ApplicationDbContext() {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<UserRepairTask>().HasKey(k => new { k.RepairTaskId, k.UserId });

            builder.Entity<ExpertReceipt>().HasKey(k => new { k.UserId, k.ReceiptId });

            builder.Entity<Receipt>()
                    .HasOne<RepairTask>(receipt => receipt.RepairTask)
                    .WithOne(repairTask => repairTask.Receipt)
                    .HasForeignKey<RepairTask>(rt => rt.ReceiptId).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Feedback>()
                    .HasOne<RepairTask>(feedback => feedback.RepairTask)
                    .WithOne(repairTask => repairTask.Feedback)
                    .HasForeignKey<RepairTask>(rt => rt.FeedbackId).OnDelete(DeleteBehavior.SetNull);


            base.OnModelCreating(builder);
        }
    }
}
