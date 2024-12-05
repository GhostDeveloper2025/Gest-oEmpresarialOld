using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace GestãoEmpresarial.Repositorios
{
    /// <summary>
    /// CRUD
    /// - Create (Criar)
    /// - Read (Ler)
    /// - Update (Atualizar)
    /// - Delete (Apagar/Remover)
    /// </summary>
    public class RVendasDAL : DatabaseConnection, IDAL<VendaModel>
    {
        internal readonly RColaboradorDAL rColaboradorDAL;
        internal readonly RClienteDAL rClienteDAL;
        internal readonly RItensVendaDAL rItensVendaDAL;

        public RVendasDAL(int idFuncionario) : base(idFuncionario)
        {
            rColaboradorDAL = new RColaboradorDAL(idFuncionario);
            rItensVendaDAL = new RItensVendaDAL(idFuncionario);
            rClienteDAL = new RClienteDAL(idFuncionario);
        }

        public async Task DeleteAsync(VendaModel t)
        {
            string query = "DELETE FROM tb_venda WHERE IdVenda = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdVenda, ParameterName = "@id" },
            };
            ExecuteNonQuery(query, arr); // Execução assíncrona
        }

        public async Task<VendaModel> GetByIdAsync(int id)
        {
            return (await GetListaAsync(true, null, null, id)).FirstOrDefault();
        }

        public async Task<int> InsertAsync(VendaModel t)
        {
            string query = "INSERT INTO tb_venda (DataVenda, DataFinalizacao, Situacao, IdFuncionario, IdCliente, ValorFrete, IdCodigoTipoPagamento) " +
                           "VALUES(NOW(), @DataFinalizacao, @Situacao, @IdFuncionario, @IdCliente, @ValorFrete, @IdCodigoTipoPagamento); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@IdFuncionario", idFuncionario);
            AddParameter(lista, "@IdCliente", t.Cliente.Idcliente);
            AddParameter(lista, "@Situacao", t.Situacao);
            AddParameter(lista, "@DataFinalizacao", t.Situacao ? DateTime.Now : (DateTime?)null);
            AddParameter(lista, "@ValorFrete", t.ValorFrete);
            AddParameter(lista, "@IdCodigoTipoPagamento", t.IdCodigoTipoPagamento);

            object id = ExecuteScalar(query, lista.ToArray()); // Execução assíncrona
            return Convert.ToInt32(id);
        }

        public async Task<List<VendaModel>> ListAsync(string nomeCliente, int? idCodigoTipoPagamento, int? IdVenda)
        {
            return await GetListaAsync(false, nomeCliente, idCodigoTipoPagamento, IdVenda);
        }

        public async Task<List<VendaModel>> GetListaAsync(bool comItens, string nomeCliente, int? idCodigoTipoPagamento, int? idVenda)
        {
            var parametros = new List<MySqlParameter>();
            var conditions = new List<string>();
            AddParameterCondition(parametros, conditions, "IdVenda", idVenda);

            if (idCodigoTipoPagamento.HasValue)
            {
                AddParameterCondition(parametros, conditions, "IdCodigoTipoPagamento", idCodigoTipoPagamento);
            }

            if (!string.IsNullOrWhiteSpace(nomeCliente))
            {
                AddParameter(parametros, "@nomeCliente", nomeCliente);
                conditions.Add("IdCliente IN (SELECT IdCliente FROM tb_cliente WHERE nome LIKE CONCAT(@nomeCliente, '%'))");
            }

            string conditionsJoin = conditions.Count > 0 ? string.Join(" AND ", conditions) : "1=1";
            string query = $@"SELECT IdVenda, DataVenda, DataFinalizacao, Situacao, ValorFrete, IdCliente, IdFuncionario, IdCodigoTipoPagamento
                      FROM tb_venda WHERE {conditionsJoin} LIMIT 200";

            List<VendaModel> lista = new List<VendaModel>();
            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                var idsClientes = new HashSet<int>();
                var idsColaboradores = new HashSet<int>();

                while (await reader.ReadAsync())
                {
                    var obj = Mapeador.Map(new VendaModel(), reader);
                    lista.Add(obj);
                    idsClientes.Add(obj.IdCliente);
                    idsColaboradores.Add(obj.IdFuncionario);
                }

                var clientes = await rClienteDAL.GetByIdsAsync(idsClientes.ToList());
                var colaboradores = await rColaboradorDAL.GetByIdsAsync(idsColaboradores.ToList());

                foreach (var venda in lista)
                {
                    venda.Cliente = clientes.FirstOrDefault(c => c.Idcliente == venda.IdCliente);
                    // venda.Cadastrante = colaboradores.FirstOrDefault(c => c.IdFuncionario == venda.IdFuncionario);
                    venda.Vendedor = colaboradores.FirstOrDefault(c => c.IdFuncionario == venda.IdFuncionario);

                    if (comItens)
                    {
                        venda.ListItensVenda = await rItensVendaDAL.GetByIdVendaAsync(venda.IdVenda);
                    }
                }
            }

            return lista;
        }

        public async Task UpdateAsync(VendaModel t)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "id", t.IdVenda);
            AddParameter(lista, "sitcao", t.Situacao);
            AddParameter(lista, "valorFrete", t.ValorFrete);
            AddParameter(lista, "idCodigoTipoPagamento", t.IdCodigoTipoPagamento);
            ExecuteNonQuery("usp_upd_venda", lista.ToArray(), true); // Execução assíncrona
        }

        public VendaModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }

}

