using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace MvcProjectDbConn
{
    public class BizAssistContext : IdentityDbContext
    {
        public DbSet<TimeSheetEntry> TimeSheetEntries { get; set; }
        public DbSet<Client> Clients { get; set; }
      
        public DbSet<WorkSchedule> WorkScheudules { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
        public DbSet<EmployeePayStub> EmployeePayStubs { get; set; } 
        public DbSet<ClientBill> ClientBills { get; set; }
        public DbSet<Project> Projects { get; set; }
        public static BizAssistContext Create()
        {
            return new BizAssistContext();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ClientProject> ClientProjects { get; set; } 
        public DbSet<DeletedCompany> DeletedCompanies { get; set; }
    
        public DbSet<Chat> Chats { get; set; }
      
       public  DbSet<IdentityUserRole> Permissions { get; set; }
    
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Chat>()
        //                .HasRequired(x=>x.Sender)
        //                .WithMany(x=>x.ChatSender)
        //                .HasForeignKey(x=>x.SenderId)
        //                .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<Chat>()
        //                .HasRequired(x=>x.Reciever)
        //                .WithMany(x=>x.ChatReciever)
        //                .HasForeignKey(x=>x.RecieverId)
        //                .WillCascadeOnDelete(false);
        //}
        public BizAssistContext()
        
            :base("BizAssistContext")
        { 
        }
       
    }
}
