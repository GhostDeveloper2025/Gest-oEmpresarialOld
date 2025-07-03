using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatorioHistoricoOsDAL : DatabaseConnection
    {
        public RRelatorioHistoricoOsDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public async Task<(List<RelatorioHistoricoOsModel>, int, decimal)> ObterHistoricoOsAsync(int? clienteId, int? idTecnico, DateTime dataInicial, DateTime dataFinal, int? statusId = null, int pagina = 1, int tamanhoPagina = 20)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>();

            AddParameter(parametros, "varStatusId", statusId);
            AddParameter(parametros, "varIdCliente", clienteId);
            AddParameter(parametros, "varIdTecnico", idTecnico);
            AddParameter(parametros, "varDataInicial", dataInicial);
            AddParameter(parametros, "varDataFinal", dataFinal);
            AddParameter(parametros, "varPageNumber", pagina);
            AddParameter(parametros, "varPageSize", tamanhoPagina);

            // Parâmetros de saída
            MySqlParameter totalRegistrosParam = new MySqlParameter("TotalRegistros", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            parametros.Add(totalRegistrosParam);

            MySqlParameter totalVendidoParam = new MySqlParameter("TotalVendido", MySqlDbType.Decimal)
            {
                Direction = ParameterDirection.Output,
                Precision = 18,
                Scale = 2
            };
            parametros.Add(totalVendidoParam);

            var list = new List<RelatorioHistoricoOsModel>();

            using (MySqlDataReader reader = ExecuteReader("usp_relatorio_os", parametros.ToArray(), true))
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new RelatorioHistoricoOsModel()
                    {
                        // Modifiquei aqui - Usando DALHelper para todos os campos
                        NumOs = DALHelper.GetInt32(reader, "num_os") ?? 0,
                        DataEntrada = DALHelper.GetDateTime(reader, "data_entrada") ?? DateTime.MinValue,
                        DataFinalizacao = DALHelper.GetDateTime(reader, "data_finalizacao"),
                        ClienteId = DALHelper.GetInt32(reader, "cliente_id") ?? 0,
                        ClienteCpf = DALHelper.GetString(reader, "cliente_cpf"),
                        ClienteNome = DALHelper.GetString(reader, "cliente_nome"),
                        TecnicoId = DALHelper.GetInt32(reader, "tecnico_id"),
                        TecnicoCpf = DALHelper.GetString(reader, "tecnico_cpf"),
                        TecnicoNome = DALHelper.GetString(reader, "tecnico_nome"),
                        Status = DALHelper.GetString(reader, "status"),
                        TotalProdutos = DALHelper.GetDecimal(reader, "total_produtos"),
                        TotalDesconto = DALHelper.GetDecimal(reader, "total_desconto"),
                        Subtotal = DALHelper.GetDecimal(reader, "subtotal"),
                        TotalMaoObra = DALHelper.GetDecimal(reader, "total_mao_obra"),
                        TotalOs = DALHelper.GetDecimal(reader, "total_os"),
                        QuantidadeTotal = DALHelper.GetInt32(reader, "quantidade_total") ?? 0
                    });
                }
            }

            // Modifiquei aqui - Tratamento seguro para os parâmetros de saída
            int totalRegistros = totalRegistrosParam.Value != DBNull.Value ? Convert.ToInt32(totalRegistrosParam.Value) : 0;
            decimal totalVendido = totalVendidoParam.Value != DBNull.Value ? Convert.ToDecimal(totalVendidoParam.Value) : 0m;

            return (list, totalRegistros, totalVendido);
        }

        public List<RelatorioHistoricoOsModel> ObterHistoricoOs(int? clienteId, int? idTecnico, DateTime dataInicial, DateTime dataFinal, int? statusId = null, int pagina = 1, int tamanhoPagina = 20)
        {
            return ObterHistoricoOsAsync(clienteId, idTecnico, dataInicial, dataFinal, statusId, pagina, tamanhoPagina)
                .GetAwaiter().GetResult().Item1;
        }
    }
}
