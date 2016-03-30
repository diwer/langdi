using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangdiDomain
{
    public class Contacter: DbBaseModel
    {
        
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string NeedDoc { get; set; }
        public string ReMark { get; set; }
    }
}
