using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.BLL.Entity.Base
{
    public class UnitOfWork<TContext> where TContext :  DbContext
    {
        #region [Properties]

        TContext Context { get; set; }

        #endregion

        #region [Constructors]

        public UnitOfWork(TContext context) { this.Context = context; }

        #endregion
    }
}
