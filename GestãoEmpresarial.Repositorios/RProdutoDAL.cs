using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mysqlx.Expect.Open.Types;

namespace GestãoEmpresarial.Repositorios
{
    public class RProdutoDAL : DatabaseConnection, IDAL<ProdutoModel>
    {
        private const string SelectQuery = "SELECT a.IdProduto, a.Nome AS NomeProduto"
                + " , CodProduto, a.Descricao AS DescricaoProduto, IdMarcaF, ValorCusto, ValorVenda"
                + " , DataCadastro, a.IdCategoria, IdFuncionario"
                + " , IdEstoque, Localizacao, Quantidade"
                + " , c.Nome AS NomeCategoria, c.Descricao AS DescricaoCategoria"
                + " FROM tb_produto a"
                + " INNER JOIN tb_estoque b ON a.IdProduto = b.IdProduto "
                + " INNER JOIN tb_categoria c ON c.IdCategoria = a.IdCategoria";

        private readonly REstoqueDAL restoque;
        private readonly RColaboradorDAL colaboradorDAL;
        private readonly RCategoriaDAL rCategoriaDAL;

        public RProdutoDAL(int idfuncionario) : base(idfuncionario)
        {
            restoque = new REstoqueDAL(idfuncionario);
            colaboradorDAL = new RColaboradorDAL(idfuncionario);
            rCategoriaDAL = new RCategoriaDAL(idfuncionario);
        }

        // Método assíncrono para deletar um produto e atualizar o estoque
        public async Task DeleteAsync(ProdutoModel t)
        {
            await restoque.DeleteByIdProdutoAsync(t.IdProduto); // Remover produto do estoque

            string query = "UPDATE tb_produto SET Apagado = 1 WHERE IdProduto = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdProduto, ParameterName = "@id" },
            };
            ExecuteNonQuery(query, arr); // Executa a query de forma assíncrona
        }

        // Busca um produto por Id de forma assíncrona
        public async Task<ProdutoModel> GetByIdAsync(int id)
        {
            var parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@filtro", id);

            string query = SelectQuery + " WHERE a.IdProduto = @filtro";
            var obj = (await GetListaAsync(query, parametros)).FirstOrDefault();
            return obj;
        }

        // Inserção assíncrona de um novo produto
        public async Task<int> InsertAsync(ProdutoModel t)
        {
            string query = "INSERT INTO tb_produto (Nome, CodProduto, Descricao, IdMarcaF, ValorCusto, ValorVenda, DataCadastro, IdCategoria, IdFuncionario) " +
                           "VALUES(@Nome, @CodProduto, @Descricao, @Marca, @ValorCusto, @ValorVenda, NOW(), @IdCategoria, @IdFuncionario); " +
                           "SELECT LAST_INSERT_ID();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
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
        //Nova Pesquisa Separando Buscas Normais Dos Providers
        //private string GetSearchConditions(string nome, string codigo, string localizacao, string marca, List<MySqlParameter> parametros, bool isAutoComplete = false)
        //{
        //    var conditions = new List<string>();

        //    if (isAutoComplete)
        //    {
        //        AddParameter(parametros, "@Filter", nome);
        //        conditions.Add("(a.Nome LIKE CONCAT(@Filter, '%') OR a.CodProduto LIKE CONCAT(@Filter, '%'))");
        //    }
        //    else
        //    {
        //        AddParameterCondition(parametros, conditions, "CodProduto", codigo);
        //        AddParameterCondition(parametros, conditions, "localizacao", localizacao);
        //        AddParameterCondition(parametros, conditions, "IdMarcaF", marca);

        //        if (!string.IsNullOrWhiteSpace(nome))
        //        {
        //            AddParameter(parametros, "@Nome", nome);
        //            conditions.Add("a.Nome LIKE CONCAT(@Nome, '%')");
        //        }
        //    }

        //    return string.Join(" OR ", conditions);
        //}



        // Este faz a pesquisa em qualquer parte do texto
        private string GetSearchConditions(string nome, string codigo, string localizacao, string marca, List<MySqlParameter> parametros, bool isAutoComplete = false)
        {
            var conditions = new List<string>();

            if (isAutoComplete)
            {
                AddParameter(parametros, "@Filter", $"%{nome}%");
                conditions.Add("(a.Nome LIKE @Filter OR a.CodProduto LIKE @Filter)");
            }
            else
            {
                AddParameterCondition(parametros, conditions, "CodProduto", codigo);
                AddParameterCondition(parametros, conditions, "localizacao", localizacao);
                AddParameterCondition(parametros, conditions, "IdMarcaF", marca);

                if (!string.IsNullOrWhiteSpace(nome))
                {
                    AddParameter(parametros, "@Nome", $"%{nome}%");
                    conditions.Add("a.Nome LIKE @Nome");
                }
            }

            return string.Join(" OR ", conditions);
        }


        // Métodos ListAsync e ListForAutoCompleteAsync que usam o GetSearchConditions

        public async Task<List<ProdutoModel>> ListAsync(string pesquisaNome, string pesquisaCodigo, string pesquisaLocalizacao, string pesquisaMarca)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            string conditionsJoin = GetSearchConditions(pesquisaNome, pesquisaCodigo, pesquisaLocalizacao, pesquisaMarca, parametros);
            string query = SelectQuery + " WHERE " + conditionsJoin + " ORDER BY a.Nome ASC LIMIT 200";

            return await GetListaAsync(query, parametros);
        }

