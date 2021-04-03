using System.Collections.Generic;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
   public interface IUserTypeRepository
    {
        List<UserType> GetAllTypes();
    }
}
