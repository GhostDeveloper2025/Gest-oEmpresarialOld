using GestãoEmpresarial.Validations;
using System;

namespace GestãoEmpresarial.Models
{
    public class EditarColaboradorModel : EditarBaseModel<ColaboradorModel>
    {
        public EditarColaboradorModel(ColaboradorValidar validar) : this(null, validar)
        {
            DataCadastro = DateTime.Now;
        }

        public EditarColaboradorModel(ColaboradorModel obj, ColaboradorValidar validar) : base(obj, validar)
        {
        }

        public override ColaboradorModel DevolveObjectoBD()
        {
            return new ColaboradorModel()
            {
                IdFuncionario = IdFuncionario,
                DataCadastro = DataCadastro,
                Nome = Nome,
                CPF = CPF,
                Telefone = Telefone,
                Email = Email,
                Senha = Senha,
                Cargo = Cargo,
                Comissao = Comissao,
                Ativo = Ativo,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(ColaboradorModel obj)
        {
            IdFuncionario = obj.IdFuncionario;
            DataCadastro = obj.DataCadastro;
            Nome = obj.Nome;
            CPF = obj.CPF;
            Telefone = obj.Telefone;
            Email = obj.Email;
            Senha = obj.Senha;
            Cargo = obj.Cargo;
            Comissao = obj.Comissao;
            Ativo = obj.Ativo;
        }

        public int IdFuncionario { get; set; }
        public DateTime DataCadastro { get; private set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }
        public decimal Comissao { get; set; }
        public bool Ativo { get; set; }
    }
}