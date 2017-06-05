using BBP.DAL.Interface.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.DAL.ADO
{
    internal static class Configuration
    {
        #region [Construtor estatico]

        /// <summary>
        /// Construtor estático para efetuar as configurações padrão do framework
        /// </summary>
        static Configuration()
        {
   
        }

        #endregion

        #region [Propriedades]

        /// <summary>
        /// ORM utilizado para o acesso a dados
        /// </summary>
        public static IMicroORM MicroORM { get { return (_MicroORM ?? (_MicroORM = new MicroORM())); } }

        /// <summary>
        /// ORM utilizado para o acesso a dados
        /// </summary>
        private static IMicroORM _MicroORM { get; set; }

        static internal void SetMicroORM(IMicroORM microORM) { _MicroORM = microORM; }

        #endregion
    }
}
