using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models;

public class Post
{
    public int PostID { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [Required]
    public int CategoryID { get; set; }
    
    public string? UserID { get; set; }

    public IdentityUser? User{ get; set; }


    public Category? Category { get; set; } 

    public ICollection<Comment> Comments { get; set; }=new List<Comment>();


}