using System.Collections.Generic;
using System.ComponentModel;



namespace TabloidMVC.Models.ViewModels
{
    public class CommentViewModel
    {
        [DisplayName("Comment")]
        public List<Comment> comments { get; set; }
        public Post post { get; set; }
        [DisplayName("User Name")]
        public UserProfile user { get; set; }
    }
}
