using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.Entity.Exception
{
    public class GetEntityKeyException : System.Exception
    {
        public GetEntityKeyException() : base() { }
        public GetEntityKeyException(string message) : base(message) { }
        public GetEntityKeyException(string message, System.Exception exception) : base(message, exception) { }
    }
}
