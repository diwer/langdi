using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangdiDomain
{
    public enum State
    {
        Nomal,
        Delete
    }
    public abstract class DbBaseModel
    {
        public int Id { get; set; }
        public State State { get; set; }
    }
}
