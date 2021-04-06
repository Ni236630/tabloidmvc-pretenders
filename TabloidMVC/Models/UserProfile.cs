using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Runtime.Intrinsics.X86;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [DisplayName("User Name")]
        public string DisplayName { get; set; }
        public string Email { get; set; }

        [DisplayName("Creation Date")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Profile Image")]
        public string ImageLocation { get; set; }

        [DisplayName("Account Type")]
        public int UserTypeId { get; set; }
        [DisplayName("User Role")]
        public UserType UserType { get; set; }
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
       public int AdminCount { get; set; }
    }
}