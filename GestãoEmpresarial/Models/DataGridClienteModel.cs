using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class DataGridClienteModel
    {
        public DataGridClienteModel(ClienteModel model)
        {
            Idcliente = model.Idcliente;
            DataCadastro = model.DataCadastro.ToString();
            Nome = model.Nome;
            CPF = model.CPF;
            Logradouro = model.Logradouro;
            Localidade = model.Localidade;
            UF = model.UF;
            Bairro = model.Bairro;
            Numero = model.Numero;
            Cep = model.Cep;
            Telefone = model.Telefone;
            Celular = model.Celular;
            Email = model.Email;
            LimiteCredito = model.LimiteCredito;
            CNPJ = model.CNPJ;
            NomeColaborador = model.Colaborador.Nome;
        }

        public int Idcliente { get; set; }
       
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        [DisplayName("Celular")]
        public string Celular { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Rua")]
        public string Logradouro { get; set; }

        //[DisplayName("Cidade")]
        public string Localidade { get; set; }

        //[DisplayName("Data Cadastro")]
        public string DataCadastro { get; set; }

        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        
        public string Email { get; set; }

        //[DisplayName("Limite de Crédito")]
        public decimal LimiteCredito { get; set; }

        //[DisplayName("Cadastrante")]
        public string NomeColaborador { get; set; }
    }
}