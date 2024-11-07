using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatorioHistoricoVendasDAL : DatabaseConnection
    {
        public RRelatorioHistoricoVendasDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public async Task<List<RelatorioHistoricoVendasModel>> ObterHistoricoVendasAsync(int? clienteId, int? idProduto, DateTime dataInicial, DateTime dataFinal)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>();

            // Adicionando os parâmetros para a stored procedure
            AddParameter(parametros, "varclienteId", clienteId);
            AddParameter(parametros, "varDataInicial", dataInicial);
            AddParameter(parametros, "varDataFinal", dataFinal.AddDays(1));
            AddParameter(parametros, "varIdProduto", idProduto);

            var list = new List<RelatorioHistoricoVendasModel>();

            using (MySqlDataReader reader = ExecuteReader("usp_relatorio_historico_venda", parametros.ToArray(), true))
            {
                while (await reader.ReadAsync()) // Leitura assíncrona
                {
                    list.Add(new RelatorioHistoricoVendasModel()
                    {
                        NumVenda = reader.GetInt32("num_venda"),
                        DataEmissao = reader.GetDateTime("data_emissao"),
                        DataHoraEmissao = reader.GetDateTime("datahora_emissao"),
                        DataFinalizacao = reader.GetDateTime("data_finalizacao"),
                        ClienteId = reader.GetInt32("cliente_id"),
                        ClienteCpf = reader.GetString("cliente_cpf"),
                        ClienteNome = reader.GetString("cliente_nome"),
                        VendedorId = reader.GetInt32("vendedor_id"),
                        VendedorCpf = reader.GetString("vendedor_cpf"),
                        VendedorNome = reader.GetString("vendedor_nome"),
                        Situacao = reader.GetString("situacao"),
                        TotalProdutos = reader.GetDecimal("totalprodutos"),
                        TotalDesconto = reader.GetDecimal("totaldesconto"),
                        Subtotal = reader.GetDecimal("subtotal"),
                        TotalFrete = reader.GetDecimal("totalfrete"),
                        TotalVenda = reader.GetDecimal("totalvenda"),
                        QuantidadeTotal = reader.GetInt32("quantidade_total")
                    });
                }
            }

            return list;
        }

        // Método sincrono para compatibilidade
        public List<RelatorioHistoricoVendasModel> ObterHistoricoVendas(int? clienteId, int? idProduto, DateTime dataInicial, DateTime dataFinal)
        {
            return ObterHistoricoVendasAsync(clienteId, idProduto, dataInicial, dataFinal).GetAwaiter().GetResult();
        }
    }
}




