using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

public class BlogPost
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public string Image { get; set; } = string.Empty;

    [Required]
    public string Created { get; set; }
    public BlogPost()
    {
        Created = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm");
    }

    public string Updated { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    
}