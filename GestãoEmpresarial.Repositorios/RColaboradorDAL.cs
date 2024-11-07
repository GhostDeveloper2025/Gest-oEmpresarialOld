using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using GestãoEmpresarial.Models;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RColaboradorDAL : DatabaseConnection, IDAL<ColaboradorModel>
    {
        public RColaboradorDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        // Método assíncrono para deletar um colaborador (inativar)
        public async Task DeleteAsync(ColaboradorModel t)
        {
            string query = "UPDATE tb_funcionario SET Ativo = 0 WHERE IdFuncionario = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdFuncionario, ParameterName = "@id" },
            };
            ExecuteNonQuery(query, arr); // Aguarda a execução assíncrona
        }

        // Busca um colaborador por ID de forma assíncrona
        public async Task<ColaboradorModel> GetByIdAsync(int id)
        {
            var lista = await ListAsync(id.ToString());
            return lista.FirstOrDefault();
        }

        // Inserção de um colaborador de forma assíncrona
        public async Task<int> InsertAsync(ColaboradorModel t)
        {
            string query = "INSERT INTO tb_funcionario (DataCadastro, Nome, CPF, Telefone, Email, Senha, Cargo, Comissao) " +
                           "VALUES(NOW(), @Nome, @CPF, @Telefone, @Email, @Senha, @Cargo, @Comissao); " +
                           "SELECT last_insert_id();";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CPF", t.CPF);
            AddParameter(lista, "@Telefone", t.Telefone);
            AddParameter(lista, "@Email", t.Email);
            AddParameter(lista, "@Senha", t.Senha);
            AddParameter(lista, "@Cargo", t.Cargo);
            AddParameter(lista, "@Comissao", t.Comissao);

            object id = ExecuteScalar(query, lista.ToArray());
            return Convert.ToInt32(id);
        }
        //
        //Codigo GPT fez 
        
        public async Task<List<ColaboradorModel>> GetByIdsAsync(List<int> ids)
        {
            List<ColaboradorModel> list = new List<ColaboradorModel>();
            if (ids == null || ids.Count == 0)
            {
                return list; // Retorna uma lista vazia se não houver IDs
            }

            // Constrói a consulta SQL
            string query = "SELECT IdFuncionario, DataCadastro, Nome, CPF, Telefone, Email, Senha, Cargo, Comissao, Ativo " +
                           "FROM tb_funcionario WHERE IdFuncionario IN (" + string.Join(",", ids) + ")";

            using (MySqlDataReader reader = ExecuteReader(query, null))
            {
                while (await reader.ReadAsync())
                {
                    var obj = new ColaboradorModel()
                    {
                        DataCadastro = DALHelper.GetDateTime(reader, "DataCadastro").GetValueOrDefault(),
                        IdFuncionario = reader.GetInt32("IdFuncionario"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        CPF = DALHelper.GetString(reader, "CPF"),
                        Telefone = DALHelper.GetString(reader, "Telefone"),
                        Email = DALHelper.GetString(reader, "Email"),
                        Senha = DALHelper.GetString(reader, "Senha"),
                        Cargo = DALHelper.GetString(reader, "Cargo"),
                        Comissao = DALHelper.GetDecimal(reader, "Comissao"),
                        Ativo = DALHelper.GetBool(reader, "Ativo"),
                    };
                    list.Add(obj);
                }
            }
            return list;
        }


        //
        // Método assíncrono que retorna uma lista de colaboradores
        public async Task<List<ColaboradorModel>> ListAsync(string filtro)
        {
            List<ColaboradorModel> list = new List<ColaboradorModel>();
            string query = "SELECT IdFuncionario, DataCadastro, Nome, CPF, Telefone, Email, Senha, Cargo, Comissao, Ativo FROM tb_funcionario " +
                           "WHERE (@filtro IS NOT NULL AND (Nome LIKE CONCAT(@filtro, '%') OR IdFuncionario = @filtro))";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@filtro", filtro);

            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                while (await reader.ReadAsync()) // Usando ReadAsync para leitura assíncrona
                {
                    var obj = new ColaboradorModel()
                    {
                        DataCadastro = DALHelper.GetDateTime(reader, "DataCadastro").GetValueOrDefault(),
                        IdFuncionario = reader.GetInt32("IdFuncionario"),
                        Nome = DALHelper.GetString(reader, "Nome"),
                        CPF = DALHelper.GetString(reader, "CPF"),
                        Telefone = DALHelper.GetString(reader, "Telefone"),
                        Email = DALHelper.GetString(reader, "Email"),
                        Senha = DALHelper.GetString(reader, "Senha"),
                        Cargo = DALHelper.GetString(reader, "Cargo"),
                        Comissao = DALHelper.GetDecimal(reader, "Comissao"),
                        Ativo = DALHelper.GetBool(reader, "Ativo"),
                    };
                    list.Add(obj);
                }
            }
            return list;
        }

        // Atualiza um colaborador de forma assíncrona
        public async Task UpdateAsync(ColaboradorModel t)
        {
            string query = "UPDATE tb_funcionario SET Nome = @Nome, CPF = @CPF, Telefone = @Telefone, " +
                           "Email = @Email, Senha = @Senha, Cargo = @Cargo, Comissao = @Comissao " +
                           "WHERE IdFuncionario = @Id";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdFuncionario);
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CPF", t.CPF);
            AddParameter(lista, "@Telefone", t.Telefone);
            AddParameter(lista, "@Email", t.Email);
            AddParameter(lista, "@Senha", t.Senha);
            AddParameter(lista, "@Cargo", t.Cargo);
            AddParameter(lista, "@Comissao", t.Comissao);

            ExecuteNonQuery(query, lista.ToArray()); // Executa o método de forma assíncrona
        }

        // Implementações da interface IDAL<T>
        public async Task Delete(ColaboradorModel t)
        {
            await DeleteAsync(t);
        }

        public async Task Update(ColaboradorModel t)
        {
            await UpdateAsync(t);
        }

        public async Task<int> Insert(ColaboradorModel t)
        {
            return await InsertAsync(t);
        }

        public ColaboradorModel GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public List<ColaboradorModel> List(string filtro)
        {
            return ListAsync(filtro).Result; // Para manter compatibilidade síncrona, se necessário
        }
    }
}

