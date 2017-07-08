using BBP.DAL.Entity.Exception;
using BBP.DAL.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BBP.DAL.Entity
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IObjectWithKey
    {

        #region [Construtores]

        /// <summary>
        /// Set the context
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DbContext context)
        {
            if (context == null)
                throw new NullContextException("The DbContext not set a reference os an object");

            this.Context = context;
        }

        #endregion

        #region [Propriedades]

        /// <summary>
        /// Contexto
        /// </summary>
        public DbContext Context { get; set; }

        #endregion

        #region [CUD]

        /// <summary>
        /// Save the changes in the context
        /// | Salva as alterações efetuadas no contexto
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        /// <summary>
        /// Add the object in the context and save him
        /// | Adiciona um objeto ao contexto e efetua o save do mesmo
        /// </summary>
        /// <param name="entity">entidade alvo</param>
        /// <param name="isSaveChanges">indica se o objeto deverá ser submetido imediatamente ao SaveChanges</param>
        /// <returns></returns>
        public virtual int Insert(T entity, bool isSaveChanges = true)
        {
            this.Context.Set<T>().Add(entity);
            return isSaveChanges ? this.SaveChanges() : 0;
        }

        /// <summary>
        /// Altera um objeto na base de dados e imediatamente aciona o SaveChanges
        /// </summary>
        /// <param name="entity">entidade alvo</param>
        /// <param name="isSaveChanges">indica se o objeto deverá ser submetido imediatamente ao SaveChanges</param>
        /// <returns></returns>
        public virtual int Edit(T entity, bool isSaveChanges = true)
        {
            this.Context.Entry<T>(entity).State = EntityState.Modified;
            return isSaveChanges ? this.SaveChanges() : 0;
        }

        /// <summary>
        /// Funciona apenas para primary key com chave única (não sendo chave composta) e do tipo numerico.
        /// Identifica se é uma alteração ou inserção, chamando o metodo correto de acordo com o valor da chave (insercao se a chave for igual a 0)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChanges"></param>
        /// <returns></returns>
        public virtual int Save(T entity, bool isSaveChanges = true)
        {
            return entity.GetPrimaryKey() == 0
               ? this.Insert(entity, isSaveChanges)
               : this.Edit(entity, isSaveChanges);
        }


        /// <summary>
        /// Deleta um registro na base de dados, chamando imediatamente o SaveChanges
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChanges"></param>
        /// <returns></returns>
        public virtual int Delete(T entity, bool isSaveChanges = true)
        {
            this.Context.Set<T>().Remove(entity);
            return isSaveChanges ? this.SaveChanges() : 0;
        }

        #endregion

        #region Read

        /// <summary>
        /// Retorna todos os registros correspondentes a entidade
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return this.Context.Set<T>().AsQueryable();
        }

        public DbQuery<T> Include(string include)
        {
            return this.Context.Set<T>().Include(include);
        }

        /// <summary>
        /// Efetua uma busca baseada em uma expressao
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.Context.Set<T>().Where(predicate);
        }

        /// <summary>
        /// Retorna o primeiro elemento baseado em uma expressao
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Context.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Conexão com a base de dados
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection()
        {
            return this.Context.Database.Connection;
        }


        #endregion
    }
}
