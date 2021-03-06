﻿using BBP.DAL.ADO.Exception;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BBP.DAL.Interface.ADO;
using Dapper;

namespace BBP.DAL.ADO.Extensions
{
    public static class ADOExtensions
    {

        #region [ADO]

        /// <summary>
        /// Executa um comando na base de dados, se a conexão fornecida estiver fechada o próprio método irá gerenciar o estado da conexão
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="Command"></param>
        /// <param name="parameters"></param>
        public static int ExecuteTextCommand(this IDbConnection connection, string commandText, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            if (connection == null) throw new NullConnectionException("Referência nula para a instância de um objeto");

            bool isOpen = connection.State == System.Data.ConnectionState.Open || transaction != null;

            try
            {
                IDbCommand command = CreateCommand(connection, commandText, System.Data.CommandType.Text, transaction, parameters);

                if (!isOpen) connection.Open();
                return command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!isOpen)
                    connection.Dispose();
            }

        }




        /// <summary>
        /// Executa a leitura de dados na base e retorna o tipo de sua escolha, se a conexão fornecida estiver fechada o próprio método irá gerenciar o estado da conexão
        /// </summary>
        /// <typeparam name="T">Tipo do retorno, pode ser um único objeto ou uma coleção</typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="reader">Função onde você poder montar o seu retorno de acordo com o DataReader fornecido</param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteReader<T>(this IDbConnection connection, string commandText, Func<IDataReader, T> reader, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            if (connection == null) throw new NullConnectionException("Referência nula para a instância de um objeto");

            bool isOpen = connection.State == System.Data.ConnectionState.Open || transaction != null;

            IDataReader readerObject = null;

            try
            {
                IDbCommand command = CreateCommand(connection, commandText, System.Data.CommandType.Text, transaction, parameters);

                if (!isOpen) connection.Open();
                readerObject = command.ExecuteReader();

                return reader(readerObject);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (readerObject != null)
                    readerObject.Dispose();

                if (!isOpen)
                    connection.Dispose();
            }

        }

        public static DynamicParameters CreateParameters(this DbConnection con, params ADOParameter[] parametros)
        {
            DynamicParameters args = new Dapper.DynamicParameters(new { });

            if (parametros != null && parametros.Length > 0)
                parametros.ToList().ForEach(p => args.Add(p.Name, p.Value));

            return args;                    
        }

        /// <summary>
        /// Executa a leitura da primeira linha da primeira coluna retornada através de uma consulta na base de dados, se a conexão fornecida estiver fechada o próprio método irá gerenciar o estado da conexão
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="reader">Função onde você poder montar o seu retorno de acordo com o DataReader fornecido</param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetScalar<T>(this IDbConnection connection, string commandText, Func<Object, T> reader, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            if (connection == null) throw new NullConnectionException("Referência nula para a instância de um objeto");

            bool isOpen = connection.State == System.Data.ConnectionState.Open || transaction != null;

            try
            {
                IDbCommand command = CreateCommand(connection, commandText, System.Data.CommandType.Text, transaction, parameters);
                if (!isOpen) connection.Open();
                return reader(command.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!isOpen)
                    connection.Dispose();
            }

        }


        /// <summary>
        /// Cria um novo comando
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static IDbCommand CreateCommand(IDbConnection connection, string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
                command.Transaction = transaction;

            if (parameters != null && parameters.Length > 0)
            {
                int parameterLength = parameters.Length;

                for (int i = 0; i > parameterLength; i++)
                    command.Parameters.Add(parameters);
            }
            return command;
        }

        #endregion

        #region [MicroORM]

        /// <summary>
        /// Executa o comando Query<T> do framerok MicroORM fornecido, se a conexão não estiver aberta o próprio método irá efetuar a abertura
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection connection, string commandText, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (connection == null) throw new NullConnectionException("Referência nula para a instância de um objeto");

            bool isOpen = connection.State == System.Data.ConnectionState.Open || transaction != null;
            if (!isOpen) connection.Open();


            return Configuration.MicroORM.Query<T>(connection, commandText, param, transaction, buffered, commandTimeout, commandType);
        }

        public static IDbDataParameter CreateParameter(this IDbConnection connection, string name, object value, DbType? dbTypeParameter = null)
        {
            IDbDataParameter parameter = connection.CreateCommand().CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            if (dbTypeParameter.HasValue)
                parameter.DbType = dbTypeParameter.Value;

            return parameter;
        }

        public class ADOParameter
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public ADOParameter(string name, object value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        public static IDbDataParameter[] CreateParameters(this IDbConnection connection, ADOParameter[] parametros, DbType? dbTypeParameter = null)
        {
            if (parametros != null && parametros.Length > 0)
            {
                List<IDbDataParameter> parameters = new List<IDbDataParameter>();

                parametros.ToList().ForEach(p => parameters.Add(connection.CreateParameter(p.Name, p.Value, dbTypeParameter)));

                return parameters.ToArray();
            }

            return null;
        }

        public static IDbDataParameter CreateParameter(this IDbConnection connection, string name, object value)
        {
            IDbDataParameter parameter = connection.CreateCommand().CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Executa o comando QueryFirst<T> do framerok MicroORM fornecido, se a conexão não estiver aberta o próprio método irá efetuar a abertura
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T QueryFirst<T>(this IDbConnection connection, string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (connection == null) throw new NullConnectionException("Referência nula para a instância de um objeto");

            bool isOpen = connection.State == System.Data.ConnectionState.Open || transaction != null;
            if (!isOpen) connection.Open();

            return Configuration.MicroORM.QueryFirst<T>(connection, commandText, param, transaction, commandTimeout, commandType);
        }

        #endregion
    }
}
