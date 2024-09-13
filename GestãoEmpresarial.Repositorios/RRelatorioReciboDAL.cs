using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatorioReciboDAL : DatabaseConnection
    {
        internal readonly RItensOSDAL rItensOSDAL;
        internal readonly RItensVendaDAL rItensVendaDAL;

        public RRelatorioReciboDAL(int idFuncionario) : base(idFuncionario)
        {
            rItensOSDAL = new RItensOSDAL(idFuncionario);
            rItensVendaDAL = new RItensVendaDAL(idFuncionario);
        }

        public RelatorioReciboOsModel ObterReciboOrdemServico(int idOs)
        {
            string query = @"
                 SELECT a.*, resp.Nome AS NomeResponsavel, tecnico.Nome AS NomeTecnico, cadas.Nome AS NomeCadastrante, cli.Nome AS NomeCliente, codigof.NomeCodigo as NomeMarca, codigostatus.NomeCodigo as NomeStatus
                    FROM dbnew.tb_os a
                    INNER JOIN dbnew.tb_cliente cli on a.IdCliente = cli.IdCliente
                    INNER JOIN dbnew.tb_funcionario cadas on a.IdCadastrante = cadas.IdFuncionario
                    INNER JOIN dbnew.uvw_codigos codigof on a.idcodigomarcasf = codigof.IdCodigo
                    INNER JOIN dbnew.uvw_codigos codigostatus on a.idcodigostatus = codigostatus.IdCodigo
                    LEFT JOIN dbnew.tb_funcionario tecnico on a.IdTecnico = tecnico.IdFuncionario
                    LEFT JOIN dbnew.tb_funcionario resp on a.IdResponsavel = resp.IdFuncionario
                    where idos = @idOs;";

            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idOs", idOs);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = Mapeador.Map(new OrdemServicoModel(), reader);
                    obj.ListItensOs = rItensOSDAL.GetByIdOs(obj.IdOs);

                    return new RelatorioReciboOsModel
                    {
                        OsModel = obj,
                        NomeCadastrante = DALHelper.GetString(reader, "NomeCadastrante"),
                        NomeCliente = DALHelper.GetString(reader, "NomeCliente"),
                        NomeMarca = DALHelper.GetString(reader, "NomeMarca"),
                        NomeResponsavel = DALHelper.GetString(reader, "NomeResponsavel"),
                        NomeStatus = DALHelper.GetString(reader, "NomeStatus"),
                        NomeTecnico = DALHelper.GetString(reader, "NomeTecnico"),
                    };
                }
            }

            return null;
        }

        public RelatorioReciboVendaModel ObterReciboVenda(int idVenda)
        {
            string query = @"
                 SELECT a.*, vend.Nome AS NomeVendedor, codigoPaga.NomeCodigo as NomePagamento
                   FROM dbnew.tb_venda a
                   INNER JOIN dbnew.tb_funcionario vend on a.IdFuncionario = vend.IdFuncionario
                   INNER JOIN dbnew.uvw_codigos codigoPaga on a.IdCodigoTipoPagamento = codigoPaga.IdCodigo
                   where idVenda = @idVenda;";

            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idVenda", idVenda);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = Mapeador.Map(new VendaModel(), reader);
                    obj.ListItensVenda = rItensVendaDAL.GetByIdVenda(obj.IdVenda);

                    return new RelatorioReciboVendaModel
                    {
                        VendaModel = obj,
                        NomeVendedor = DALHelper.GetString(reader, "NomeVendedor"),
                        NomePagamento = DALHelper.GetString(reader, "NomePagamento"),
                    };
                }
            }

            return null;
        }
    }
}
