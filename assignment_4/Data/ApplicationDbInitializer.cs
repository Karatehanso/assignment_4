using System;
using assignment_4.Models;
using assignment_4.Data;
using assignment_4.Models;
using Microsoft.AspNetCore.Identity;

namespace assignment_4.Data
{
    public static class ApplicationDbInitializer
    {

        public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um)
        {
            db.Database.EnsureDeleted();

            db.Database.EnsureCreated();
       
           /* var user = new ApplicationUser { UserName = "user@uia.no", Email = "user@uia.no", Nickname = "Nabon"};
            um.CreateAsync(user, "Password1.").Wait();
            
           db.SaveChanges();
            
           var post1 = new Post {Title = "Min første blog post", Summary = "Konklusjon", Content  = "This is a blog post",Time = DateTime.Now, Owner = user };

           
            db.Add(post1); */
  
            db.SaveChanges();              // Save changes to the database

        }
    }
}