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
    public class RRelatorioProdutoMaisVendidoDAL : DatabaseConnection
    {
        public RRelatorioProdutoMaisVendidoDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        //public async Task<List<RelatorioProdutoMaisVendido>> ObterProdutoMaisVendidoAsync(DateTime? dataInicial, DateTime? dataFinal)
        //{
        //    string query = "SELECT * FROM dbnew.uvw_relatorio_produto_mais_vendido WHERE 1=1";

        //    if (dataInicial.HasValue)
        //    {
        //        query += $" AND DataFinalizacao >= '{dataInicial.Value:yyyy-MM-dd}'";
        //    }

        //    if (dataFinal.HasValue)
        //    {
        //        query += $" AND DataFinalizacao <= '{dataFinal.Value:yyyy-MM-dd}'";
        //    }

        //    var list = new List<RelatorioProdutoMaisVendido>();
        //    using (MySqlDataReader reader = ExecuteReader(query))
        //    {
        //        while (await reader.ReadAsync()) // Leitura assíncrona
        //        {
        //            list.Add(new RelatorioProdutoMaisVendido()
        //            {
        //                Id = reader.GetInt32("IdProduto"),
        //                Codigo = DALHelper.GetString(reader, "CodigoProduto"),
        //                Descricao = DALHelper.GetString(reader, "DescricaoProduto"),
        //                Localizacao = DALHelper.GetString(reader, "Localizacao"),
        //                Desconto = DALHelper.GetDecimal(reader, "Desconto"),
        //                Quantidade = reader.GetInt32("QuantidadeVendida"),
        //                ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
        //                CustoTotal = DALHelper.GetDecimal(reader, "ValTotal"),
        //                DataFinalizacao = reader.GetDateTime("DataFinalizacao"),
        //            });
        //        }
        //    }
        //    return list;
        //}


        public async Task<(List<RelatorioProdutoMaisVendido> Itens, int TotalRegistros)> ObterProdutoMaisVendidoAsync(
    DateTime? dataInicial, DateTime? dataFinal, int pagina, int itensPorPagina)
        {
            // Query para contar o total de registros (sem paginação)
            string queryCount = "SELECT COUNT(*) FROM dbnew.uvw_relatorio_produto_mais_vendido WHERE 1=1";

            if (dataInicial.HasValue)
            {
                queryCount += $" AND DataFinalizacao >= '{dataInicial.Value:yyyy-MM-dd}'";
            }

            if (dataFinal.HasValue)
            {
                queryCount += $" AND DataFinalizacao <= '{dataFinal.Value:yyyy-MM-dd}'";
            }

            // Executa a contagem total de registros
            int totalRegistros = Convert.ToInt32(ExecuteScalar(queryCount));

            // Query para obter os registros paginados
            string query = "SELECT * FROM dbnew.uvw_relatorio_produto_mais_vendido WHERE 1=1";

            if (dataInicial.HasValue)
            {
                query += $" AND DataFinalizacao >= '{dataInicial.Value:yyyy-MM-dd}'";
            }

            if (dataFinal.HasValue)
            {
                query += $" AND DataFinalizacao <= '{dataFinal.Value:yyyy-MM-dd}'";
            }

            // Paginação
            int offset = (pagina - 1) * itensPorPagina;
            query += $" ORDER BY DataFinalizacao DESC LIMIT {itensPorPagina} OFFSET {offset}";

            var list = new List<RelatorioProdutoMaisVendido>();
            using (MySqlDataReader reader = ExecuteReader(query))
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new RelatorioProdutoMaisVendido()
                    {
                        Id = reader.GetInt32("IdProduto"),
                        Codigo = DALHelper.GetString(reader, "CodigoProduto"),
                        Descricao = DALHelper.GetString(reader, "DescricaoProduto"),
                        Localizacao = DALHelper.GetString(reader, "Localizacao"),
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        Quantidade = reader.GetInt32("QuantidadeVendida"),
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
                        CustoTotal = DALHelper.GetDecimal(reader, "ValTotal"),
                        DataFinalizacao = reader.GetDateTime("DataFinalizacao"),
                    });
                }
            }

            return (list, totalRegistros);
        }
        // Método síncrono para compatibilidade
        public (List<RelatorioProdutoMaisVendido> Itens, int TotalRegistros) ObterProdutoMaisVendido(DateTime? dataInicial, DateTime? dataFinal, int pagina = 1, int itensPorPagina = 1)
        {
            return ObterProdutoMaisVendidoAsync(dataInicial, dataFinal, pagina, itensPorPagina).GetAwaiter().GetResult();
        }

        //Método sincrono para compatibilidade
        //public List<RelatorioProdutoMaisVendido> ObterProdutoMaisVendido(DateTime? dataInicial, DateTime? dataFinal)
        //{
        //    return ObterProdutoMaisVendidoAsync(dataInicial, dataFinal).GetAwaiter().GetResult();
        //}
    }
}

