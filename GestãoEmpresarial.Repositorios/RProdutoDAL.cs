using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    public class RProdutoDAL : DatabaseConnection, IDAL<ProdutoModel>
    {
        private const string SelectQuery = "SELECT a.IdProduto, a.Nome AS NomeProduto"
                + ", CodProduto, a.Descricao AS DescricaoProduto, IdMarcaF, ValorCusto, ValorVenda"
                + ", DataCadastro, a.IdCategoria, IdFuncionario"
            + " , IdEstoque, Localizacao, Quantidade"
            + " , c.Nome AS NomeCategoria, c.Descricao AS DescricaoCategoria"
                + " FROM tb_produto a"
            + " INNER JOIN tb_estoque b ON a.IdProduto = b.IdProduto "
            + " INNER JOIN tb_categoria c on c.IdCategoria = a.IdCategoria";

        private readonly REstoqueDAL restoque;
        private readonly RColaboradorDAL colaboradorDAL;
        private readonly RCategoriaDAL rCategoriaDAL;

        public RProdutoDAL(int idfuncionario) : base(idfuncionario)
        {
            restoque = new REstoqueDAL(idfuncionario);
            colaboradorDAL = new RColaboradorDAL(idfuncionario);
            rCategoriaDAL = new RCategoriaDAL(idfuncionario);
        }

        public void Delete(ProdutoModel t)
        {
            restoque.Delete(t.IdProduto);

            string query = "Update tb_produto SET Apagado = 1 WHERE IdProduto = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdProduto, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public ProdutoModel GetById(int id)
        {
            string query = SelectQuery + " where a.IdProduto = @filtro";
            var obj = GetLista(query, id.ToString()).FirstOrDefault();
            return obj;
        }

        public int Insert(ProdutoModel t)
        {
            string query = "INSERT INTO tb_produto (Nome, CodProduto, Descricao, IdMarcaF, ValorCusto, ValorVenda, DataCadastro, IdCategoria, IdFuncionario) " +
            " VALUES(@Nome,@CodProduto,@Descricao,@Marca,@ValorCusto,@ValorVenda,NOW(),@IdCategoria,@IdFuncionario);"
            + " SELECT LAST_INSERT_ID();";
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar 
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CodProduto", t.CodProduto);
            AddParameter(lista, "@Descricao", t.Descricao);
            AddParameter(lista, "@Marca", t.IdMarca);
            AddParameter(lista, "@ValorCusto", t.ValorCusto);
            AddParameter(lista, "@ValorVenda", t.ValorVenda);
            AddParameter(lista, "@IdCategoria", t.IdCategoria);
            AddParameter(lista, "@IdFuncionario", idFuncionario);
            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public List<ProdutoModel> List(string filtro)
        {
            string query = SelectQuery
                + " WHERE a.Nome LIKE CONCAT(@filtro, '%') OR IdFuncionario = @filtro OR CodProduto = @filtro OR IdMarcaF = @filtro AND Apagado = 0"
                + " ORDER BY a.Nome ASC" // ASC para ordenação ascendente (A-Z)
                + " LIMIT 200"; // Adicionando LIMIT 200 para limitar o número de registros

            return GetLista(query, filtro);
        }

        private List<ProdutoModel> GetLista(string query, string filtro)
        {
            List<ProdutoModel> list = new List<ProdutoModel>();
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@filtro", filtro);
            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                while (reader.Read())
                {
                    var obj = new ProdutoModel()
                    {
                        DataCadastro = DALHelper.GetDateTime(reader, "DataCadastro").GetValueOrDefault(),
                        Nome = DALHelper.GetString(reader, "NomeProduto"),
                        CodProduto = DALHelper.GetString(reader, "CodProduto"),
                        Descricao = DALHelper.GetString(reader, "DescricaoProduto"),
                        IdMarca = DALHelper.GetInt32(reader, "IdMarcaF").GetValueOrDefault(),
                        ValorCusto = DALHelper.GetDecimal(reader, "ValorCusto"),
                        ValorVenda = DALHelper.GetDecimal(reader, "ValorVenda"),
                        IdCadastrante = DALHelper.GetInt32(reader, "IdFuncionario").GetValueOrDefault(),
                        IdCategoria = DALHelper.GetInt32(reader, "IdCategoria").Value,
                        Estoque = new EstoqueModel
                        {
                            Localizacao = DALHelper.GetString(reader, "Localizacao"),
                        },
                        Categoria = new CategoriaModel
                        {
                            Nome = DALHelper.GetString(reader, "NomeCategoria"),
                            Descricao = DALHelper.GetString(reader, "DescricaoCategoria"),
                        }
                    };
                    obj.IdProduto = reader.GetInt32("IdProduto");
                    obj.Estoque.IdEstoque = DALHelper.GetInt32(reader, "IdEstoque").GetValueOrDefault();
                    obj.Estoque.Quantidade = reader.GetInt32("Quantidade");
                    //obj.IdCategoria = reader.GetInt32("IdCategoria");

                    obj.Colaborador = colaboradorDAL.GetById(obj.IdCadastrante);
                    list.Add(obj);
                }
            }
            return list;
        }


        public void Update(ProdutoModel t)
        {
            string query = "UPDATE tb_produto SET Nome = @Nome, CodProduto = @CodProduto"
                + " , Descricao = @Descricao, IdMarcaF = @Marca, ValorCusto = @ValorCusto, ValorVenda = @ValorVenda "
                + " , IdCategoria = @IdCategoria"
                + " WHERE IdProduto = @Id";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar 
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CodProduto", t.CodProduto);
            AddParameter(lista, "@Descricao", t.Descricao);
            AddParameter(lista, "@Marca", t.IdMarca);
            AddParameter(lista, "@ValorCusto", t.ValorCusto);
            AddParameter(lista, "@ValorVenda", t.ValorVenda);
            AddParameter(lista, "@Id", t.IdProduto);
            AddParameter(lista, "@IdCategoria", t.IdCategoria);
            ExecuteNonQuery(query, lista.ToArray());
        }
    }
}
