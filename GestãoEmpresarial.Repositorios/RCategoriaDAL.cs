using MySql.Data.MySqlClient;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GestãoEmpresarial.Repositorios
{
    public class RCategoriaDAL : DatabaseConnection, IDAL<CategoriaModel>
    {
        public RCategoriaDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public void Delete(CategoriaModel t)
        {
            string query = "DELETE FROM tb_categoria WHERE IdCategoria = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdCategoria, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }
        //Verifica se tem uma categoria em um produto
        public bool HasProducts(int id)
        {
            //Devolve todos os registos se houver algum, EXECUTE READER
            string query = "SELECT * FROM tb_produto WHERE IdCategoria = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = id, ParameterName= "@id" },
            };

            using (var reader = ExecuteReader(query, arr))
            {
                return reader.HasRows;
            }
        }

        public CategoriaModel GetById(int id)
        {
            return List(id.ToString()).FirstOrDefault();
        }

        public int Insert(CategoriaModel t)
        {
            string query = "INSERT INTO tb_categoria (Nome, Descricao) " +
             " VALUES(@Nome, @Descricao);"
            + " SELECT last_insert_id()";

            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@Descricao", t.Descricao);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<CategoriaModel> List(string filtro)
        {
            List<CategoriaModel> list = new List<CategoriaModel>();
            string query = "SELECT IdCategoria, Nome, Descricao FROM tb_categoria "
                + " WHERE @filtro IS NULL OR Nome LIKE CONCAT('%', @filtro, '%') OR IdCategoria = @filtro";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@filtro", filtro);
            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                while (reader.Read())
                {
                    list.Add(new CategoriaModel()
                    {
                        IdCategoria = reader.GetInt32("IdCategoria"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        Descricao = DALHelper.GetString(reader, "Descricao"),

                    });
                }
            }
            return list;
        }

        public void Update(CategoriaModel t)
        {
            string query = "UPDATE tb_categoria SET Nome = @Nome, Descricao = @Descricao " +
             " WHERE IdCategoria = @Id";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdCategoria);
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@Descricao", t.Descricao);
            ExecuteNonQuery(query, lista.ToArray());
        }
    }
}
