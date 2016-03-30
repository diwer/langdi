using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangdiDomain
{
    public class News : DbBaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public virtual Image Image { get; set; }
        public virtual Category Category { get; set; }
        public int ViewCount { get; set; }
        public int LoveCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EditTime { get; set; }
        public int CategoryId { get; set; }
    }
    public class Image : DbBaseModel
    {
        public string Src { get; set; }
        public string Alt { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
