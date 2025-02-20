using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RItensVendaDAL : DatabaseConnection, IDAL<ItemVendaModel>
    {
        private readonly RProdutoDAL rProdutoDal;

        public RItensVendaDAL(int idFuncionario) : base(idFuncionario)
        {
            rProdutoDal = new RProdutoDAL(idFuncionario);
        }

        public async Task DeleteAsync(ItemVendaModel t)
        {
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdItensVenda, ParameterName = "id" },
            };
            ExecuteNonQuery("usp_del_venda_item", arr, true); // Execução assíncrona
        }

        public async Task<List<ItemVendaModel>> GetByIdVendaAsync(int id)
        {
            return await ListAsync(id.ToString());
        }

        public async Task<ItemVendaModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(ItemVendaModel t)
        {
            string query = "INSERT INTO tb_itensvenda (Quantidade, ValUnitario, Desconto, ValTotal, IdVenda, IdProduto) " +
                           "VALUES(@Quantidade, @ValUnitario, @Desconto, @ValTotal, @IdVenda, @IdProduto);" +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Quantidade", t.Quantidade);
            AddParameter(lista, "@ValUnitario", t.ValUnitario);
            AddParameter(lista, "@Desconto", t.Desconto);
            AddParameter(lista, "@ValTotal", t.CustoTotal);
            AddParameter(lista, "@IdVenda", t.IdVenda);
            AddParameter(lista, "@IdProduto", t.Produto.IdProduto);

            object id = ExecuteScalar(query, lista.ToArray()); // Execução assíncrona
            return Convert.ToInt32(id);
        }

        public async Task<List<ItemVendaModel>> ListAsync(string filtro)
        {
            string query = @"SELECT IdVenda, Desconto, IdItensVenda, Quantidade, ValUnitario, IdProduto, ValTotal 
                             FROM tb_itensvenda 
                             WHERE IdVenda = @IdVenda";

            List<ItemVendaModel> lista = new List<ItemVendaModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@IdVenda", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray())) // Leitura assíncrona
            {
                while (await reader.ReadAsync()) // Processamento assíncrono dos dados
                {
                    var obj = new ItemVendaModel
                    {
                        IdVenda = DALHelper.GetInt32(reader, "IdVenda").Value,
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        IdItensVenda = DALHelper.GetInt32(reader, "IdItensVenda").Value,
                        Quantidade = DALHelper.GetInt32(reader, "Quantidade").Value,
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
                        CustoTotal = DALHelper.GetDecimal(reader, "ValTotal"),
                    };

                    int? idProduto = DALHelper.GetInt32(reader, "IdProduto");
                    if (idProduto.HasValue)
                    {
                        obj.Produto = await rProdutoDal.GetByIdAsync(idProduto.Value); // Busca produto de forma assíncrona
                    }
                    lista.Add(obj);
                }
            }

            return lista;
        }

        public async Task UpdateAsync(ItemVendaModel t)
        {
            // Implementação de update se necessário, atualmente não faz nada

        }

        // Implementações da interface IDAL<T>
        public async Task Delete(ItemVendaModel t)
        {
            await DeleteAsync(t);
        }

        public async Task<int> Insert(ItemVendaModel t)
        {
            return await InsertAsync(t);
        }

        
        public ItemVendaModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public async Task<List<ItemVendaModel>> List(string filtro)
        {
            return await ListAsync(filtro);
        }

        public async Task Update(ItemVendaModel t)
        {
            await UpdateAsync(t);
        }

       
    }
}

