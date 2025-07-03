using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatorioHistoricoVendasDAL : DatabaseConnection
    {
        public RRelatorioHistoricoVendasDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public async Task<(List<RelatorioHistoricoVendasModel> Itens, int TotalRegistros, Decimal TotalVendido)> ObterHistoricoVendasAsync(
        int? clienteId, int? idProduto, DateTime? dataInicial, DateTime? dataFinal, int pagina, int itensPorPagina)
        {
            // 1. Contagem total de registros
            string queryCount = @"
                    SELECT 
                        COUNT(DISTINCT v.IdVenda) AS TotalRegistos,
                        COALESCE(CAST(ROUND(
                            SUM(iv.ValTotal) + 
                            SUM(v.ValorFrete) - 
                            SUM(ROUND((iv.Desconto / 100) * (iv.Quantidade * iv.ValUnitario), 2))
                        , 2) AS DECIMAL(10,2)), 0) AS TotalVendido
                    FROM tb_venda v
                    INNER JOIN tb_cliente c ON c.Idcliente = v.Idcliente
                    INNER JOIN tb_funcionario f ON f.IdFuncionario = v.IdFuncionario
                    INNER JOIN tb_itensvenda iv ON iv.IdVenda = v.IdVenda
                    WHERE v.Situacao = 1
                    AND v.Cancelada = 0
                    AND (@clienteId IS NULL OR c.Idcliente = @clienteId)
                    AND (@idProduto IS NULL OR iv.IdProduto = @idProduto)
                    AND (@dataInicial IS NULL OR v.DataVenda >= @dataInicial)
                    AND (@dataFinal IS NULL OR v.DataVenda <= @dataFinal)";

            var parametrosCount = new List<MySqlParameter>
        {
            new MySqlParameter("@clienteId", clienteId ?? (object)DBNull.Value),
            new MySqlParameter("@idProduto", idProduto ?? (object)DBNull.Value),
            new MySqlParameter("@dataInicial", dataInicial ?? (object)DBNull.Value),
            new MySqlParameter("@dataFinal", dataFinal ?? (object)DBNull.Value)
        };
            int totalRegistros = 0;
            decimal totalVendido = 0;
           
            // Modifiquei aqui - Usando o DALHelper.GetDecimal para tratar valores nulos
            using (MySqlDataReader reader = ExecuteReader(queryCount, parametrosCount.ToArray()))
            {
                if (reader.Read())
                {
                    totalRegistros = DALHelper.GetInt32(reader, "TotalRegistos") ?? 0;
                    totalVendido = DALHelper.GetDecimal(reader, "TotalVendido"); // Modifiquei aqui
                }
            }


            // 2. Obter dados paginados
            string queryPaginada = @"
                    SELECT 
                        v.IdVenda AS num_venda,
                        CAST(DATE_FORMAT(v.DataVenda, '%Y-%m-%d') AS DATE) AS data_emissao,
                        v.DataVenda AS datahora_emissao,
                        CAST(DATE_FORMAT(v.DataFinalizacao, '%Y-%m-%d') AS DATE) AS data_finalizacao,
                        c.Idcliente AS cliente_id,
                        c.CPF AS cliente_cpf,
                        c.Nome AS cliente_nome,
                        f.IdFuncionario AS vendedor_id,
                        f.CPF AS vendedor_cpf,
                        f.Nome AS vendedor_nome,
                        v.Situacao AS situacao,
                        CAST(SUM(iv.ValTotal) AS DECIMAL(10, 2)) AS totalprodutos,
                        CAST(SUM(ROUND((iv.Desconto / 100) * (iv.Quantidade * iv.ValUnitario), 2)) AS DECIMAL(10, 2)) AS totaldesconto,
                        CAST(SUM(iv.ValTotal) AS DECIMAL(10, 2)) AS subtotal,
                        CAST(v.ValorFrete AS DECIMAL(10, 2)) AS totalfrete,
                        CAST(ROUND(SUM(iv.ValTotal) + v.ValorFrete - SUM(ROUND((iv.Desconto / 100) * (iv.Quantidade * iv.ValUnitario), 2)), 2) AS DECIMAL(10, 2)) AS totalvenda,
                        SUM(iv.Quantidade) AS quantidade_total
                    FROM
                        tb_venda v
                    INNER JOIN tb_cliente c ON c.Idcliente = v.Idcliente
                    INNER JOIN tb_funcionario f ON f.IdFuncionario = v.IdFuncionario
                    INNER JOIN tb_itensvenda iv ON iv.IdVenda = v.IdVenda
                    WHERE v.Situacao = 1
                    AND v.Cancelada = 0
                    AND (@clienteId IS NULL OR c.Idcliente = @clienteId)
                    AND (@idProduto IS NULL OR iv.IdProduto = @idProduto)
                    AND (@dataInicial IS NULL OR v.DataVenda >= @dataInicial)
                    AND (@dataFinal IS NULL OR v.DataVenda <= @dataFinal)
                    GROUP BY v.IdVenda
                    ORDER BY v.IdVenda ASC
                    LIMIT @Limit OFFSET @Offset";

            int offset = (pagina - 1) * itensPorPagina;

            var parametrosPaginados = new List<MySqlParameter>
        {
            new MySqlParameter("@clienteId", clienteId ?? (object)DBNull.Value),
            new MySqlParameter("@idProduto", idProduto ?? (object)DBNull.Value),
            new MySqlParameter("@dataInicial", dataInicial ?? (object)DBNull.Value),
            new MySqlParameter("@dataFinal", dataFinal ?? (object)DBNull.Value),
            new MySqlParameter("@Limit", itensPorPagina),
            new MySqlParameter("@Offset", offset)
        };

            var list = new List<RelatorioHistoricoVendasModel>();

            using (MySqlDataReader reader = ExecuteReader(queryPaginada, parametrosPaginados.ToArray()))
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new RelatorioHistoricoVendasModel()
                    {
                        // Modifiquei aqui - Usando os helpers para todos os campos
                        NumVenda = DALHelper.GetInt32(reader, "num_venda") ?? 0,
                        DataEmissao = DALHelper.GetDateTime(reader, "data_emissao") ?? DateTime.MinValue,
                        DataHoraEmissao = DALHelper.GetDateTime(reader, "datahora_emissao") ?? DateTime.MinValue,
                        DataFinalizacao = DALHelper.GetDateTime(reader, "data_finalizacao") ?? DateTime.MinValue,
                        ClienteId = DALHelper.GetInt32(reader, "cliente_id") ?? 0,
                        ClienteCpf = DALHelper.GetString(reader, "cliente_cpf"),
                        ClienteNome = DALHelper.GetString(reader, "cliente_nome"),
                        VendedorId = DALHelper.GetInt32(reader, "vendedor_id") ?? 0,
                        VendedorCpf = DALHelper.GetString(reader, "vendedor_cpf"),
                        VendedorNome = DALHelper.GetString(reader, "vendedor_nome"),
                        Situacao = DALHelper.GetInt32(reader, "situacao") ?? 0,
                        TotalProdutos = DALHelper.GetDecimal(reader, "totalprodutos"),
                        TotalDesconto = DALHelper.GetDecimal(reader, "totaldesconto"),
                        Subtotal = DALHelper.GetDecimal(reader, "subtotal"),
                        TotalFrete = DALHelper.GetDecimal(reader, "totalfrete"),
                        TotalVenda = DALHelper.GetDecimal(reader, "totalvenda"),
                        QuantidadeTotal = DALHelper.GetInt32(reader, "quantidade_total") ?? 0
                    });
                }
            }

            return (list, totalRegistros, totalVendido);
        }
    }
}




