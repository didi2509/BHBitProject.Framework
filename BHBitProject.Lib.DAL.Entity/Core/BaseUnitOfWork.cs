using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.Entity.Core
{
    public class BaseUnitOfWork
    {

        #region [Construtores]

        /// <summary>
        /// Construtor, devera ser fornecida a string de conexao
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseUnitOfWork(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Propriedades]

        /// <summary>
        /// String de conexão
        /// </summary>
        protected string ConnectionString { get; set; }


        private DbContext _context { get; set; }

        /// <summary>
        /// Contexto
        /// </summary>
        protected DbContext Context
        {
            get { return (this._context ?? (this._context = new DbContext(this.ConnectionString))); }
        }

        #endregion
    }
}