using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;

namespace GestãoEmpresarial.Repositorios
{
    public class RItensVendaDAL : DatabaseConnection, IDAL<ItemVendaModel>
    {

        private readonly RProdutoDAL rProdutoDal;
        public RItensVendaDAL(int idFuncionario) : base(idFuncionario)
        {
            rProdutoDal = new RProdutoDAL(idFuncionario);
        }

        public void Delete(ItemVendaModel t)
        {
            string query = "DELETE FROM tb_itensvenda WHERE IdItensVenda = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdItensVenda, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public List<ItemVendaModel> GetByIdVenda(int id)
        {
            return List(id.ToString());
        }
        public ItemVendaModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(ItemVendaModel t)
        {
            string query = "INSERT INTO tb_itensvenda (Quantidade, ValUnitario, Desconto, ValTotal, IdVenda, IdProduto) " +
        "  VALUES(@Quantidade, @ValUnitario, @Desconto, @ValTotal, @IdVenda, @IdProduto);"
         + " SELECT last_insert_id()";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Quantidade", t.Quantidade);
            AddParameter(lista, "@ValUnitario", t.ValUnitario);
            AddParameter(lista, "@Desconto", t.Desconto);
            AddParameter(lista, "@ValTotal", t.CustoTotal);
            AddParameter(lista, "@IdVenda", t.IdVenda);
            AddParameter(lista, "@IdProduto", t.Produto.IdProduto);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<ItemVendaModel> List(string filtro)
        {
            string query = @"select IdVenda, Desconto, IdItensVenda, Quantidade, ValUnitario, IdProduto from tb_itensvenda WHERE IdVenda = @IdVenda";

            List<ItemVendaModel> lista = new List<ItemVendaModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@IdVenda", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = new ItemVendaModel
                    {
                        IdVenda = DALHelper.GetInt32(reader, "IdVenda").Value,
                        //Acao = DALHelper.GetString(reader, "Acao"),
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        IdItensVenda = DALHelper.GetInt32(reader, "IdItensVenda").Value,
                        //Produto = DALHelper.GetString(reader, "Box"),
                        Quantidade = DALHelper.GetInt32(reader, "Quantidade").Value,
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
                    };

                    int? idProduto = DALHelper.GetInt32(reader, "IdProduto");
                    if (idProduto.HasValue)
                    {
                        obj.Produto = rProdutoDal.GetById(idProduto.Value);
                    }
                    lista.Add(obj);
                }
            }

            return lista;
        }

        public void Update(ItemVendaModel t)
        {
            //não fazer nada
        }
    }
}
