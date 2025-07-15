using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApplicationMVCLogin.Models;

namespace WebApplicationMVCLogin.Views.DBContect
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<LoginViewModel> loginViewModels { get; set; }

    }
}
