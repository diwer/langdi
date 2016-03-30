using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangdiDomain
{
    public class User : DbBaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string GetRole()
        {
            var str = "";
            switch (Role)
            {
                case Role.Editer:
                    str = "编辑";
                    break;
                case Role.Manager:
                    str = "管理员";
                    break;
            }
            return str;
        }
    }
    public enum Role
    {
        Editer,
        Manager
    }
}
