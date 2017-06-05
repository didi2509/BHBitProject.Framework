using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.DAL.Entity.Interface
{
    public interface IObjectWithKey
    {
        long GetPrimaryKey();
    }
}
