using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LangdiDomain
{

    public class Category : DbBaseModel
    {
        public string Name { get; set; }
        public int Order { get; set; }

        public virtual List<News> News { get; set; }

    }

}
