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

        //public async Task<RelatorioReciboOsModel> ObterReciboOrdemServicoAsync(int idOs)
        //{
        //    string query = @"
        //         SELECT a.*, resp.Nome AS NomeResponsavel, tecnico.Nome AS NomeTecnico, cadas.Nome AS NomeCadastrante, cli.Nome AS NomeCliente, codigof.NomeCodigo as NomeMarca, codigostatus.NomeCodigo as NomeStatus
        //            FROM dbnew.tb_os a
        //            INNER JOIN dbnew.tb_cliente cli on a.IdCliente = cli.IdCliente
        //            INNER JOIN dbnew.tb_funcionario cadas on a.IdCadastrante = cadas.IdFuncionario
        //            INNER JOIN dbnew.uvw_codigos codigof on a.idcodigomarcasf = codigof.IdCodigo
        //            INNER JOIN dbnew.uvw_codigos codigostatus on a.idcodigostatus = codigostatus.IdCodigo
        //            LEFT JOIN dbnew.tb_funcionario tecnico on a.IdTecnico = tecnico.IdFuncionario
        //            LEFT JOIN dbnew.tb_funcionario resp on a.IdResponsavel = resp.IdFuncionario
        //            where idos = @idOs;";

        //    List<MySqlParameter> parametros = new List<MySqlParameter>();
        //    AddParameter(parametros, "@idOs", idOs);

        //    using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
        //    {
        //        while (await reader.ReadAsync()) // Leitura assíncrona
        //        {
        //            var obj = Mapeador.Map(new OrdemServicoModel(), reader);
        //            obj.ListItensOs = await rItensOSDAL.GetByIdOsAsync(obj.IdOs); // Usar await aqui

        //            return new RelatorioReciboOsModel
        //            {
        //                OsModel = obj,
        //                NomeCadastrante = DALHelper.GetString(reader, "NomeCadastrante"),
        //                NomeCliente = DALHelper.GetString(reader, "NomeCliente"),
        //                NomeMarca = DALHelper.GetString(reader, "NomeMarca"),
        //                NomeResponsavel = DALHelper.GetString(reader, "NomeResponsavel"),
        //                NomeStatus = DALHelper.GetString(reader, "NomeStatus"),
        //                NomeTecnico = DALHelper.GetString(reader, "NomeTecnico"),
        //            };
        //        }
        //    }

        //    return null;
        //}


        // Aqui trago os registro para mostrar no recibo ordemserviço e relatorios (novo codigo)
        public async Task<RelatorioReciboOsModel> ObterReciboOrdemServicoAsync(int idOs)
        {
            string query = @"
         SELECT a.*, resp.Nome AS NomeResponsavel, tecnico.Nome AS NomeTecnico, 
                cadas.Nome AS NomeCadastrante, cli.Nome AS NomeCliente, cli.Celular AS CelularCliente, cli.Cnpj AS CnpjCliente,
                codigof.NomeCodigo as NomeMarca, codigostatus.NomeCodigo as NomeStatus
            FROM dbnew.tb_os a
            INNER JOIN dbnew.tb_cliente cli on a.IdCliente = cli.IdCliente
            INNER JOIN dbnew.tb_funcionario cadas on a.IdCadastrante = cadas.IdFuncionario
            INNER JOIN dbnew.uvw_codigos codigof on a.idcodigomarcasf = codigof.IdCodigo
            INNER JOIN dbnew.uvw_codigos codigostatus on a.idcodigostatus = codigostatus.IdCodigo
            LEFT JOIN dbnew.tb_funcionario tecnico on a.IdTecnico = tecnico.IdFuncionario
            LEFT JOIN dbnew.tb_funcionario resp on a.IdResponsavel = resp.IdFuncionario
            WHERE a.IdOs = @idOs;";

            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idOs", idOs);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (await reader.ReadAsync()) // Leitura assíncrona
                {
                    var obj = Mapeador.Map(new OrdemServicoModel(), reader);
                    obj.ListItensOs = await rItensOSDAL.GetByIdOsAsync(obj.IdOs); // Usar await aqui

                    return new RelatorioReciboOsModel
                    {
                        OsModel = obj,
                        NomeCadastrante = DALHelper.GetString(reader, "NomeCadastrante"),
                        NomeCliente = DALHelper.GetString(reader, "NomeCliente"),
                        CelularCliente = DALHelper.GetString(reader, "CelularCliente"), // Adicionando CelularCliente
                        CnpjCliente = DALHelper.GetString(reader, "CnpjCliente"), // Adicionando CNPJ
                        NomeMarca = DALHelper.GetString(reader, "NomeMarca"),
                        NomeResponsavel = DALHelper.GetString(reader, "NomeResponsavel"),
                        NomeStatus = DALHelper.GetString(reader, "NomeStatus"),
                        NomeTecnico = DALHelper.GetString(reader, "NomeTecnico"),
                    };
                }
            }

            return null;
        }


        public async Task<RelatorioReciboVendaModel> ObterReciboVendaAsync(int idVenda)
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
                while (await reader.ReadAsync()) // Leitura assíncrona
                {
                    var obj = Mapeador.Map(new VendaModel(), reader);
                    obj.ListItensVenda = await rItensVendaDAL.GetByIdVendaAsync(obj.IdVenda); // Usar await aqui

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

        // Métodos síncronos para compatibilidade
        public RelatorioReciboOsModel ObterReciboOrdemServico(int idOs)
        {
            return ObterReciboOrdemServicoAsync(idOs).GetAwaiter().GetResult();
        }

        public RelatorioReciboVendaModel ObterReciboVenda(int idVenda)
        {
            return ObterReciboVendaAsync(idVenda).GetAwaiter().GetResult();
        }
    }
}


