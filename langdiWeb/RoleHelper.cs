using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace langdiWeb
{
    public class RoleHelper
    {
        public static IRepository<User, int> UserRepository = new Repository<User, int>();
        public static Role GetUserRole(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var id = int.Parse(userId);
                var user = UserRepository.Load(id);
                if (user == null)
                    throw new DataException("未找到该用户");
                return user.Role;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}