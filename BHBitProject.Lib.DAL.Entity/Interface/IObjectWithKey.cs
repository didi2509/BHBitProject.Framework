using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.Entity.Interface
{
    public interface IObjectWithKey
    {
        long GetPrimaryKey();
    }
}
