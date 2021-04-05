using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        UserProfile GetById(int userId);
        void CreateUser(UserProfile user);
        void UpdateUserProfile(int id, UserProfile user);
        List<UserProfile> GetAllUsers();
        UserProfile getAdminCount();
    }
}