using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatoriosDAL : DatabaseConnection
    {
        public RRelatoriosDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public object ObterCommissao()
        {
            string query = "feita no sql";
            using (MySqlDataReader reader = ExecuteReader(query))
            {
                while (reader.Read())
                {
                    //list.Add(new CategoriaModel()
                    //{
                    //    IdCategoria = reader.GetInt32("IdCategoria"),
                    //    Nome = DALHelper.GetString(reader, "Nome"),
                    //    Descricao = DALHelper.GetString(reader, "Descricao"),

                    //});
                }
            }
            //return list;
            return null;
        }
    }
}
