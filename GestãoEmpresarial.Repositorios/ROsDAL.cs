using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    public class ROsDAL : DatabaseConnection, IDAL<OsModel>
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

        public void Delete(OsModel t)
        {
            string query = "DELETE FROM tb_os WHERE IdOs = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdOs, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public OsModel GetById(int id)
        {
            return GetLista(true, idOs: id).FirstOrDefault();
        }

        public int Insert(OsModel t)
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

        public List<OsModel> List(string filtro)
        {
            return GetLista(false, filtro);
        }


        #region Codigo que eu tentei fazer
        public List<OsModel> GetLista(bool comItens, string nomeCliente = null, int? idStatus = null, DateTime? dataEntrada = null, int? idOs = null)
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

            List<OsModel> lista = new List<OsModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@nomeCliente", nomeCliente);
            AddParameter(parametros, "@idStatus", idStatus);
            AddParameter(parametros, "@dataEntrada", dataEntrada);
            AddParameter(parametros, "@idOs", idOs);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = new OsModel
                    {
                        IdOs = reader.GetInt32("IdOs"),
                        DataEntrada = reader.GetDateTime("DataEntrada"),
                        DataFinalizacao = DALHelper.GetDateTime(reader, "DataFinalizacao"),
                        Ferramenta = DALHelper.GetString(reader, "Ferramenta"),
                        Modelo = DALHelper.GetString(reader, "Modelo"),
                        Obs = DALHelper.GetString(reader, "Obs"),
                        TotalDescontoProduto = DALHelper.GetDecimal(reader, "DescontoProduto"),
                        SubTotalProduto = DALHelper.GetDecimal(reader, "SubTotalProduto"),
                        TotalProduto = DALHelper.GetDecimal(reader, "TotalProduto"),
                        TotalOS = DALHelper.GetDecimal(reader, "TotalOS"),
                        TotalMaoObra = DALHelper.GetDecimal(reader, "TotalMaoObra"),
                        Box = DALHelper.GetString(reader, "Box"),
                        Finalizado = DALHelper.GetBool(reader, "Finalizado"),
                        Garantia = DALHelper.GetBool(reader, "Garantia"),
                        Status = reader.GetInt32("idcodigostatus"),
                        Marca = DALHelper.GetInt32(reader, "idcodigomarcasf").Value,
                    };

                    if (comItens)
                    {
                        obj.ListItensOs = rItensOSDAL.GetByIdOs(obj.IdOs);
                    }

                    int? idCliente = DALHelper.GetInt32(reader, "IdCliente");
                    if (idCliente.HasValue)
                    {
                        obj.Cliente = rClienteDAL.GetById(idCliente.Value);
                    }

                    int? idResponsavel = DALHelper.GetInt32(reader, "IdResponsavel");
                    if (idResponsavel.HasValue)
                    {
                        obj.Responsavel = rColaboradorDAL.GetById(idResponsavel.Value);
                    }
                    int? IdTecnico = DALHelper.GetInt32(reader, "IdTecnico");
                    if (IdTecnico.HasValue)
                    {
                        obj.Tecnico = rColaboradorDAL.GetById(IdTecnico.Value);
                    }
                    int? idCadastrante = DALHelper.GetInt32(reader, "IdCadastrante");
                    if (idCadastrante.HasValue)
                    {
                        obj.Cadastrante = rColaboradorDAL.GetById(idCadastrante.Value);
                    }

                    lista.Add(obj);
                }
            }

            return lista;
        }


        #endregion

        public void Update(OsModel t)
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
