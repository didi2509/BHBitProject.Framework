using BBP.DAL.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BBP.DAL.Entity.Interface
{
    public interface IGenericRepository<T>:IGenericRepositoryBehavior<T> where T : class,IObjectWithKey
    {
        #region [Propriedades]

        /// <summary>
        /// Contexto
        /// </summary>
        DbContext Context { get; set; }

        #endregion
    }
}
