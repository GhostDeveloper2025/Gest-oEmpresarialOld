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
    public class RRelatorioComissaoVendaDAL : DatabaseConnection
    {
        public RRelatorioComissaoVendaDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public async Task<(List<RelatorioComissaoVendasModel> Itens, int TotalRegistros, decimal TotalVendido, decimal TotalComissao)> ObterComissaoVendasAsync(int? idColaborador, DateTime? dataInicial, DateTime? dataFinal, int pagina, int itensPorPagina)
        {
            // 1. Contagem total de registros
            // QueryCount modificada - Mantendo consistência nos cálculos
            string queryCount = @"
    SELECT 
        COUNT(DISTINCT v.IdVenda) AS TotalRegistros,
        COALESCE(CAST(ROUND(
            SUM(iv.ValTotal) + 
            SUM(v.ValorFrete) - 
            SUM(ROUND((iv.Desconto / 100) * (iv.Quantidade * iv.ValUnitario), 2))
        , 2) AS DECIMAL(10,2)), 0) AS TotalVendido,
        COALESCE(CAST(ROUND(
            SUM((f.Comissao / 100) * 
                (iv.ValTotal - ROUND((iv.Desconto / 100) * iv.ValTotal, 2)))
        , 2) AS DECIMAL(10,2)), 0) AS TotalComissao
    FROM 
        tb_venda v
    JOIN 
        tb_funcionario f ON v.IdFuncionario = f.IdFuncionario
    JOIN 
        tb_itensvenda iv ON v.IdVenda = iv.IdVenda
    WHERE 
        v.Situacao = '1'
        AND v.Cancelada = 0
        AND (@pIdFuncionario IS NULL OR v.IdFuncionario = @pIdFuncionario)
        AND (@pDataInicial IS NULL OR v.DataVenda >= @pDataInicial)
        AND (@pDataFinal IS NULL OR v.DataVenda <= @pDataFinal)";


            var parametrosCount = new List<MySqlParameter>
        {
            new MySqlParameter("@pIdFuncionario", idColaborador ?? (object)DBNull.Value),
            new MySqlParameter("@pDataInicial", dataInicial ?? (object)DBNull.Value),
            new MySqlParameter("@pDataFinal", dataFinal ?? (object)DBNull.Value)
        };

            int totalRegistros = 0;
            decimal totalVendido = 0;
            decimal totalComissao = 0;

            // Modifiquei aqui - Usando DALHelper para todos os campos
            using (MySqlDataReader reader = ExecuteReader(queryCount, parametrosCount.ToArray()))
            {
                if (reader.Read())
                {
                    totalRegistros = DALHelper.GetInt32(reader, "TotalRegistros") ?? 0;
                    totalVendido = DALHelper.GetDecimal(reader, "TotalVendido");
                    totalComissao = DALHelper.GetDecimal(reader, "TotalComissao");
                }
            }

            string queryPaginada = @"
    SELECT 
        v.IdVenda AS num_venda,
        v.DataVenda AS data_venda,
        v.DataFinalizacao AS data_finalizacao,
        v.IdFuncionario AS vendedor_id,
        f.CPF AS vendedor_cpf,
        f.Nome AS vendedor_nome,
        v.Situacao AS situacao,
        v.Cancelada AS cancelada,
        CAST(ROUND(COALESCE(SUM(iv.ValTotal), 0), 2) AS DECIMAL(10,2)) AS totalvenda,
        CAST(ROUND(COALESCE(SUM(ROUND(iv.ValTotal * (iv.Desconto / 100), 2)), 0), 2) AS DECIMAL(10,2)) AS desconto_venda,
        CAST(ROUND(COALESCE(v.ValorFrete, 0), 2) AS DECIMAL(10,2)) AS frete_venda,
        CAST(ROUND(COALESCE(f.Comissao, 0), 2) AS DECIMAL(10,2)) AS perc_comissao,
        CAST(ROUND(COALESCE(
            (f.Comissao / 100) * 
            (SUM(iv.ValTotal) - SUM(ROUND(iv.ValTotal * (iv.Desconto / 100), 2)))
        , 0), 2) AS DECIMAL(10,2)) AS val_comissao
    FROM 
        tb_venda v
    JOIN 
        tb_funcionario f ON v.IdFuncionario = f.IdFuncionario
    JOIN 
        tb_itensvenda iv ON v.IdVenda = iv.IdVenda
    WHERE 
        v.Situacao = '1'
        AND v.Cancelada = 0
        AND (@pIdFuncionario IS NULL OR v.IdFuncionario = @pIdFuncionario)
        AND (@pDataInicial IS NULL OR v.DataVenda >= @pDataInicial)
        AND (@pDataFinal IS NULL OR v.DataVenda <= @pDataFinal)
    GROUP BY 
        v.IdVenda, v.DataVenda, v.DataFinalizacao, v.IdFuncionario, 
        f.CPF, f.Nome, v.Situacao, v.Cancelada, v.ValorFrete, f.Comissao
    ORDER BY 
        v.DataVenda ASC
    LIMIT @Limit OFFSET @Offset";


            int offset = (pagina - 1) * itensPorPagina;

            var parametrosPaginados = new List<MySqlParameter>
        {
            new MySqlParameter("@pIdFuncionario", idColaborador ?? (object)DBNull.Value),
            new MySqlParameter("@pDataInicial", dataInicial ?? (object)DBNull.Value),
            new MySqlParameter("@pDataFinal", dataFinal ?? (object)DBNull.Value),
            new MySqlParameter("@Limit", itensPorPagina),
            new MySqlParameter("@Offset", offset)
        };

            var list = new List<RelatorioComissaoVendasModel>();

            // Modifiquei aqui - Usando DALHelper consistentemente
            using (MySqlDataReader reader = ExecuteReader(queryPaginada, parametrosPaginados.ToArray()))
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new RelatorioComissaoVendasModel()
                    {
                        NumVenda = DALHelper.GetInt32(reader, "num_venda") ?? 0,
                        DataInicial = DALHelper.GetDateTime(reader, "data_venda") ?? DateTime.MinValue,
                        DataFinalizacao = DALHelper.GetDateTime(reader, "data_finalizacao") ?? DateTime.MinValue,
                        VendedorId = DALHelper.GetInt32(reader, "vendedor_id") ?? 0,
                        VendedorCpf = DALHelper.GetString(reader, "vendedor_cpf"),
                        VendedorNome = DALHelper.GetString(reader, "vendedor_nome"),
                        Situacao = DALHelper.GetInt32(reader, "situacao") ?? 0,
                        Cancelada = DALHelper.GetBool(reader, "cancelada"),
                        TotalVenda = DALHelper.GetDecimal(reader, "totalvenda"),
                        DescontoVenda = DALHelper.GetDecimal(reader, "desconto_venda"),
                        PercComissao = DALHelper.GetDecimal(reader, "perc_comissao"),
                        ValComissao = DALHelper.GetDecimal(reader, "val_comissao")
                    });
                }
            }

            return (list, totalRegistros, totalVendido, totalComissao);
        }
    }
}


