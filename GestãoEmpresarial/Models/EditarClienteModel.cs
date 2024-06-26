using GestãoEmpresarial.Validations;
using System;

namespace GestãoEmpresarial.Models
{
    public class EditarClienteModel : EditarBaseModel<ClienteModel>
    {
        public EditarClienteModel(ClienteValidar validar) : this(null, validar)
        {
            DataCadastro = DateTime.Now;
        }

        public EditarClienteModel(ClienteModel obj, ClienteValidar validar) : base(obj, validar)
        {
        }

        public override ClienteModel DevolveObjectoBD()
        {
            return new ClienteModel()
            {
                Idcliente = Idcliente,
                DataCadastro = DataCadastro,
                Nome = Nome,
                CPF = CPF,
                Logradouro = Logradouro,
                Localidade = Localidade,
                UF = UF,
                Bairro = Bairro,
                Numero = Numero,
                Cep = Cep,
                Telefone = Telefone,
                Celular = Celular,
                CNPJ = CNPJ,
                Email = Email,
                LimiteCredito = LimiteCredito,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(ClienteModel obj)
        {
            Idcliente = obj.Idcliente;
            DataCadastro = obj.DataCadastro;
            Nome = obj.Nome;
            CPF = obj.CPF;
            Logradouro = obj.Logradouro;
            Localidade = obj.Localidade;
            UF = obj.UF;
            Bairro = obj.Bairro;
            Numero = obj.Numero;
            Cep = obj.Cep;
            Telefone = obj.Telefone;
            Celular = obj.Celular;
            Email = obj.Email;
            LimiteCredito = obj.LimiteCredito;
            CNPJ = obj.CNPJ;
        }

        public int Idcliente { get; set; }
        public DateTime DataCadastro { get; private set; }
        public string Nome { get; set; }
        public string CPF { get; set; }

        // public string CPFNum { get { return CPF.GetNumber(); } }
        public string Logradouro { get; set; }

        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string CNPJ { get; set; }

        //public string CNPJNum { get { return CNPJ.GetNumber(); } }
        public string Email { get; set; }

        public decimal LimiteCredito { get; set; }
        public int IdCadastrante { get; set; }

        public string EnderecoCompleto
        { get { return String.Format("Endereço: {0},{1} - {2}, {3} ", Logradouro, Localidade, UF, Cep); } }

        public ColaboradorModel Colaborador { get; set; }
    }
}