        public async Task<List<ProdutoModel>> ListForAutoCompleteAsync(string filter)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            string conditionsJoin = GetSearchConditions(filter, null, null, null, parametros, isAutoComplete: true);
            string query = SelectQuery + " WHERE " + conditionsJoin + " ORDER BY a.Nome ASC LIMIT 50";

            return await GetListaAsync(query, parametros);
        }

        /// <summary>
        /// *   Coleta de IDs de colaboradores: A lógica agora coleta todos os IDs de colaboradores antes de entrar no loop,
        ///       evitando múltiplas chamadas assíncronas.
        ///     
        ///  *   Chamada única para obter colaboradores: Uma vez que todos os IDs foram coletados, uma única chamada é feita
        ///        para obter todos os colaboradores correspondentes.
        ///        
        ///  *   Atribuição de colaboradores: Depois de obter a lista de colaboradores, a lógica atribui cada colaborador ao
        ///       respectivo produto.
        ///       
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private async Task<List<ProdutoModel>> GetListaAsync(string query, List<MySqlParameter> lista)
        {
            List<ProdutoModel> list = new List<ProdutoModel>();

            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                // Coletar todos os IDs de colaboradores
                var idsColaboradores = new HashSet<int>();

                while (await reader.ReadAsync())
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

                    // Adicionar ID do colaborador à lista
                    idsColaboradores.Add(obj.IdCadastrante);

                    list.Add(obj);
                }

                // Buscar todos os colaboradores em uma única chamada
                var colaboradores = await colaboradorDAL.GetByIdsAsync(idsColaboradores.ToList());

                // Atribuir os colaboradores aos produtos
                foreach (var produto in list)
                {
                    // Use a propriedade IdFuncionario em vez de Id
                    produto.Colaborador = colaboradores.FirstOrDefault(c => c.IdFuncionario == produto.IdCadastrante);
                }

            }
            return list;
        }
        #region Este codigo nao salva a data ao atualizar
        //public async Task UpdateAsync(ProdutoModel t)
        //{
        //    string query = "UPDATE tb_produto SET Nome = @Nome, CodProduto = @CodProduto, Descricao = @Descricao, " +
        //                   "IdMarcaF = @Marca, ValorCusto = @ValorCusto, ValorVenda = @ValorVenda, IdCategoria = @IdCategoria " +
        //                   "WHERE IdProduto = @Id;";

        //    List<MySqlParameter> lista = new List<MySqlParameter>();
        //    AddParameter(lista, "@Nome", t.Nome);
        //    AddParameter(lista, "@CodProduto", t.CodProduto);
        //    AddParameter(lista, "@Descricao", t.Descricao);
        //    AddParameter(lista, "@Marca", t.IdMarca);
        //    AddParameter(lista, "@ValorCusto", t.ValorCusto);
        //    AddParameter(lista, "@ValorVenda", t.ValorVenda);
        //    AddParameter(lista, "@IdCategoria", t.IdCategoria);
        //    AddParameter(lista, "@Id", t.IdProduto);

        //    ExecuteNonQuery(query, lista.ToArray());
        //}
        #endregion

        // Salva a data ao atualizar
        public async Task UpdateAsync(ProdutoModel t)
        {
            string query = "UPDATE tb_produto SET Nome = @Nome, CodProduto = @CodProduto, Descricao = @Descricao, " +
                           "IdMarcaF = @Marca, ValorCusto = @ValorCusto, ValorVenda = @ValorVenda, IdCategoria = @IdCategoria, " +
                           "DataCadastro = NOW() " +  // Atualiza a data de cadastro para a data atual
                           "WHERE IdProduto = @Id;";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CodProduto", t.CodProduto);
            AddParameter(lista, "@Descricao", t.Descricao);
            AddParameter(lista, "@Marca", t.IdMarca);
            AddParameter(lista, "@ValorCusto", t.ValorCusto);
            AddParameter(lista, "@ValorVenda", t.ValorVenda);
            AddParameter(lista, "@IdCategoria", t.IdCategoria);
            AddParameter(lista, "@Id", t.IdProduto);

            //await ExecuteNonQueryAsync(query, lista.ToArray());

            ExecuteNonQuery(query, lista.ToArray());
        }

        public ProdutoModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}
