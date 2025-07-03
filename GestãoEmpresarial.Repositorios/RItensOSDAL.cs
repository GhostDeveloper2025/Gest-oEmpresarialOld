using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    public class RItensOSDAL : DatabaseConnection, IDAL<ItemOrdemServicoModel>
    {
        private readonly RProdutoDAL rProdutoDal;

        public RItensOSDAL(int idFuncionario) : base(idFuncionario)
        {
            rProdutoDal = new RProdutoDAL(idFuncionario);
        }

        // Método para buscar múltiplos itens de OS por vários IDs
        public async Task<Dictionary<int, List<ItemOrdemServicoModel>>> GetByMultipleIdsOsAsync(IEnumerable<int> idsOs)
        {
            if (idsOs == null || !idsOs.Any())
                return new Dictionary<int, List<ItemOrdemServicoModel>>();

            string query = @"SELECT IdOs, Desconto, IdItensOs, Quantidade, ValUnitario, IdProduto 
                     FROM tb_itensos 
                     WHERE IdOs IN (@idsOs)";

            string idsOsParam = string.Join(",", idsOs);

            Dictionary<int, List<ItemOrdemServicoModel>> itensMap = new Dictionary<int, List<ItemOrdemServicoModel>>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idsOs", idsOsParam);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray())) // Leitura assíncrona
            {
                while (await reader.ReadAsync()) // Processamento assíncrono dos dados
                {
                    var item = new ItemOrdemServicoModel
                    {
                        IdOs = DALHelper.GetInt32(reader, "IdOs").Value,
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        IdItensOs = DALHelper.GetInt32(reader, "IdItensOs").Value,
                        Quantidade = DALHelper.GetInt32(reader, "Quantidade").Value,
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
                    };

                    int? idProduto = DALHelper.GetInt32(reader, "IdProduto");
                    if (idProduto.HasValue)
                    {
                        item.Produto = await rProdutoDal.GetByIdAsync(idProduto.Value); // Busca produto de forma assíncrona
                    }

                    // Verificar se o IdOs já está no dicionário
                    if (!itensMap.ContainsKey(item.IdOs))
                    {
                        itensMap[item.IdOs] = new List<ItemOrdemServicoModel>();
                    }

                    itensMap[item.IdOs].Add(item);
                }
            }

            return itensMap;
        }

        public async Task DeleteAsync(ItemOrdemServicoModel t)
        {
            MySqlParameter[] arr = new MySqlParameter[]
            {
        new MySqlParameter() { Value = t.IdOs, ParameterName = "Os_id" },
            };
            ExecuteNonQuery("usp_cancel_Os", arr, true); // Execução assíncrona
        }

        public async Task DeleteItemlAsync(ItemOrdemServicoModel t)
        {
            string query = "DELETE FROM tb_itensos WHERE IdItensOs = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdItensOs, ParameterName= "@id" },
            };
            ExecuteNonQuery("usp_Del_Os_item", arr, true); // Execução assíncrona

        }
        // Retorna lista de itens da OS por ID
        public async Task<List<ItemOrdemServicoModel>> GetByIdOsAsync(int id)
        {
            return await ListAsync(id.ToString());
        }

        // Busca de item por ID (não implementado)
        public Task<ItemOrdemServicoModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        // Inserção assíncrona de item da OS
        public async Task<int> InsertAsync(ItemOrdemServicoModel t)
        {
            string query = "INSERT INTO tb_itensos (Quantidade, ValUnitario, Desconto, ValTotal, IdOs, IdProduto) " +
                           "VALUES(@Quantidade, @ValUnitario, @Desconto, @ValTotal, @IdOs, @IdProduto); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Quantidade", t.Quantidade);
            AddParameter(lista, "@ValUnitario", t.ValUnitario);
            AddParameter(lista, "@Desconto", t.Desconto);
            AddParameter(lista, "@ValTotal", t.CustoTotal);
            AddParameter(lista, "@IdOs", t.IdOs);
            AddParameter(lista, "@IdProduto", t.Produto.IdProduto);

            object id = ExecuteScalar(query, lista.ToArray()); // Execução assíncrona
            return Convert.ToInt32(id);
        }

        // Lista de itens da OS de forma assíncrona
        public async Task<List<ItemOrdemServicoModel>> ListAsync(string filtro)
        {
            string query = @"SELECT IdOs, Desconto, IdItensOs, Quantidade, ValUnitario, IdProduto FROM tb_itensos WHERE IdOs = @idOs";

            List<ItemOrdemServicoModel> lista = new List<ItemOrdemServicoModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idOs", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray())) // Leitura assíncrona
            {
                while (await reader.ReadAsync()) // Processamento assíncrono dos dados
                {
                    var obj = new ItemOrdemServicoModel
                    {
                        IdOs = DALHelper.GetInt32(reader, "IdOs").Value,
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        IdItensOs = DALHelper.GetInt32(reader, "IdItensOs").Value,
                        Quantidade = DALHelper.GetInt32(reader, "Quantidade").Value,
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
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

        // Atualização (não implementada)
        public Task UpdateAsync(ItemOrdemServicoModel t)
        {
            //não fazer nada
            return Task.CompletedTask;
        }

        // Implementações da interface IDAL<T>
        public async Task Delete(ItemOrdemServicoModel t)
        {
            await DeleteAsync(t);
        }

        public async Task<int> Insert(ItemOrdemServicoModel t)
        {
            return await InsertAsync(t);
        }
        
        public ItemOrdemServicoModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public List<ItemOrdemServicoModel> List(string filtro)
        {
            return ListAsync(filtro).Result; // Compatibilidade com método síncrono
        }
    }
}


