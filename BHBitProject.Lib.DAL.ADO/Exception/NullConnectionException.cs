using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.ADO.Exception
{
    public class NullConnectionException : System.Exception
    {
        public NullConnectionException() : base() { }
        public NullConnectionException(string message) : base(message) { }
        public NullConnectionException(string message, System.Exception exception) : base(message, exception) { }
    }
}
