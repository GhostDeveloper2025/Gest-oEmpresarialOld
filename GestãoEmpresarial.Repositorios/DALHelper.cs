using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using static Mysqlx.Expect.Open.Types;

namespace GestãoEmpresarial.Helpers
{
    static class DALHelper
    {
        public static decimal? GetDecimalNullable(MySqlDataReader reader, string column_name)
        {
            try
            {
                if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                    return reader.GetDecimal(column_name);
                return null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public static string GetString(MySqlDataReader reader, string column_name)
        {
            string text = string.Empty;

            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                text = reader.GetString(column_name);

            return text;
        }

        public static decimal GetDecimal(MySqlDataReader reader, string column_name)
        {
            decimal value = 0;

            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                value = reader.GetDecimal(column_name);

            return value;
        }

        public static double GetDouble(MySqlDataReader reader, string column_name)
        {
            double value = 0.0;

            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                value = reader.GetDouble(column_name);

            return value;
        }

        public static DateTime? GetDateTime(MySqlDataReader reader, string column_name)
        {
            DateTime? value = null;

            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                value = reader.GetDateTime(column_name);

            return value;
        }

        public static int? GetInt32(MySqlDataReader reader, string column_name)
        {
            int? value = null;

            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                value = reader.GetInt32(column_name);

            return value;
        }

        /// <summary>
        /// Verifica na base de dados se o valor é nulo (NULL).
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="column_name"></param>
        /// <returns>Devolve True se sim, devolve False se não for nulo.</returns>
        public static bool IsNull(MySqlDataReader reader, string column_name)
        {
            return reader.IsDBNull(reader.GetOrdinal(column_name));
        }

        public static bool GetBool(MySqlDataReader reader, string column_name)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(column_name)))
                return reader.GetBoolean(column_name);
            else
                return false;
        }
    }
}
