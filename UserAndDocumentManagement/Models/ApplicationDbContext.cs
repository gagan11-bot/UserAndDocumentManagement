using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAndDocumentManagement.Models;

namespace UserAndDocumentManagement.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IngestionRequestModel> IngestionRequestModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
        public DbSet<RegisterModel> RegisterModels { get; set; }
        public DbSet<RoleUpdateModel> RoleUpdateModels { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<IngestionStatus> IngestionStatuses { get; set; }

    }
}