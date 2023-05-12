using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
//using WpfApp.Models;

namespace WpfApp
{
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {

        }/*
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.Product> Products { get; set; }*/
    }
}
