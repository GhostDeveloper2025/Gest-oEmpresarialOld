using MySql.Data.MySqlClient;
using GestãoEmpresarial.Helpers;
using GestãoEmpresarial.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using GestãoEmpresarial.Models;

namespace GestãoEmpresarial.Repositorios
{
    public class RColaboradorDAL : DatabaseConnection, IDAL<ColaboradorModel>
    {
        public RColaboradorDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public void Delete(ColaboradorModel t)
        {
            string query = "UPDATE tb_funcionario SET Ativo = 0 WHERE IdFuncionario = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.IdFuncionario, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }
        
        public ColaboradorModel GetById(int id)
        {
            return List(id.ToString()).FirstOrDefault();
        }

        public int Insert(ColaboradorModel t)
        {
            string query = "INSERT INTO tb_funcionario (DataCadastro, Nome, CPF, Telefone, Email, Senha, Cargo, Comissao) " +
            " VALUES(NOW(), @Nome, @CPF, @Telefone, @Email, @Senha, @Cargo, @Comissao);"
            + " SELECT last_insert_id()";
            //Inicia o objeto
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

        public List<ColaboradorModel> List(string filtro)
        {
            List<ColaboradorModel> list = new List<ColaboradorModel>();
            string query = "SELECT IdFuncionario, DataCadastro, Nome, CPF, Telefone, Email, Senha, Cargo, Comissao, Ativo FROM tb_funcionario"
                + " WHERE (@filtro IS NOT NULL AND (Nome LIKE CONCAT(@filtro, '%')  OR Idfuncionario = @filtro))";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@filtro", filtro);
            using (MySqlDataReader reader = ExecuteReader(query, lista.ToArray()))
            {
                
                while (reader.Read())
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

        public void Update(ColaboradorModel t)
        {
             string query = "UPDATE tb_funcionario SET Nome = @Nome, CPF = @CPF, Telefone = @Telefone, Email = @Email, Senha = @Senha, Cargo = @Cargo, Comissao = @Comissao " +
             " WHERE IdFuncionario = @Id";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@Id", t.IdFuncionario);
            AddParameter(lista, "@Nome", t.Nome);
            AddParameter(lista, "@CPF", t.CPF);
            AddParameter(lista, "@Telefone", t.Telefone);
            AddParameter(lista, "@Email", t.Email);
            AddParameter(lista, "@Senha", t.Senha);
            AddParameter(lista, "@Cargo", t.Cargo);
            AddParameter(lista, "@Comissao", t.Comissao);
            ExecuteNonQuery(query, lista.ToArray());
        }
    }
}
