using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;
public class Comment
{
    public int CommentID { get; set; }
    [Required]
    public string AuthorName { get; set; }

    [Required]
    public string Body { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Required]
    public int PostID { get; set; }

    public Post? Post { get; set; }




}