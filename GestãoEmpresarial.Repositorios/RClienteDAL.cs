using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RClienteDAL : DatabaseConnection, IDAL<ClienteModel>
    {
        public RClienteDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        // Método assíncrono para deletar um cliente
        public async Task DeleteAsync(ClienteModel t)
        {
            string query = "DELETE FROM tb_cliente WHERE Idcliente = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.Idcliente, ParameterName = "@id" },
            };
            ExecuteNonQuery(query, arr);  // Aguarda a execução assíncrona
        }

        // Busca um cliente por ID de forma assíncrona
        public async Task<ClienteModel> GetByIdAsync(int id)
        {
            var lista = await ListAsync(id, null, null, null, null);
            return lista.FirstOrDefault();
        }

        // Inserção de um cliente de forma assíncrona
        public async Task<int> InsertAsync(ClienteModel t)
        {
            string query = "INSERT INTO tb_cliente (DataCadastro, Nome, CPF, Logradouro, Localidade, UF, Bairro, Numero, Cep, Telefone, Celular, Email, IdFuncionario, LimiteCredito, CNPJ) " +
                           "VALUES(NOW(), @Nome, @CPF, @Logradouro, @Localidade, @UF, @Bairro, @Numero, @Cep, @Telefone, @Celular, @Email, @IdFuncionario, @LimiteCredito, @CNPJ); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CPF", t.CPFNum);
            AddParameter(lista, "@CNPJ", t.CNPJNum);
            AddParameter(lista, "@Logradouro", t.Logradouro);
            AddParameter(lista, "@Localidade", t.Localidade);
            AddParameter(lista, "@UF", t.UF);
            AddParameter(lista, "@Bairro", t.Bairro);
            AddParameter(lista, "@Numero", t.Numero);
            AddParameter(lista, "@Cep", t.Cep);
            AddParameter(lista, "@Telefone", t.Telefone);
            AddParameter(lista, "@Celular", t.Celular);
            AddParameter(lista, "@Email", t.Email);
            AddParameter(lista, "@LimiteCredito", t.LimiteCredito);
            AddParameter(lista, "@IdFuncionario", idFuncionario);

            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }

        public async Task<List<ClienteModel>> GetByIdsAsync(List<int> ids)
        {
            List<ClienteModel> lista = new List<ClienteModel>();

            if (ids == null || ids.Count == 0)
                return lista; // Retorna uma lista vazia se não houver IDs

            // Cria uma string de parâmetros para a cláusula IN da consulta
            string parameterList = string.Join(",", ids.Select((id, index) => $"@id{index}"));

            string query = $"SELECT Idcliente, DataCadastro, Nome, CPF, Logradouro, Localidade, UF, Bairro, Numero, Cep, Telefone, Celular, Email, LimiteCredito, IdFuncionario, CNPJ " +
                           $"FROM tb_cliente WHERE Idcliente IN ({parameterList})";

            List<MySqlParameter> parameters = ids.Select((id, index) => new MySqlParameter($"@id{index}", id)).ToList();

            using (MySqlDataReader reader = ExecuteReader(query, parameters.ToArray()))
            {
                while (await reader.ReadAsync())
                {
                    var obj = new ClienteModel()
                    {
                        // Mapeie os campos do banco de dados para o modelo ClienteModel
                        Idcliente = reader.GetInt32("Idcliente"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        CPF = DALHelper.GetString(reader, "CPF"),
                        Celular = DALHelper.GetString(reader, "Celular"),
                        // Adicione os outros campos conforme necessário...
                    };
                    lista.Add(obj);
                }
            }

            return lista;
        }

        //
        // Método assíncrono que retorna uma lista de clientes
        public async Task<List<ClienteModel>> ListAsync(int? id, string pesquisaCPF, string pesquisaCNPJ, string pesquisaCelular, string pesquisaNome)
        {
            var parametros = new List<MySqlParameter>();
            var conditions = new List<string>();
            AddParameterCondition(parametros, conditions, "Idcliente", id);
            AddParameterCondition(parametros, conditions, "CPF", pesquisaCPF);
            AddParameterCondition(parametros, conditions, "CNPJ", pesquisaCNPJ);
            AddParameterCondition(parametros, conditions, "Celular", pesquisaCelular);

            if (string.IsNullOrWhiteSpace(pesquisaNome) == false)
            {
                AddParameter(parametros, "@Nome", pesquisaNome);
                conditions.Add("Nome LIKE CONCAT(@Nome, '%')");
            }

            // Consulta SQL
            string conditionsJoin = string.Join(" OR ", conditions);
            string query = @"SELECT Idcliente, DataCadastro, Nome, CPF, Logradouro, Localidade, UF,
                Bairro, Numero, Cep, Telefone, Celular, Email, LimiteCredito, IdFuncionario, CNPJ
                FROM tb_cliente WHERE " + conditionsJoin + " ORDER BY Nome ASC LIMIT 200";

            List<ClienteModel> lista = new List<ClienteModel>();
            // Usando await para garantir que a leitura seja feita de maneira assíncrona
            using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
            {
                // Instanciando RColaboradorDAL
                RColaboradorDAL colaboradorDAL = new RColaboradorDAL(idFuncionario);

                // Loop para ler os resultados da consulta
                while (await reader.ReadAsync()) // Usando ReadAsync para leitura assíncrona
                {
                    // Criando o objeto ClienteModel
                    var obj = new ClienteModel()
                    {
                        DataCadastro = DALHelper.GetDateTime(reader, "DataCadastro").GetValueOrDefault(),
                        Idcliente = reader.GetInt32("Idcliente"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        CPF = DALHelper.GetString(reader, "CPF"),
                        CNPJ = DALHelper.GetString(reader, "CNPJ"),
                        Logradouro = DALHelper.GetString(reader, "Logradouro"),
                        Localidade = DALHelper.GetString(reader, "Localidade"),
                        UF = DALHelper.GetString(reader, "UF"),
                        Bairro = DALHelper.GetString(reader, "Bairro"),
                        Numero = DALHelper.GetString(reader, "Numero"),
                        Cep = DALHelper.GetString(reader, "Cep"),
                        Telefone = DALHelper.GetString(reader, "Telefone"),
                        Celular = DALHelper.GetString(reader, "Celular"),
                        Email = DALHelper.GetString(reader, "Email"),
                        LimiteCredito = DALHelper.GetDecimal(reader, "LimiteCredito"),
                        IdCadastrante = reader.GetInt32("IdFuncionario")
                    };

                    // Associando o colaborador
                    obj.Colaborador = await colaboradorDAL.GetByIdAsync(obj.IdCadastrante);

                    // Adicionando à lista
                    lista.Add(obj);
                }
            }

            // Retorna a lista
            return lista;
        }

        // Atualiza um cliente de forma assíncrona
        public async Task UpdateAsync(ClienteModel t)
        {
            string query = "UPDATE tb_cliente SET Nome = @Nome, CPF = @CPF, Logradouro = @Logradouro, " +
                           "Localidade = @Localidade, UF = @UF, Bairro = @Bairro, Numero = @Numero, " +
                           "Cep = @Cep, Telefone = @Telefone, Celular = @Celular, Email = @Email, " +
                           "LimiteCredito = @LimiteCredito, CNPJ = @CNPJ " +
                           "WHERE Idcliente = @Id";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CPF", t.CPFNum);
            AddParameter(lista, "@CNPJ", t.CNPJNum);
            AddParameter(lista, "@Logradouro", t.Logradouro);
            AddParameter(lista, "@Localidade", t.Localidade);
            AddParameter(lista, "@UF", t.UF);
            AddParameter(lista, "@Bairro", t.Bairro);
            AddParameter(lista, "@Numero", t.Numero);
            AddParameter(lista, "@Cep", t.Cep);
            AddParameter(lista, "@Telefone", t.Telefone);
            AddParameter(lista, "@Celular", t.Celular);
            AddParameter(lista, "@Email", t.Email);
            AddParameter(lista, "@LimiteCredito", t.LimiteCredito);
            AddParameter(lista, "@Id", t.Idcliente);

            ExecuteNonQuery(query, lista.ToArray());  // Executa o método de forma assíncrona
        }

        public ClienteModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}

