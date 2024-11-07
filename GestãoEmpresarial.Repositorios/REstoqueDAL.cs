using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class REstoqueDAL : DatabaseConnection, IDAL<EstoqueModel>
    {
        public REstoqueDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        // Método assíncrono para deletar um produto no estoque pelo Id do produto
        public async Task DeleteByIdProdutoAsync(int idproduto)
        {
            string query = "DELETE FROM tb_estoque WHERE IdProduto = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = idproduto, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr); // Aguarda a execução assíncrona
        }

        // Método assíncrono para deletar uma entrada de estoque pelo Id do estoque
        public async Task DeleteAsync(EstoqueModel t)
        {
            string query = "DELETE FROM tb_estoque WHERE IdEstoque = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdEstoque, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr); // Aguarda a execução assíncrona
        }

        // Busca de estoque por ID (não implementado)
        public Task<EstoqueModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Inserção assíncrona de estoque
        public async Task<int> InsertAsync(EstoqueModel t)
        {
            string query = "INSERT INTO tb_estoque (IdProduto, localizacao, Quantidade) " +
                           "VALUES(@IdProduto, @localizacao, @Quantidade); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@IdProduto", t.IdProduto);
            AddParameter(lista, "@localizacao", t.Localizacao);
            AddParameter(lista, "@Quantidade", t.Quantidade);

            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        // Lista assíncrona de estoques
        public async Task<List<EstoqueModel>> ListAsync(string filtro)
        {
            List<EstoqueModel> list = new List<EstoqueModel>();
            string query = "SELECT IdEstoque, IdProduto, localizacao, Quantidade FROM tb_estoque";

            using (MySqlDataReader reader = ExecuteReader(query))
            {
                while (await reader.ReadAsync()) // Leitura assíncrona dos dados
                {
                    list.Add(new EstoqueModel()
                    {
                        IdEstoque = reader.GetInt32("IdEstoque"),
                        Localizacao = DALHelper.GetString(reader, "localizacao"),
                        Quantidade = reader.GetInt32("Quantidade"),
                    });
                }
            }
            return list;
        }

        // Atualização assíncrona de estoque
        public async Task UpdateAsync(EstoqueModel t)
        {
            string query = "UPDATE tb_estoque SET localizacao = @localizacao, Quantidade = @Quantidade " +
                           "WHERE IdEstoque = @Id";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdEstoque);
            AddParameter(lista, "@localizacao", t.Localizacao);
            AddParameter(lista, "@Quantidade", t.Quantidade);

            ExecuteNonQuery(query, lista.ToArray()); // Aguarda a execução assíncrona
        }

        // Implementações da interface IDAL<T>
        public async Task Delete(EstoqueModel t)
        {
            await DeleteAsync(t);
        }

        public async Task Update(EstoqueModel t)
        {
            await UpdateAsync(t);
        }

        public async Task<int> Insert(EstoqueModel t)
        {
            return await InsertAsync(t);
        }

        
        public EstoqueModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public List<EstoqueModel> List(string filtro)
        {
            return ListAsync(filtro).Result; // Para manter compatibilidade síncrona, se necessário
        }

        
    }
}

