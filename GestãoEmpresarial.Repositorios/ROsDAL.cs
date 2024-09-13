using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    public class ROsDAL : DatabaseConnection, IDAL<OrdemServicoModel>
    {
        internal readonly RColaboradorDAL rColaboradorDAL;
        internal readonly RClienteDAL rClienteDAL;
        internal readonly RItensOSDAL rItensOSDAL;

        public ROsDAL(int idfuncionario) : base(idfuncionario)
        {
            rColaboradorDAL = new RColaboradorDAL(idFuncionario);
            rItensOSDAL = new RItensOSDAL(idFuncionario);
            rClienteDAL = new RClienteDAL(idFuncionario);
        }

        public bool PodeEditar(int statusId)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "statusId", statusId);
            object existe = ExecuteScalar("usp_pode_editar_ordem_servico", lista.ToArray(), true);
            return Convert.ToBoolean(existe);
        }

        public void Delete(OrdemServicoModel t)
        {
            string query = "DELETE FROM tb_os WHERE IdOs = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdOs, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public OrdemServicoModel GetById(int id)
        {
            return GetLista(true, idOs: id).FirstOrDefault();
        }

        public int Insert(OrdemServicoModel t)
        {
            string query = "INSERT INTO tb_os (DataEntrada, DataFinalizacao, IdCadastrante, IdCliente, Finalizado, IdCodigoStatus, Ferramenta, IdCodigoMarcasF, Modelo, Obs, IdTecnico, IdResponsavel, TotalMaoObra, Box, Garantia, SubTotalProduto, DescontoProduto, TotalProduto, TotalOS) "
            + " VALUES(NOW(),@DataFinalizacao,@IdCadastrante,@IdCliente,@Finalizado,@Status,@Ferramenta,@Marca,@Modelo,@Obs,@IdTecnico,@IdResponsavel,@TotalMaoObra,@Box,@Garantia,@SubTotalProduto,@DescontoProduto,@TotalProduto,@TotalOS);"
            + " SELECT last_insert_id()";
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar
            AddParameter(lista, "@DataFinalizacao", t.DataFinalizacao);
            AddParameter(lista, "@IdCadastrante", idFuncionario);
            AddParameter(lista, "@IdCliente", t.Cliente.Idcliente);
            AddParameter(lista, "@Finalizado", t.Finalizado);
            AddParameter(lista, "@Status", t.Status);
            AddParameter(lista, "@Ferramenta", t.Ferramenta);
            AddParameter(lista, "@Marca", t.Marca);
            AddParameter(lista, "@Modelo", t.Modelo);
            AddParameter(lista, "@Obs", t.Obs);
            AddParameter(lista, "@IdTecnico", t.IdTecnico);
            AddParameter(lista, "@IdResponsavel", t.IdResponsavel);
            AddParameter(lista, "@TotalMaoObra", t.TotalMaoObra);
            AddParameter(lista, "@Box", t.Box);
            AddParameter(lista, "@Garantia", t.Garantia);
            AddParameter(lista, "@SubTotalProduto", t.SubTotalProduto);
            AddParameter(lista, "@DescontoProduto", t.TotalDescontoProduto);
            AddParameter(lista, "@TotalProduto", t.TotalProduto);
            AddParameter(lista, "@TotalOS", t.TotalOS);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<OrdemServicoModel> List(string filtro)
        {
            return GetLista(false, filtro);
        }


        #region Codigo que eu tentei fazer
        public List<OrdemServicoModel> GetLista(bool comItens, string nomeCliente = null, int? idStatus = null, DateTime? dataEntrada = null, int? idOs = null)
        {
            string query = @"
                 SELECT IdOs, DataEntrada, DataFinalizacao, IdCadastrante, IdCliente, Finalizado, Ferramenta, Modelo, Obs, IdTecnico,
                 IdResponsavel, TotalMaoObra, Box, Garantia, SubTotalProduto, DescontoProduto, TotalProduto, TotalOS, idcodigostatus, idcodigomarcasf
                 FROM tb_os
                 WHERE (@nomeCliente IS NULL OR Idcliente IN (SELECT idcliente FROM tb_cliente WHERE nome LIKE CONCAT(@nomeCliente, '%')))
                 AND (@idStatus IS NULL OR @idStatus = 0 OR idcodigostatus = @idStatus)
                 AND (@dataEntrada IS NULL OR DataEntrada = @dataEntrada)
                 AND (@idOs IS NULL OR IdOS = @idOs)
                 LIMIT 200";  // Adicionando LIMIT 200 para limitar o número de registros

            List<OrdemServicoModel> lista = new List<OrdemServicoModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@nomeCliente", nomeCliente);
            AddParameter(parametros, "@idStatus", idStatus);
            AddParameter(parametros, "@dataEntrada", dataEntrada);
            AddParameter(parametros, "@idOs", idOs);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = Mapeador.Map(new OrdemServicoModel(), reader);
                    if (comItens)
                    {
                        obj.ListItensOs = rItensOSDAL.GetByIdOs(obj.IdOs);
                    }

                    obj.Cliente = rClienteDAL.GetById(obj.IdCliente);
                    obj.Cadastrante = rColaboradorDAL.GetById(obj.IdCadastrante);

                    if (obj.IdResponsavel.HasValue)
                    {
                        obj.Responsavel = rColaboradorDAL.GetById(obj.IdResponsavel.Value);
                    }

                    if (obj.IdTecnico.HasValue)
                    {
                        obj.Tecnico = rColaboradorDAL.GetById(obj.IdTecnico.Value);
                    }

                    lista.Add(obj);
                }
            }

            return lista;
        }


        #endregion

        public void Update(OrdemServicoModel t)
        {
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "id", t.IdOs);
            AddParameter(lista, "statusId", t.Status);
            AddParameter(lista, "obsv", t.Obs);
            AddParameter(lista, "box", t.Box);
            AddParameter(lista, "Garantia", t.Garantia);
            AddParameter(lista, "idTecnico", t.Tecnico?.IdFuncionario);
            AddParameter(lista, "idResponsavel", t.Responsavel?.IdFuncionario);
            AddParameter(lista, "TotalMaoObra", t.TotalMaoObra);
            AddParameter(lista, "SubTotalProduto", t.SubTotalProduto);
            AddParameter(lista, "DescontoProduto", t.TotalDescontoProduto);
            AddParameter(lista, "TotalProduto", t.TotalProduto);
            AddParameter(lista, "TotalOS", t.TotalOS);
            // Atualize a DataFinalizacao para a data e hora atuais
            //t.DataFinalizacao = DateTime.Now;
            //AddParameter(lista, "DataFinalizacao", t.DataFinalizacao);

            ExecuteNonQuery("usp_upd_ordem_servico", lista.ToArray(), true);
        }

    }
}
