using System;
using System.ComponentModel;

namespace GestãoEmpresarial.Models
{
    public class ColaboradorModel
    {
        public ColaboradorModel(DateTime DataCadastro)
        {
            _DataCadastro = DataCadastro;
        }

        public ColaboradorModel() : this(DateTime.Now)
        {
        }
        private DateTime _DataCadastro;

        [DisplayName("Id")]
        public int IdFuncionario { get; set; }

        [DisplayName("Data Cadastro")]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }
        public decimal Comissao { get; set; }
        public bool Ativo { get; set; }
    }
}
