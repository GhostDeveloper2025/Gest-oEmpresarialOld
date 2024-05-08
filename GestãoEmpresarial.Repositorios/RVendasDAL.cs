using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    /// <summary>
    /// CRUD
    /// - Create (Criar)
    /// - Read (Ler)
    /// - Update (Atualizar)
    /// - Delete (Apagar/Remover)
    /// </summary>
    public class RVendasDAL : DatabaseConnection, IDAL<VendaModel>
    {
        internal readonly RColaboradorDAL rColaboradorDAL;
        internal readonly RClienteDAL rClienteDAL;
        internal readonly RItensVendaDAL rItensVendaDAL;
        public RVendasDAL(int idFuncionario) : base(idFuncionario)
        {
            rColaboradorDAL = new RColaboradorDAL(idFuncionario);
            rItensVendaDAL = new RItensVendaDAL(idFuncionario);
            rClienteDAL = new RClienteDAL(idFuncionario);
        }

        public void Delete(VendaModel t)
        {
            string query = "DELETE FROM tb_venda WHERE IdVenda = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdVenda, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public VendaModel GetById(int id)
        {
            return GetLista(true, IdVenda: id).FirstOrDefault();
        }

        public int Insert(VendaModel t)
        {
            string query = "INSERT INTO tb_venda (DataVenda, DataFinalizacao, Situacao, IdFuncionario, IdCliente, ValorFrete, IdCodigoTipoPagamento) "
            + " VALUES(NOW(),@DataFinalizacao,@Situacao,@IdFuncionario,@IdCliente,@ValorFrete,@IdCodigoTipoPagamento);"
            + " SELECT last_insert_id()";
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar
            AddParameter(lista, "@idFuncionario", idFuncionario);
            AddParameter(lista, "@IdCliente", t.Cliente.Idcliente);
            AddParameter(lista, "@Situacao", t.Situacao);

            if (t.Situacao == 1)
                AddParameter(lista, "@DataFinalizacao", DateTime.Now);
            else
                AddParameter(lista, "@DataFinalizacao", null);

            AddParameter(lista, "@ValorFrete", t.ValorFrete);
            AddParameter(lista, "@IdCodigoTipoPagamento", t.IdCodigoTipoPagamento);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<VendaModel> List(string filtro)
        {
            return GetLista(false, filtro);
        }

        public List<VendaModel> GetLista(bool comItens, string nomeCliente = null, DateTime? dataEntrada = null, int? IdVenda = null)
        {
            string query = @"
                 SELECT IdVenda, DataVenda, DataFinalizacao, Situacao, ValorFrete, Idcliente, IdFuncionario, IdCodigoTipoPagamento
                 FROM tb_venda
                 WHERE (@nomeCliente IS NULL OR Idcliente IN (SELECT idcliente FROM tb_cliente WHERE nome LIKE CONCAT(@nomeCliente, '%')))
                 AND (@dataVenda IS NULL OR DataVenda = @dataVenda)
                 AND (@IdVenda IS NULL OR IdVenda = @idVenda)
                 LIMIT 200";  // Adicionando LIMIT 200 para limitar o número de registros

            List<VendaModel> lista = new List<VendaModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@nomeCliente", nomeCliente);
            AddParameter(parametros, "@dataVenda", dataEntrada);
            AddParameter(parametros, "@IdVenda", IdVenda);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = new VendaModel
                    {
                        IdVenda = reader.GetInt32("IdVenda"),
                        DataVenda = reader.GetDateTime("DataVenda"),
                        DataFinalizacao = reader.GetDateTime("DataFinalizacao"),
                        IdCodigoTipoPagamento = reader.GetInt32("IdCodigoTipoPagamento"),
                    };

                    if (comItens)
                    {
                        obj.ListItensVenda = rItensVendaDAL.GetByIdVenda(obj.IdVenda);
                    }

                    int? idCliente = DALHelper.GetInt32(reader, "IdCliente");
                    if (idCliente.HasValue)
                    {
                        obj.Cliente = rClienteDAL.GetById(idCliente.Value);
                    }
                    int? idCadastrante = DALHelper.GetInt32(reader, "IdFuncionario");
                    if (idCadastrante.HasValue)
                    {
                        obj.Cadastrante = rColaboradorDAL.GetById(idCadastrante.Value);
                    }

                    lista.Add(obj);
                }
            }

            return lista;
        }

        public void Update(VendaModel t)
        {
            throw new NotImplementedException();
        }

    }
}
