using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BBP.DAL.Entity.Interface
{
    public interface IGenericRepositoryBehavior<T> where T : class, IObjectWithKey
    {

        #region [CUD]

     
        int SaveChanges();


        int Insert(T entity, bool isSaveChanges = true);


        int Edit(T entity, bool isSaveChanges = true);


        int Save(T entity, bool isSaveChanges = true);

   


        int Delete(T entity, bool isSaveChanges = true);

        #endregion

        DbQuery<T> Include(string include);
 
        #region Read

        IQueryable<T> GetAll();

  

  
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

     
        T FirstOrDefault(Expression<Func<T, bool>> predicate);


        #endregion

        #region [Connection]

        /// <summary>
        /// Acesso a conexao com a base de dados
        /// </summary>
        /// <returns></returns>
        DbConnection GetConnection();

        #endregion
    }
}
