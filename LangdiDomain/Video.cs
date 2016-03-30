using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LangdiDomain
{
    public class Video:DbBaseModel
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Code { get; set; }
    }
}
