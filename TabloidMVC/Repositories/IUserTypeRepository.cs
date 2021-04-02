using System.Collections.Generic;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    interface IUserTypeRepository
    {
        List<UserType> GetAllTypes();
    }
}
