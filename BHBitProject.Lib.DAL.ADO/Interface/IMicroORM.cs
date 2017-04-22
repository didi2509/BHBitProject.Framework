using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.Interface.ADO
{

   
    /// <summary>
    /// Interface para a utilização de MicroORM no framework
    /// </summary>
    public interface IMicroORM
    {

        /// <summary>
        /// Função da ferramenta ORM que irá buscar uma coleção de registros
        /// </summary>
        //
        // Summary:
        //     Executes a query, returning the data typed as per T
        //
        // Returns:
        //     A sequence of data of the supplied type; if a basic type (int, string, etc) is
        //     queried then the data from the first column in assumed, otherwise an instance
        //     is created per row, and a direct column-name===member-name mapping is assumed
        //     (case insensitive).
        IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));




        /// <summary>
        /// Função da ferramenta ORM que irá buscar um único registro
        /// </summary>
         //
        // Summary:
        //     Executes a single-row query, returning the data typed as per T
        //
        // Returns:
        //     A sequence of data of the supplied type; if a basic type (int, string, etc) is
        //     queried then the data from the first column in assumed, otherwise an instance
        //     is created per row, and a direct column-name===member-name mapping is assumed
        //     (case insensitive).
        T QueryFirst<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));


    }
}
