using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestãoEmpresarial.Repositorios
{
    public class RClienteDAL : DatabaseConnection, IDAL<ClienteModel>
    {
        public RClienteDAL(int idfuncionario) : base(idfuncionario)
        {
        }

        public void Delete(ClienteModel t)
        {
            string query = "DELETE FROM tb_cliente WHERE Idcliente = @id";
            MySqlParameter[] arr = new MySqlParameter[]
            {
                new MySqlParameter() { Value = t.Idcliente, ParameterName= "@id" },
            };
            ExecuteNonQuery(query, arr);
        }

        public ClienteModel GetById(int id)
        {
            return List(id.ToString()).FirstOrDefault();
        }

        public int Insert(ClienteModel t)
        {
            string query = "INSERT INTO tb_cliente (DataCadastro, Nome, CPF, Logradouro, Localidade, UF, Bairro, Numero, Cep, Telefone, Celular, Email, IdFuncionario,LimiteCredito,CNPJ) " +
            " VALUES(NOW(),@Nome,@CPF,@Logradouro,@Localidade,@UF,@Bairro,@Numero,@Cep,@Telefone,@Celular,@Email,@IdFuncionario,@LimiteCredito,@CNPJ);"
            + " SELECT last_insert_id()";
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar
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

        #region Codigo List Para filtrar
        public List<ClienteModel> List(string filtro)
        {
            List<ClienteModel> lista = new List<ClienteModel>();
            //Where com o('%', @filtro) faz a busca em qualquer parte do texto e('%', @filtro) faz apenas no inicio a primeira letra
            // Consulta SQL usando interpolação de strings para melhor legibilidade
            string query = $@"
                            SELECT Idcliente, DataCadastro, Nome, CPF, Logradouro, Localidade, UF,
                            Bairro, Numero, Cep, Telefone, Celular, Email, LimiteCredito,
                            IdFuncionario, CNPJ
                            FROM tb_cliente 
                            WHERE Nome LIKE CONCAT(@filtro, '%') OR Idcliente = @filtro OR Celular = @filtro OR CPF = @filtro 
                            ORDER BY Nome ASC 
                            LIMIT 200"; // Adicionando ORDER BY para ordenação alfabética e LIMIT 200 para limitar o número de registros

            // Lista de parâmetros
            List<MySqlParameter> parametros = new List<MySqlParameter>();
            AddParameter(parametros, "@filtro", filtro);

                // Utilizando a instrução 'using' para garantir que os recursos são liberados corretamente
                using (MySqlDataReader reader = ExecuteReader(query, parametros.ToArray()))
                {
                    // Criando uma instância do RColaboradorDAL fora do loop, se a lógica de criação for compartilhada
                    RColaboradorDAL colaboradorDAL = new RColaboradorDAL(idFuncionario);

                    // Loop para ler os resultados da consulta
                    while (reader.Read())
                    {
                        // Sua lógica existente para criar objetos ClienteModel
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

                        // Associando o colaborador ao objeto ClienteModel
                        obj.Colaborador = colaboradorDAL.GetById(obj.IdCadastrante);

                        // Adicionando o objeto à lista
                        lista.Add(obj);
                    }
                }

            // Retornando a lista de objetos ClienteModel
            return lista;
        }

        #endregion
        public void Update(ClienteModel t)
        {
            string query = "UPDATE tb_cliente SET Nome = @Nome, CPF = @CPF, Logradouro = @Logradouro"
                + " , Localidade = @Localidade, UF = @UF, Bairro = @Bairro, Bairro = @Bairro, Numero = @Numero"
                + " , Cep = @Cep, Telefone = @Telefone, Celular = @Celular, Email = @Email, LimiteCredito = @LimiteCredito "
                + " , CNPJ = @CNPJ"
                + " WHERE Idcliente = @Id";
            //Inicia o objeto
            List<MySqlParameter> lista = new List<MySqlParameter>();
            //lAcessoDados.LimparParametro();// Limpar parâmetro salvar 
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
            ExecuteNonQuery(query, lista.ToArray());
        }

    }
}
