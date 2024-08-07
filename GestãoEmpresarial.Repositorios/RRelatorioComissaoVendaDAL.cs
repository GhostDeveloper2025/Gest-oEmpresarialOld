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

        public List<RelatorioComissaoVendasModel> ObterComissaoVendas(int? idColaborador, DateTime? dataInicial, DateTime? dataFinal)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>();

            // Adicionando os parâmetros para a stored procedure
            AddParameter(parametros, "pIdFuncionario", idColaborador);
            AddParameter(parametros, "pDataInicial", dataInicial);
            AddParameter(parametros, "pDataFinal", dataFinal);

            var list = new List<RelatorioComissaoVendasModel>();

            using (MySqlDataReader reader = ExecuteReader("usp_Comecao_Venda", parametros.ToArray(), true))
            {
                while (reader.Read())
                {
                    list.Add(new RelatorioComissaoVendasModel()
                    {
                        NumVenda = reader.GetInt32("num_venda"),
                        DataInicial = reader.GetDateTime("data_Venda"),
                        DataFinalizacao = reader.GetDateTime("data_finalizacao"),
                        VendedorId = reader.GetInt32("vendedor_id"),
                        VendedorCpf = reader.GetString("vendedor_cpf"),
                        VendedorNome = reader.GetString("vendedor_nome"),
                        Situacao = reader.GetString("situacao"),
                        TotalVenda = reader.GetDecimal("totalvenda"),
                        PercComissao = reader.GetDecimal("perc_comissao"),
                        ValComissao = reader.GetDecimal("val_comissao")
                    });
                }
            }

            return list;
        }
    }
}

