using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zadatak_1;
using Zadatak_2.Models;

namespace Zadatak_2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Zadatak_1.TodoItem> TodoItem { get; set; }

        public DbSet<Zadatak_2.Models.LabelsViewModel> LabelsViewModel { get; set; }

        public DbSet<Zadatak_2.Models.TodoViewModel> TodoViewModel { get; set; }
    }
}
