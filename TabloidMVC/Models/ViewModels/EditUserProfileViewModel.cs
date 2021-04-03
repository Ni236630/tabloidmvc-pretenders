using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class EditUserProfileViewModel
    {
        public UserProfile UserProfile { get; set; }
        public List<UserType> UserTypes { get; set; }
        public List<UserProfile> Users { get; set; }
    }
}
