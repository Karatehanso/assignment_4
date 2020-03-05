using System.Collections.Generic;
using assignment_4.Models;
using Microsoft.AspNetCore.Identity;

namespace assignment_4.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
       
        public List<Post> Posts { get; set; }
    }
}