using System.Collections.Generic;


namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    {
        public List<Comment> comments { get; set; }
        public Post post { get; set; }
        public UserProfile user { get; set; }
    }
}
