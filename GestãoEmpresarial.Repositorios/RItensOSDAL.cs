using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;

namespace GestãoEmpresarial.Repositorios
{
    public class RItensOSDAL : DatabaseConnection, IDAL<ItemOrdemServicoModel>
    {
        private readonly RProdutoDAL rProdutoDal;

        public RItensOSDAL(int idFuncionario) : base(idFuncionario)
        {
            rProdutoDal = new RProdutoDAL(idFuncionario);
        }

        public void Delete(ItemOrdemServicoModel t)
        {
            string query = "DELETE FROM tb_itensos WHERE IdItensOs = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdItensOs, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public List<ItemOrdemServicoModel> GetByIdOs(int id)
        {
            return List(id.ToString());
        }

        public ItemOrdemServicoModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(ItemOrdemServicoModel t)
        {
            string query = "INSERT INTO tb_itensos (Quantidade, ValUnitario, Desconto, ValTotal, IdOs, IdProduto) " +
          "  VALUES(@Quantidade, @ValUnitario, @Desconto, @ValTotal, @IdOs, @IdProduto);"
           + " SELECT last_insert_id()";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Quantidade", t.Quantidade);
            AddParameter(lista, "@ValUnitario", t.ValUnitario);
            AddParameter(lista, "@Desconto", t.Desconto);
            AddParameter(lista, "@ValTotal", t.CustoTotal);
            AddParameter(lista, "@IdOs", t.IdOs);
            AddParameter(lista, "@IdProduto", t.Produto.IdProduto);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<ItemOrdemServicoModel> List(string filtro)
        {
            string query = @"select IdOs, Desconto, IdItensOs, Quantidade, ValUnitario, IdProduto from tb_itensos WHERE IdOs = @idOs";

            List<ItemOrdemServicoModel> lista = new List<ItemOrdemServicoModel>();
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@idOs", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = new ItemOrdemServicoModel
                    {
                        IdOs = DALHelper.GetInt32(reader, "IdOs").Value,
                        //Acao = DALHelper.GetString(reader, "Acao"),
                        Desconto = DALHelper.GetDecimal(reader, "Desconto"),
                        IdItensOs = DALHelper.GetInt32(reader, "IdItensOs").Value,
                        //Produto = DALHelper.GetString(reader, "Box"),
                        Quantidade = DALHelper.GetInt32(reader, "Quantidade").Value,
                        ValUnitario = DALHelper.GetDecimal(reader, "ValUnitario"),
                    };

                    //string idProduto = DALHelper.GetString(reader, "IdProduto");
                    //if (string.IsNullOrWhiteSpace(idProduto) == false)
                    //{
                    //    obj.Produto = rProdutoDal.GetById(Convert.ToInt32(idProduto));
                    //}

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

        public void Update(ItemOrdemServicoModel t)
        {
            //não fazer nada
        }
    }
}
