
using BBP.DAL.Entity.Interface;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;

namespace BBP.DAL.Entity
{
    /// <summary>
    /// Classe base para a criação de objetos de entidade especializados para o acesso a dados
    /// </summary>
    /// <typeparam name="EntityType"></typeparam>
    public class BaseDAL<EntityType> : IGenericRepositoryBehavior<EntityType> where EntityType : class,IObjectWithKey
    {
        #region [Propriedades]

        /// <summary>
        /// Contexto
        /// </summary>
        private DbContext _context { get; set; }

        /// <summary>
        /// Repositório genérico
        /// </summary>
        private IGenericRepository<EntityType> repository { get; set; }

        #endregion

        #region [Construtores]

        /// <summary>
        /// Instancia um repositório genérico e seta o contexto do DAL
        /// </summary>
        /// <param name="context"></param>
        public BaseDAL(DbContext context)
        {
            this._context = context;
            this.repository = new GenericRepository<EntityType>(context);
        }

        /// <summary>
        /// Seta o repositório
        /// </summary>
        /// <param name="context">Contexto utilizado pelo repositório</param>
        /// <param name="repository">Repositório</param>
        public BaseDAL(DbContext context, IGenericRepository<EntityType> repository)
        {
            this.repository = (GenericRepository<EntityType>)repository;
            this.repository.Context = context;
        }

        #endregion

        #region [CRUD]

        /// <summary>
        /// Retorna o acesso aos comportamentos do objeto de repositório
        /// </summary>
        /// <returns></returns>
        protected IGenericRepositoryBehavior<EntityType> GetRepositorie()
        {
            return this.repository;
        }


        /// <summary>
        /// Deleta um registro na base de dados, chamando imediatamente o SaveChanges
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChanges"></param>
        /// <returns></returns>
        public virtual int Delete(EntityType entity, bool isSaveChanges = true)
        {
            return this.repository.Delete(entity, isSaveChanges);
        }

        /// <summary>
        /// Altera um objeto na base de dados e imediatamente aciona o SaveChanges
        /// </summary>
        /// <param name="entity">entidade alvo</param>
        /// <param name="isSaveChanges">indica se o objeto deverá ser submetido imediatamente ao SaveChanges</param>
        /// <returns></returns>
        public virtual int Edit(EntityType entity, bool isSaveChanges = true)
        {
            return this.repository.Edit(entity, isSaveChanges);
        }


        /// <summary>
        /// Retorna o primeiro elemento baseado em uma expressao
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual EntityType FirstOrDefault(Expression<Func<EntityType, bool>> predicate)
        {
            return this.repository.FirstOrDefault(predicate);
        }


        /// <summary>
        /// Retorna todos os registros correspondentes a entidade
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<EntityType> GetAll()
        {
            return this.repository.GetAll();
        }




        /// <summary>
        /// Add the object in the context and save him
        /// | Adiciona um objeto ao contexto e efetua o save do mesmo
        /// </summary>
        /// <param name="entity">entidade alvo</param>
        /// <param name="isSaveChanges">indica se o objeto deverá ser submetido imediatamente ao SaveChanges</param>
        /// <returns></returns>
        public virtual int Insert(EntityType entity, bool isSaveChanges = true)
        {
            return this.repository.Insert(entity, isSaveChanges);
        }

        /// <summary>
        /// Funciona apenas para primary key com chave única (não sendo chave composta) e do tipo numerico.
        /// Identifica se é uma alteração ou inserção, chamando o metodo correto de acordo com o valor da chave (insercao se a chave for igual a 0)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSaveChanges"></param>
        /// <returns></returns>
        public virtual int Save(EntityType entity, bool isSaveChanges = true)
        {
            return this.repository.Save(entity, isSaveChanges);
        }

        /// <summary>
        /// Save the changes in the context
        /// | Salva as alterações efetuadas no contexto
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return this.repository.SaveChanges();
        }


        /// <summary>
        /// Efetua uma busca baseada em uma expressao
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<EntityType> Where(Expression<Func<EntityType, bool>> predicate)
        {
            return this.repository.Where(predicate);
        }

        /// <summary>
        /// Retorna a conexão com a base de dados
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection()
        {
            return this.repository.GetConnection();
        }

        #endregion
    }
}
