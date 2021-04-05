using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository : BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }

        public List<UserType> GetAllTypes()
        {
            using(var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, [Name]
                            FROM UserType
                    ";
                    var reader = cmd.ExecuteReader();
                    List<UserType> types = new List<UserType>();
                    while(reader.Read())
                    {
                        UserType type = new UserType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        types.Add(type);
                    }
                    reader.Close();
                    return types;
                }
            }
        }
    }
}
