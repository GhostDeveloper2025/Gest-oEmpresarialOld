using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class ClienteModel
    {
        public int Idcliente { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string CPFNum { get { return CPF.GetNumber(); } }
        public string Logradouro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string CelularNum { get { return Celular.GetNumber(); } }
        public string CNPJ { get; set; }
        public string CNPJNum { get { return CNPJ.GetNumber(); } }
        public string Email { get; set; }
        public decimal LimiteCredito { get; set; }
        public int IdCadastrante { get; set; }
        public string EnderecoCompleto { get { return String.Format("Endereço: {0},{1} - {2}, {3} ", Logradouro, Localidade, UF, Cep); } }
        public ColaboradorModel Colaborador { get; set; }
    }
}
