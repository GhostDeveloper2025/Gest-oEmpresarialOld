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
    namespace GestãoEmpresarial.Repositorios
    {
        public class RRelatoriosDAL : DatabaseConnection
        {
            public RRelatoriosDAL(int idFuncionario) : base(idFuncionario)
            {
            }

            public List<RelatorioProdutoMaisVendido> ObterProdutoMaisVendido(DateTime? dataInicial, DateTime? dataFinal)
            {
                string query = "SELECT * FROM dbnew.uvw_relatorio_produto_mais_vendido WHERE 1=1";

                if (dataInicial.HasValue)
                {
                    query += $" AND DataFinalizacao >= '{dataInicial.Value:yyyy-MM-dd}'";
                }

                if (dataFinal.HasValue)
                {
                    query += $" AND DataFinalizacao <= '{dataFinal.Value:yyyy-MM-dd}'";
                }

                var list = new List<RelatorioProdutoMaisVendido>();
                using (MySqlDataReader reader = ExecuteReader(query))
                {
                    while (reader.Read())
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
                return list;
            }
        }
    }

}
