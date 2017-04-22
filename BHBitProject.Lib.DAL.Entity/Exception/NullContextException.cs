using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.Entity.Exception
{
    public class NullContextException : System.Exception
    {
        public NullContextException() : base() { }
        public NullContextException(string message) : base(message) { }
        public NullContextException(string message, System.Exception exception) : base(message, exception) { }
    }
}
