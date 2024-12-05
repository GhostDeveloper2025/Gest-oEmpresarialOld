using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using static Mysqlx.Expect.Open.Types;

namespace GestãoEmpresarial.Repositorios
{
    public class DatabaseConnection
    {
        protected readonly int idFuncionario;

        public DatabaseConnection(int idFuncionario)
        {
            if (idFuncionario < 0)
                throw new ArgumentOutOfRangeException(nameof(idFuncionario));

            this.idFuncionario = idFuncionario;
        }

        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["dbnew"].ConnectionString;

        private MySqlConnection OpenConnection()
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        private static void MysqlCommandProps(string query, MySqlParameter[] parameters, MySqlCommand command, bool isSp)
        {
            command.CommandText = query;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            if (isSp)
                command.CommandType = CommandType.StoredProcedure;
        }

        protected void ExecuteNonQuery(string query, MySqlParameter[] parameters, bool isSp = false)
        {
            using (var connection = OpenConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    MysqlCommandProps(query, parameters, command, isSp);
                    command.ExecuteNonQuery();
                }
            }
        }

        protected MySqlDataReader ExecuteReader(string query, MySqlParameter[] parameters = null, bool isSp = false)
        {
            var connection = OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            MysqlCommandProps(query, parameters, command, isSp);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        protected object ExecuteScalar(string query, MySqlParameter[] parameters = null, bool isSp = false)
        {
            using (var connection = OpenConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    MysqlCommandProps(query, parameters, command, isSp);
                    return command.ExecuteScalar();
                }
            }
        }

        internal void AddParameter(List<MySqlParameter> parameters, string parameterName, object value)
        {
            parameters.Add(new MySqlParameter { ParameterName = parameterName, Value = value ?? DBNull.Value });
        }

        public void AddParameterCondition(List<MySqlParameter> parametros, List<string> condicoes, string nomeColuna, object valorPesquisa)
        {
            if (valorPesquisa != null)
            {
                AddParameter(parametros, "@" + nomeColuna, valorPesquisa);
                condicoes.Add(nomeColuna + " = @" + nomeColuna);
            }
        }
    }
}
