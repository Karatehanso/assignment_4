using System;
using System.ComponentModel.DataAnnotations;
using assignment_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace assignment_4.Models
{
    public class Post
    {
        
    public int Id { get; set; }

    [Required]
    [StringLength(50), MinLength(3)]
    public String Title { get; set; }
    
    [Required]
    [StringLength(50), MinLength(3)]
    public String Summary { get; set; }
    
    [Required]
    [StringLength(50), MinLength(3)]
    public String Content { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Time { get; set; }

    public string OwnerId { get; set; }
        
    // Navigation property, doesn't become a field in the DB
    public ApplicationUser Owner { get; set; }
    
    }
}