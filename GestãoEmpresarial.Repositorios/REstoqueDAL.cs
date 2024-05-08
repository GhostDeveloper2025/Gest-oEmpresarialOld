using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;

namespace GestãoEmpresarial.Repositorios
{
    public class REstoqueDAL : DatabaseConnection, IDAL<EstoqueModel>
    {
        public REstoqueDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public void Delete(int idproduto)
        {
            string query = "DELETE FROM tb_estoque WHERE IdProduto = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = idproduto, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public void Delete(EstoqueModel t)
        {
            string query = "DELETE FROM tb_estoque WHERE IdEstoque = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdEstoque, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public EstoqueModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(EstoqueModel t)
        {
            string query = "INSERT INTO tb_estoque (IdProduto, localizacao, Quantidade) " +
           "  VALUES(@IdProduto, @localizacao, @Quantidade);"
            + " SELECT last_insert_id()";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@IdProduto", t.IdProduto);
            AddParameter(lista, "@localizacao", t.Localizacao);
            AddParameter(lista, "@Quantidade", t.Quantidade);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);

        }

        public List<EstoqueModel> List(string filtro)
        {
            List<EstoqueModel> list = new List<EstoqueModel>();
            string query = "SELECT IdEstoque, IdProduto, localizacao, Quantidade FROM tb_estoque ";

            using (MySqlDataReader reader = ExecuteReader(query))
            {
                while (reader.Read())
                {
                    list.Add(new EstoqueModel()
                    {
                        IdEstoque = reader.GetInt32("IdEstoque"),
                        //IdProduto = reader.GetInt32("IdEstoque"),                        
                        Localizacao = DALHelper.GetString(reader, "localizacao"),
                        Quantidade = reader.GetInt32("Quantidade"),

                    });
                }
            }
            return list;
        }


        public void Update(EstoqueModel t)
        {
            string query = "UPDATE tb_estoque SET localizacao = @localizacao, Quantidade = @Quantidade " +
            " WHERE IdEstoque = @Id";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdEstoque);
            AddParameter(lista, "@localizacao", t.Localizacao);
            AddParameter(lista, "@Quantidade", t.Quantidade);
            ExecuteNonQuery(query, lista.ToArray());
        }
    }
}
