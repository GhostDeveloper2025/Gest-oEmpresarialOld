using System;
using GestãoEmpresarial.Models.Atributos;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class RelatorioProdutoMaisVendido
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Localizacao { get; set; }
        
        public int Quantidade { get; set; }
        public decimal ValUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal CustoTotal { get; set; }

        public DateTime DataFinalizacao { get; set; }
    }
}
