using MySql.Data.MySqlClient;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GestãoEmpresarial.Repositorios
{
    public class RCategoriaDAL : DatabaseConnection, IDAL<CategoriaModel>
    {
        public RCategoriaDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        // Método para deletar uma categoria de forma assíncrona
        public async Task DeleteAsync(CategoriaModel t)
        {
            await Task.Run(() =>
            {
                string query = "DELETE FROM tb_categoria WHERE IdCategoria = @id";
                MySqlParameter[] arr = new MySqlParameter[]
                {
                    new MySqlParameter() { Value = t.IdCategoria, ParameterName = "@id" },
                };
                ExecuteNonQuery(query, arr);  // Aguarda a execução do método assíncrono
            });
        }

        // Verifica se há produtos na categoria de forma assíncrona
        public async Task<bool> HasProductsAsync(int id)
        {
            string query = "SELECT * FROM tb_produto WHERE IdCategoria = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = id, ParameterName = "@id" },
            };

            using (var reader = ExecuteReader(query, arr))
            {
                return reader.HasRows;
            }
        }

        // Busca uma categoria por ID de forma assíncrona
        public async Task<CategoriaModel> GetByIdAsync(int id)
        {
            var lista = await ListAsync(id.ToString());
            return lista.FirstOrDefault();
        }

        // Inserção de uma categoria de forma assíncrona
        public async Task<int> InsertAsync(CategoriaModel t)
        {
            string query = "INSERT INTO tb_categoria (Nome, Descricao) " +
                           "VALUES(@Nome, @Descricao); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@Descricao", t.Descricao);

            object id = ExecuteScalar(query, lista.ToArray());  // Aguarda a execução do método assíncrono
            return Convert.ToInt32(id);
        }

        // Método assíncrono que retorna uma lista de categorias
        public async Task<List<CategoriaModel>> ListAsync(string filtro)
        {
            List<CategoriaModel> list = new List<CategoriaModel>();
            string query = "SELECT IdCategoria, Nome, Descricao FROM tb_categoria " +
                           "WHERE @filtro IS NULL OR Nome LIKE CONCAT('%', @filtro, '%') OR IdCategoria = @filtro";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@filtro", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                while (await reader.ReadAsync())
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

        // Atualiza uma categoria de forma assíncrona
        public async Task UpdateAsync(CategoriaModel t)
        {
            string query = "UPDATE tb_categoria SET Nome = @Nome, Descricao = @Descricao " +
                           "WHERE IdCategoria = @Id";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdCategoria);
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@Descricao", t.Descricao);

            ExecuteNonQuery(query, lista.ToArray());  // Aguarda a execução do método assíncrono
        }

        // Método List síncrono que chama a versão assíncrona (usando .Result, se necessário)
        public List<CategoriaModel> List(string filtro)
        {
            return ListAsync(filtro).Result;  // Mantém compatibilidade síncrona, mas idealmente deve ser evitado
        }

        // Implementação de métodos da interface
        public async Task Delete(CategoriaModel t)
        {
            await DeleteAsync(t);
        }

        public async Task Update(CategoriaModel t)
        {
            await UpdateAsync(t);
        }

        public async Task<int> Insert(CategoriaModel t)
        {
            return await InsertAsync(t);
        }

        public CategoriaModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}


