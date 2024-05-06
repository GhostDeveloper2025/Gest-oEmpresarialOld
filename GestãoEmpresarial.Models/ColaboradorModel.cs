using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int IdFuncionario { get; set; }
        public DateTime DataCadastro { get; set; }
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
