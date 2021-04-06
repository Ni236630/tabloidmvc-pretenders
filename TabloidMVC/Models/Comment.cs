using System;
using System.ComponentModel;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserProfileId { get; set; }
        [DisplayName("Subject")]
        public string Subject { get; set; }
        [DisplayName("Content")]
        public string Content { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string CommentAuthor { get; set; }


    }
}

